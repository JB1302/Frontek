
using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptoService : ICryptoService
{
    private readonly byte[] Key;
    private readonly byte[] IV;

    public CryptoService()
    {
        Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

        IV = Encoding.UTF8.GetBytes("1234567890123456");
    }

    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
                sw.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
            return string.Empty;

        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                byte[] buffer = Convert.FromBase64String(cipherText);

                using (var ms = new MemoryStream(buffer))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch
        {
            return string.Empty;
        }
    }
}