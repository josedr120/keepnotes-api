namespace keepnotes_api.Interfaces
{
    public interface ICrypto
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }
}