using Doodle.Domain.Enums;
using Doodle.Domain.Extensions;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Doodle.Infrastructure.Security.Cryptography;
using Doodle.Infrastructure.Security.Cryptography.Integrity;
using Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Security.Models;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Security
{
    public class CryptoService : ICryptoService
    {
        private IUserRepository _userRepository;
        private IUsersService _usersService;

        public CryptoService(IUserRepository userRepository, IUsersService usersService)
        {
            _userRepository = userRepository;
            _usersService = usersService;
        }

        public (byte[] derivedPassword, byte[] salt) GenerateDerivedKey(string password)
        {
            var salt = SaltGenerator.GenerateSalt();
            var derivedKey = PKBDF2KeyDerivation.DeriveKey(password, salt);

            return (derivedKey, salt);
        }

        public bool MatchDerivedKey(string password, byte[] salt, string derivedKeyToMatch)
        {
            var derivedKeyCheck = PKBDF2KeyDerivation.HasMatchedDerivedKey(password, salt, derivedKeyToMatch);

            return derivedKeyCheck;
        }

        public bool MatchHash(HashAlgorithmOptionsEnum hashOption, string data, string password)
        {
            bool hashedCheck;
            if (hashOption == HashAlgorithmOptionsEnum.HMACSHA512)
                hashedCheck = HMACSHA512Algorithm.VerifyHash(password.ToByteArray(), data.ToByteArray());
            else hashedCheck = HMACSHA256Algorithm.VerifyHash(password.ToByteArray(), data.ToByteArray());

            return hashedCheck;
        }

        public DataIntegritySummaryResultDTO GenerateExecutionSummary(DataIntegrityInputDTO input)
        {
            var (derivedKey, salt) = GenerateDerivedKey(input.Key);

            var derivedKeyCheck = MatchDerivedKey(input.Key, salt, derivedKey.AsString());

            var hash = GenerateKeyedHashFromData(input.HashAlgorithmOption, input.InputData, input.Key);

            var hashCheck = MatchHash(input.HashAlgorithmOption, input.InputData, derivedKey.AsString());

            return new DataIntegritySummaryResultDTO()
            {
                Key = input.Key,
                Salt = salt.AsString(),
                DerivedKey = derivedKey.AsString(),
                DerivedKeyCheckResult = derivedKeyCheck,
                HashedResult = hash,
                HashCheckResult = hashCheck,
                DataEncryptionStrategy = input.DataEncryptionStrategy,
                HashAlgorithmOption = input.HashAlgorithmOption
            };
        }

        public string GenerateKeyedHashFromData(HashAlgorithmOptionsEnum hashOption, string data, string password)
        {
            var (derivedKey, _) = GenerateDerivedKey(password);

            string hashedData;
            if (hashOption == HashAlgorithmOptionsEnum.HMACSHA512)
                hashedData = HMACSHA512Algorithm.HashData(derivedKey, data.ToByteArray());
            else hashedData = HMACSHA256Algorithm.HashData(derivedKey, data.ToByteArray());

            return hashedData;
        }

        public async Task<byte[]> GetUserCredentials(UserFilterDTO userFilter)
        {
            var user = await _userRepository.GetByUsernameAndPassword(userFilter.UserName, userFilter.Password);

            return new byte[1];
        }
    }
}