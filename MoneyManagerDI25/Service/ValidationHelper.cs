using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoneyManagerX.Service
{
    public static class ValidationHelper
    {
        public static bool IsNullOrWhiteSpace(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsMinLength(string input, int minLength)
        {
            if (input == null) return false;
            return input.Length >= minLength;
        }
        public static bool IsMaxLength(string input, int maxLength)
        {
            if (input == null) return false;
            return input.Length <= maxLength;
        }
        public static bool IsValidPattern(string input, string pattern)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(input, pattern);
        }
    }
}
