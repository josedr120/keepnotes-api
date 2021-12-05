using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using keepnotes_api.Interfaces;

namespace keepnotes_api.Helpers
{
    public class Crypto : ICrypto
    {
        public string Encrypt(string plainText)  
        {  
            var iv = new byte[16];
            byte[] array; 
  
            using (var aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes("2D4A614E635266556A586E3272357538");  
                aes.IV = iv;  
  
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
                using (var memoryStream = new MemoryStream())  
                {  
                    using (var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (var streamWriter = new StreamWriter((Stream)cryptoStream))  
                        {  
                            streamWriter.Write(plainText);  
                        }  
  
                        array = memoryStream.ToArray();  
                    }  
                }  
            }  
  
            return Convert.ToBase64String(array);  
        }

        public string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("2D4A614E635266556A586E3272357538");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}