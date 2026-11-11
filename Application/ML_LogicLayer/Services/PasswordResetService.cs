using SharedLibraryML;
using SharedLibraryML.Interfaces;
using ML_LogicLayer.Security;
using ML_LogicLayer.Validators;

namespace ML_LogicLayer.Services
{
    public class PasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;

        public PasswordResetService(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        // request reset
        public void RequestPasswordReset(string email)
        {
            var user = _userRepository.FindUserByEmail(email);

            // Prevent email enumeration
            if (user == null)
                return;

            string token = PasswordResetTokenHelper.GenerateToken();
            string tokenHash = PasswordResetTokenHelper.HashToken(token);

            var resetToken = new PasswordResetToken
            {
                UserId = user.GetId(),
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };

            _tokenRepository.Create(resetToken);

           //could be email with link functionality
        }

        //  validate token
        public bool ValidateToken(string token, out int userId)
        {
            userId = 0;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            string tokenHash = PasswordResetTokenHelper.HashToken(token);
            var storedToken = _tokenRepository.GetValidToken(tokenHash);

            if (storedToken == null)
                return false;

            userId = storedToken.UserId;
            return true;
        }

        //  reset password
        public void ResetPassword(string token, string newPassword)
        {
            if (!ValidateToken(token, out int userId))
                throw new UnauthorizedAccessException("Invalid or expired reset token.");

            UserValidator.ValidatePasswordStrength(newPassword);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _tokenRepository.UpdateUserPassword(userId, hashedPassword);
            _tokenRepository.MarkAsUsed(
                PasswordResetTokenHelper.HashToken(token)
            );
        }
    }
}
