using Doodle.Domain.Entities;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Models;

namespace Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions
{
    public interface IVerification
    {
        Task<(VerificationResult, string)> StartVerificationAsync(User user);

        Task<VerificationResult> CheckVerificationAsync(User user, string code);

        Task<VerificationResult> VerifyResource(User user, string payload);
    }
}