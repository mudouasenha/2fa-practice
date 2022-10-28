using Doodle.Domain.Entities;
using Doodle.Domain.Extensions;
using Doodle.Infrastructure.Security.Cryptography;
using Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric;
using Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Security.Models;

namespace Doodle.Services.Auth.Deprecated
{
    public class CryptoService : ICryptoService
    {
        public CryptoService()
        {
        }

        public UserEncryptedData GenerateUserEncryptionData(string username, string password)
        {
            var salt = SaltGenerator.GenerateSalt();
            var iv = PKBDF2KeyDerivation.DeriveKey(password, salt, true);
            var derivedPassword = PKBDF2KeyDerivation.DeriveKey(password, salt);

            var encryptedPassword = AesGcmSymmetricEncryption.Encrypt(password, derivedPassword, iv);
            var encryptedUsername = ScryptKDF.Encrypt(username, salt);

            return new UserEncryptedData
            {
                EncryptedUsername = encryptedUsername,
                EncryptedPassword = encryptedPassword,
                Salt = salt.AsHexadecimalString()
            };
        }

        public ApplicationUser VerifyLogin(List<ApplicationUser> users, string username, string password)
        {
            foreach (var user in users)
            {
                var salt = user.Salt.FromHexStringToByteArray();

                var iv = PKBDF2KeyDerivation.DeriveKey(password, salt, true);
                var derivedPassword = PKBDF2KeyDerivation.DeriveKey(password, salt);

                var encryptedPassword = AesGcmSymmetricEncryption.Encrypt(password, derivedPassword, iv);
                var hasEqualUsername = ScryptKDF.VerifyEncrypt(username, salt, user.UserName);

                var hasEqualPassword = string.Equals(encryptedPassword, user.Password);
                if (hasEqualUsername & hasEqualPassword)
                    return user;
            }

            return default;
        }
    }
}