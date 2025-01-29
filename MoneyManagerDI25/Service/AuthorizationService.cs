using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagerX.Service
{
    public static class AuthorizationService
    {

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            string inputPasswordHash = HashPassword(inputPassword);
            return inputPasswordHash == storedPasswordHash;
        }
        public static string GeneratePassword()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"№;%:?*";
            Random random = new Random();

            int count = random.Next(12, 17);
            StringBuilder pass = new StringBuilder();
            for (int i = 0; i <= count; i++)
            {
                pass.Append(chars[random.Next(chars.Length)]);
            }
            return pass.ToString();
        }
    }
}
