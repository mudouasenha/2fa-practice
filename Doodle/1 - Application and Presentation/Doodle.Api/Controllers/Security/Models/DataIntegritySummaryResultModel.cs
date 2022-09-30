using Doodle.Domain.Enums;

namespace Doodle.Api.Controllers.Security.Models
{
    public class DataIntegritySummaryResultModel
    {
        public string InputData { get; set; }

        public string Key { get; set; }

        public string HashedResult { get; set; }

        public string HashCheckResult { get; set; }

        public HashAlgorithmOptionsEnum HashAlgorithmOption { get; set; }

        public DataEncryptionStrategiesEnum DataEncryptionStrategy { get; set; }
    }
}