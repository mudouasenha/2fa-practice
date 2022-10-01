using Doodle.Domain.Enums;

namespace Doodle.Services.Security.Models
{
    public class DataIntegrityInputDTO
    {
        public string InputData { get; set; }

        public string Key { get; set; }

        public HashAlgorithmOptionsEnum HashAlgorithmOption { get; set; }

        public DataEncryptionStrategiesEnum DataEncryptionStrategy { get; set; }
    }
}