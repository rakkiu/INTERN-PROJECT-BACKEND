using System;
using System.Text;

namespace Application.Utilities
{
    /// <summary>
    /// Utility for generating secure random passwords.
    /// </summary>
    public static class PasswordGenerator
    {
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumericChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*";
        
        private static readonly Random _random = new Random();

        /// <summary>
        /// Generates a secure random password.
        /// Format: At least 12 characters with uppercase, lowercase, numbers, and special characters.
        /// </summary>
        /// <returns>Generated password</returns>
        public static string GenerateSecurePassword()
        {
            const int passwordLength = 12;
            var password = new StringBuilder();

            // Ensure at least one character from each category
            password.Append(UppercaseChars[_random.Next(UppercaseChars.Length)]);
            password.Append(LowercaseChars[_random.Next(LowercaseChars.Length)]);
            password.Append(NumericChars[_random.Next(NumericChars.Length)]);
            password.Append(SpecialChars[_random.Next(SpecialChars.Length)]);

            // Fill the rest with random characters
            var allChars = UppercaseChars + LowercaseChars + NumericChars + SpecialChars;
            for (int i = password.Length; i < passwordLength; i++)
            {
                password.Append(allChars[_random.Next(allChars.Length)]);
            }

            // Shuffle the password
            var passwordArray = password.ToString().ToCharArray();
            for (int i = passwordArray.Length - 1; i > 0; i--)
            {
                int randomIndex = _random.Next(i + 1);
                (passwordArray[i], passwordArray[randomIndex]) = (passwordArray[randomIndex], passwordArray[i]);
            }

            return new string(passwordArray);
        }
    }
}
