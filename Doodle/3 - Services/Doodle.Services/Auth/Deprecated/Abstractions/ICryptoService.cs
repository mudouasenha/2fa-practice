using Doodle.Domain.Entities;
using Doodle.Services.Security.Models;

namespace Doodle.Services.Security.Abstractions
{
    public interface ICryptoService
    {
        UserEncryptedData GenerateUserEncryptionData(string username, string password);

        ApplicationUser VerifyLogin(List<ApplicationUser> users, string username, string password);
    }
}