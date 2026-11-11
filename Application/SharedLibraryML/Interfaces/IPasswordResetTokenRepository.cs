using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        void Create(PasswordResetToken token);
        PasswordResetToken? GetValidToken(string tokenHash);
        void MarkAsUsed(string tokenHash);
        void UpdateUserPassword(int userId, string hashedPassword);
    }
}
