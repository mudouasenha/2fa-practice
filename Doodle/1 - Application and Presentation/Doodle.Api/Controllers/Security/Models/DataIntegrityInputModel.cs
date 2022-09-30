using Doodle.Domain.Enums;

namespace Doodle.Api.Controllers.Security.Models
{
    public class DataIntegrityInputModel
    {
        public string InputData { get; set; }

        public string Key { get; set; }

        public HashAlgorithmOptionsEnum HashAlgorithmOption { get; set; }

        public DataEncryptionStrategiesEnum DataEncryptionStrategy { get; set; }
    }
}