using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontek_Full_Web_E_Commerce.Models.Utils
{
    public static class SecretService
    {
        // Hashear contraseña
        public static string HashSecret(string Secret)
        {
            return BCrypt.Net.BCrypt.HashPassword(Secret);
        }

        // Verificar contraseña
        public static bool VerifySecret(string Secret, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(Secret, storedHash);
        }
    }
}