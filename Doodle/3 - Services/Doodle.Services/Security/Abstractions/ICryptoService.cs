using Doodle.Domain.Entities;
using Doodle.Services.Security.Models;

namespace Doodle.Services.Security.Abstractions
{
    public interface ICryptoService
    {
        UserEncryptedData GenerateUserEncryptionData(string username, string password);

        DataIntegritySummaryResultDTO GenerateExecutionSummary(DataIntegrityInputDTO input);

        User VerifyLogin(List<User> users, string username, string password);
    }
}