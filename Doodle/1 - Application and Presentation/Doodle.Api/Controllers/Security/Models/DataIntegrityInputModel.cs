using Doodle.Domain.Enums;
using Doodle.Services.Security.Models;

namespace Doodle.Api.Controllers.Security.Models
{
    public class DataIntegrityInputModel
    {
        public string InputData { get; set; }

        public string Key { get; set; }

        public HashAlgorithmOptionsEnum HashAlgorithmOption { get; set; }

        public DataEncryptionStrategiesEnum DataEncryptionStrategy { get; set; }

        public static DataIntegrityInputDTO ToDto(DataIntegrityInputModel input) => new()
        {
            InputData = input.InputData,
            Key = input.Key,
            HashAlgorithmOption = input.HashAlgorithmOption,
            DataEncryptionStrategy = input.DataEncryptionStrategy
        };
    }
}