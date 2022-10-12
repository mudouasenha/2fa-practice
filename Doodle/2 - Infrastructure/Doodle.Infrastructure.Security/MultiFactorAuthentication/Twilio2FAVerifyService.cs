using Doodle.Domain.Entities;
using Doodle.Infrastructure.Security.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;
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

        public async Task<string> CreateVerificationResource(User user)
        {
            TwilioClient.Init(_twilioOptions.Value.AccountSid, _twilioOptions.Value.AuthToken);

            var userIdentity = Guid.NewGuid().ToString();

            var verification = await NewFactorResource.CreateAsync(
                    friendlyName: _twilioOptions.Value.ServiceName,
                    factorType: NewFactorResource.FactorTypesEnum.Totp,
                    pathServiceSid: _twilioOptions.Value.ServiceSid,
                    pathIdentity: userIdentity
                );

            _logger.LogInformation(verification.Binding.ToString());
            return userIdentity;
        }

        private async Task<List<FactorResource>> ReadResource(User user)
        {
            TwilioClient.Init(_twilioOptions.Value.AccountSid, _twilioOptions.Value.AuthToken);

            var factors = FactorResource.Read(
                pathServiceSid: _twilioOptions.Value.ServiceSid,
                pathIdentity: user.MfaIdentity,
                limit: 20
            );

            return factors.ToList();
        }

        public async Task<bool> VerifyResource(User user, string payload)
        {
            TwilioClient.Init(_twilioOptions.Value.AccountSid, _twilioOptions.Value.AuthToken);

            var factors = await ReadResource(user);
            foreach (var record in factors)
            {
                var factor = FactorResource.Update(
                    authPayload: payload,
                    pathServiceSid: _twilioOptions.Value.ServiceSid,
                    pathIdentity: user.MfaIdentity,
                    pathSid: record.Sid
                );

                //return verificationCheckResource.Status.Equals("approved") ?
                //    new VerificationResult(verificationCheckResource.Sid) :
                //    new VerificationResult(new List<string> { "Wrong code. Try again." });
            }

            return false;
        }

        public async Task<VerificationResult> StartVerificationAsync(User user, string channel)
        {
            try
            {
                var factors = await ReadResource(user);

                var verificationResource = await VerificationResource.CreateAsync(
                    to: user.PhoneNumber,
                    channel: channel,
                    pathServiceSid: _twilioOptions.Value.ServiceSid
                );

                return new VerificationResult(verificationResource.Sid);
            }
            catch (TwilioException e)
            {
                return new VerificationResult(new List<string> { e.Message });
            }
        }

        public async Task<VerificationResult> CheckVerificationAsync(User user, string code)
        {
            try
            {
                var challenge = ChallengeResource.Create(
                    authPayload: code,
                    factorSid: "YFXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                    pathServiceSid: _twilioOptions.Value.ServiceSid,
                    pathIdentity: user.MfaIdentity
                );

                return challenge.Status.Equals("approved") ?
                    new VerificationResult(challenge.Sid) :
                    new VerificationResult(new List<string> { "Wrong code. Try again." });
            }
            catch (Exception e)
            {
                return new VerificationResult(new List<string> { e.Message });
            }
        }
    }

    public interface IVerification
    {
        Task<VerificationResult> StartVerificationAsync(User user, string channel);

        Task<VerificationResult> CheckVerificationAsync(User user, string code);
    }

    public class VerificationResult
    {
        public VerificationResult(string sid)
        {
            Sid = sid;
            IsValid = true;
        }

        public VerificationResult(List<string> errors)
        {
            Errors = errors;
            IsValid = false;
        }

        public bool IsValid { get; set; }

        public string Sid { get; set; }

        public List<string> Errors { get; set; }
    }
}