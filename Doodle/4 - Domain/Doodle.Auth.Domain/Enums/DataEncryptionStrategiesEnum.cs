namespace Doodle.Auth.Domain.Enums
{
    public enum DataEncryptionStrategiesEnum
    {
        Encrypt,
        Hash,
        MAC,
        MACThenEncrypt,
        EncryptThenHashMAC,
        EncryptAndMAC
    }
}
