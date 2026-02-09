using Application.Interfaces;

namespace Infrastructure.Security
{
    /// <summary>
    /// Encryption service implementation using EncryptionHelper
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        public string EncryptDeterministic(string plainText)
        {
            return EncryptionHelper.EncryptDeterministic(plainText);
        }

        public string DecryptDeterministic(string cipherText)
        {
            return EncryptionHelper.DecryptDeterministic(cipherText);
        }

        public string Encrypt(string plainText)
        {
            return EncryptionHelper.Encrypt(plainText);
        }

        public string Decrypt(string cipherText)
        {
            return EncryptionHelper.Decrypt(cipherText);
        }
    }
}
