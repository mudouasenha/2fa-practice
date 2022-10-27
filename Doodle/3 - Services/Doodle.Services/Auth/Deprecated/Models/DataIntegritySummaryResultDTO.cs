using Doodle.Domain.Enums;

namespace Doodle.Services.Security.Models
{
    public class DataIntegritySummaryResultDTO
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
    }
}