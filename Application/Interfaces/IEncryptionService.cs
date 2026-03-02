namespace Application.Interfaces
{
    /// <summary>
    /// Encryption service interface
    /// </summary>
    public interface IEncryptionService
    {
        string EncryptDeterministic(string plainText);
        string DecryptDeterministic(string cipherText);
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
