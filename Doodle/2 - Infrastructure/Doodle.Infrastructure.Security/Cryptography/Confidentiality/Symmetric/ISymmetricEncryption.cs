namespace Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric
{
    public interface ISymmetricEncryption
    {
        byte[] Encrypt(string plainText, byte[] Key, byte[] IV);

        string Decrypt(byte[] cipherText, byte[] Key, byte[] IV);
    }
}