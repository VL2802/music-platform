using System.Security.Cryptography;
using System.Text;

namespace ML_LogicLayer.Security
{
    public static class PasswordResetTokenHelper
    {
        public static string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}
