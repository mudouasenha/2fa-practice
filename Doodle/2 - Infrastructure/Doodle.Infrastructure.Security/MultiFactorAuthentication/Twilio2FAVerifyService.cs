using Doodle.Domain.Entities;
using Doodle.Infrastructure.Security.Models.Options;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service.Entity;

namespace Doodle.Infrastructure.Security.MultiFactorAuthentication
{
    public class Twilio2FAVerifyService : IVerification
    {
        private readonly ILogger<Twilio2FAVerifyService> _logger;
        private readonly IOptions<TwilioOptions> _twilioOptions;

        public Twilio2FAVerifyService(ILogger<Twilio2FAVerifyService> logger, IOptions<TwilioOptions> twilioOptions)
        {
            _logger = logger;
            _twilioOptions = twilioOptions;
            TwilioClient.Init(_twilioOptions.Value.AccountSid, _twilioOptions.Value.AuthToken);
        }

        public async Task<VerificationResult> VerifyResource(ApplicationUser user, string payload)
        {
            var factors = await ReadResource(user);
            var totpFactor = factors.FirstOrDefault(p => p.FactorType == FactorResource.FactorTypesEnum.Totp);

            var factor = FactorResource.Update(
                authPayload: payload,
                pathServiceSid: _twilioOptions.Value.ServiceSid,
                pathIdentity: user.MfaIdentity,
                pathSid: totpFactor.Sid
            );

            return factor.Status.Equals(FactorResource.FactorStatusesEnum.Verified) ?
                new VerificationResult(factor.Status.ToString()) :
                new VerificationResult(new List<string> { "Wrong code. Try again." });
        }

        public async Task<(VerificationResult, string)> StartVerificationAsync(ApplicationUser user)
        {
            try
            {
                if (user.MfaIdentity != default)
                    return (new VerificationResult(new List<string> { "Already created" }), user.MfaIdentity);

                var (resource, userIdentity) = await CreateVerificationResource();

                return (new VerificationResult(resource.Binding), userIdentity);
            }
            catch (TwilioException e)
            {
                return (new VerificationResult(new List<string> { e.Message }), user.MfaIdentity);
            }
        }

        public async Task<VerificationResult> CheckVerificationAsync(ApplicationUser user, string code)
        {
            try
            {
                var factors = await ReadResource(user);
                var totpFactor = factors.FirstOrDefault(p => p.FactorType == FactorResource.FactorTypesEnum.Totp);

                if (totpFactor == default)
                    return new VerificationResult(new List<string> { "No Totp factor found." });

                var challenge = await ChallengeResource.CreateAsync(
                    authPayload: code,
                    factorSid: totpFactor.Sid,
                    pathServiceSid: _twilioOptions.Value.ServiceSid,
                    pathIdentity: user.MfaIdentity
                );

                return challenge.Status.Equals(ChallengeResource.ChallengeStatusesEnum.Approved) ?
                    new VerificationResult(challenge.Status.ToString()) :
                    new VerificationResult(new List<string> { "Wrong code. Try again." });
            }
            catch (Exception e)
            {
                return new VerificationResult(new List<string> { e.Message });
            }
        }

        private async Task<(NewFactorResource, string)> CreateVerificationResource()
        {
            var userIdentity = Guid.NewGuid().ToString();

            var verification = await NewFactorResource.CreateAsync(
                    friendlyName: _twilioOptions.Value.ServiceName,
                    factorType: NewFactorResource.FactorTypesEnum.Totp,
                    pathServiceSid: _twilioOptions.Value.ServiceSid,
                    pathIdentity: userIdentity
                );

            _logger.LogInformation(verification.Binding.ToString());
            return (verification, userIdentity);
        }

        private async Task<List<FactorResource>> ReadResource(ApplicationUser user)
        {
            var factors = await FactorResource.ReadAsync(
                pathServiceSid: _twilioOptions.Value.ServiceSid,
                pathIdentity: user.MfaIdentity,
                limit: 20
            );

            return factors.ToList();
        }
    }
}