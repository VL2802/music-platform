using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharedLibraryML;

namespace ML_LogicLayer.Validators
{
    public static class UserValidator
    {
        public static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 14 || password.Length > 50)
                throw new ArgumentException("Password must be between 14 and 50 characters.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                throw new ArgumentException("Password must contain at least one digit.");

            if (!Regex.IsMatch(password, @"[\W_]"))
                throw new ArgumentException("Password must contain at least one special character.");
        }
    }
}
