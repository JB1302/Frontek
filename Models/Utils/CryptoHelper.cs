using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Frontek_Full_Web_E_Commerce.Models.Utils
{
    public static class CryptoHelper
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("DFFHnXx8lPpXIwgdbwU50RSWme7GaaBw");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("iboS0QQL1bPwyYhM");

        public static string Encriptar(string Valor)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(Valor);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string ValorCifrado) 
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                byte[] BytesCifrados = Convert.FromBase64String(ValorCifrado);

                using (var ms = new MemoryStream(BytesCifrados))
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}