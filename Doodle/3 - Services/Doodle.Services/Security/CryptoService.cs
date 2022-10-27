using Doodle.Domain.Entities;
using Doodle.Domain.Enums;
using Doodle.Domain.Extensions;
using Doodle.Infrastructure.Repository.Auth.Repositories.Abstractions;
using Doodle.Infrastructure.Security.Cryptography;
using Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric;
using Doodle.Infrastructure.Security.Cryptography.Integrity;
using Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Security.Models;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Security
{
    public class CryptoService : ICryptoService
    {
        private IUserRepository _userRepository;
        //private IUsersService _usersService;

        // BUSCA DE USUÁRIOS DEVE SER EM FORÇA BRUTA. O IV NÃO PODE SER GUARDADO E PRECISA SER DERIVADO DO SALT,
        // PORTANTO NÃO SE SABE O SALT/REGISTRO CORRETO. DEVO DERIVAR O IV E CHAVE PRA ***BUSCAR O USUÁRIO***, USANDO CADA SALT ARMAZENADO NA BASE
        public CryptoService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            //_usersService = usersService;
        }

        public (byte[] derivedPassword, byte[] salt) GenerateDerivedKey(string password)
        {
            var salt = SaltGenerator.GenerateSalt();
            var derivedKey = PKBDF2KeyDerivation.DeriveKey(password, salt);

            return (derivedKey, salt);
        }

        public bool MatchDerivedKey(string password, byte[] salt, byte[] derivedKeyToMatch)
        {
            var derivedKeyCheck = PKBDF2KeyDerivation.HasMatchedDerivedKey(password, salt, derivedKeyToMatch.AsHexadecimalString());

            return derivedKeyCheck;
        }

        public bool MatchHash(HashAlgorithmOptionsEnum hashOption, string data, byte[] password, string hashToVerify)
        {
            bool hashedCheck;
            if (hashOption == HashAlgorithmOptionsEnum.HMACSHA512)
                hashedCheck = HMACSHA512Algorithm.VerifyHash(password, data, hashToVerify);
            else hashedCheck = HMACSHA256Algorithm.VerifyHash(password, data, hashToVerify);

            return hashedCheck;
        }

        public DataIntegritySummaryResultDTO GenerateExecutionSummary(DataIntegrityInputDTO input)
        {
            var (derivedKey, salt) = GenerateDerivedKey(input.Key);

            var derivedKeyCheck = MatchDerivedKey(input.Key, salt, derivedKey);

            var hash = GenerateKeyedHashFromData(input.HashAlgorithmOption, input.InputData, derivedKey);

            var hashCheck = MatchHash(input.HashAlgorithmOption, input.InputData, derivedKey, hash);

            return new DataIntegritySummaryResultDTO()
            {
                InputData = input.InputData,
                EncodedAndDecodedInputData = input.InputData.ToByteArray().AsString(),
                Key = input.Key,
                Salt = salt.AsHexadecimalString(),
                EncodedAndDecodedSalt = salt.AsHexadecimalString().FromHexStringToByteArray().AsHexadecimalString(),
                DerivedKey = derivedKey.AsHexadecimalString(),
                EncodedAndDecodedDerivedKey = derivedKey.AsHexadecimalString().FromHexStringToByteArray().AsHexadecimalString(),
                DerivedKeyCheckResult = derivedKeyCheck,
                HashedResult = hash,
                EncodedAndDecodedHashedResult = hash.ToByteArray().AsString(),
                HashCheckResult = hashCheck,
                DataEncryptionStrategy = input.DataEncryptionStrategy,
                HashAlgorithmOption = input.HashAlgorithmOption
            };
        }

        public string GenerateKeyedHashFromData(HashAlgorithmOptionsEnum hashOption, string data, byte[] derivedKey)
        {
            byte[] hashedData;

            if (hashOption == HashAlgorithmOptionsEnum.HMACSHA512) hashedData = HMACSHA512Algorithm.HashData(derivedKey, data);
            else hashedData = HMACSHA256Algorithm.HashData(derivedKey, data);

            return hashedData.AsHexadecimalString();
        }

        public async Task<byte[]> GetUserCredentials(UserFilterDTO userFilter)
        {
            var user = await _userRepository.GetByUsernameAndPassword(userFilter.UserName, userFilter.Password);

            return new byte[1];
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

        public User VerifyLogin(List<User> users, string username, string password)
        {
            foreach (var user in users)
            {
                var salt = user.Salt.FromHexStringToByteArray();

                var iv = PKBDF2KeyDerivation.DeriveKey(password, salt, true);
                var derivedPassword = PKBDF2KeyDerivation.DeriveKey(password, salt);

                var encryptedPassword = AesGcmSymmetricEncryption.Encrypt(password, derivedPassword, iv);
                var hasEqualUsername = ScryptKDF.VerifyEncrypt(username, salt, user.Username);

                var hasEqualPassword = string.Equals(encryptedPassword, user.Password);
                if (hasEqualUsername & hasEqualPassword)
                    return user;
            }

            return default;
        }
    }
}