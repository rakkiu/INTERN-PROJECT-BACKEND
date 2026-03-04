using Application.Interfaces;
using BCrypt.Net;

namespace Infrastructure.Security
{
    /// <summary>
    /// Password hashing service implementation using BCrypt.
    /// BCrypt is more secure than SHA256 for password hashing.
    /// </summary>
    public class PasswordHashService : IPasswordHashService
    {
        /// <summary>
        /// Hashes password using BCrypt with work factor 12.
        /// </summary>
        public string HashPassword(string password)
        {
            // BCrypt with work factor 12 (default is 10, but 12 is more secure)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifies password against BCrypt hash.
        /// </summary>
        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Password verification error: {ex.Message}");
                return false;
            }
        }
    }
}

