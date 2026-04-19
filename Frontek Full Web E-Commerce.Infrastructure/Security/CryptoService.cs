
using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptoService : ICryptoService
{
    private readonly byte[] Key = Encoding.UTF8.GetBytes("Zx9Kp2Lm8Qw7Er5Ty6Ui1Op3As4Df8Gh");
    private readonly byte[] IV = Encoding.UTF8.GetBytes("A1b2C3d4E5f6G7h8");

    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string Decrypt(string cipheredText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] cipheredBytes = Convert.FromBase64String(cipheredText);

            using (MemoryStream msDecrypt = new MemoryStream(cipheredBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}