using Doodle.Domain.Enums;
using Doodle.Services.Security.Models;

namespace Doodle.Api.Controllers.Security.Models
{
    public class DataIntegritySummaryResultModel
    {
        public string InputData { get; set; }

        public string EncodedAndDecodedInputData { get; set; }

        public string Key { get; set; }

        public string DerivedKey { get; set; }

        public string EncodedAndDecodedDerivedKey { get; set; }

        public bool DerivedKeyCheckResult { get; set; }

        public string Salt { get; set; }

        public string EncodedAndDecodedSalt { get; set; }

        public string HashedResult { get; set; }

        public string EncodedAndDecodedHashedResult { get; set; }

        public bool HashCheckResult { get; set; }

        public HashAlgorithmOptionsEnum HashAlgorithmOption { get; set; }

        public DataEncryptionStrategiesEnum DataEncryptionStrategy { get; set; }

        public static DataIntegritySummaryResultModel FromDto(DataIntegritySummaryResultDTO result) => new()
        {
            InputData = result.InputData,
            EncodedAndDecodedInputData = result.EncodedAndDecodedInputData,
            Key = result.Key,
            DerivedKey = result.DerivedKey,
            DerivedKeyCheckResult = result.DerivedKeyCheckResult,
            EncodedAndDecodedDerivedKey = result.EncodedAndDecodedDerivedKey,
            Salt = result.Salt,
            EncodedAndDecodedSalt = result.EncodedAndDecodedSalt,
            HashedResult = result.HashedResult,
            EncodedAndDecodedHashedResult = result.EncodedAndDecodedHashedResult,
            HashCheckResult = result.HashCheckResult,
            HashAlgorithmOption = result.HashAlgorithmOption,
            DataEncryptionStrategy = result.DataEncryptionStrategy
        };
    }
}