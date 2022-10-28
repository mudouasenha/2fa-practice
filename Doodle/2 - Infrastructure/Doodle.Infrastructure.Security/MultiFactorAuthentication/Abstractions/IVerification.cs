using Doodle.Domain.Entities;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Models;

namespace Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions
{
    public interface IVerification
    {
        Task<(VerificationResult, string)> StartVerificationAsync(ApplicationUser user);

        Task<VerificationResult> CheckVerificationAsync(ApplicationUser user, string code);

        Task<VerificationResult> VerifyResource(ApplicationUser user, string payload);
    }
}