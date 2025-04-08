using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password, out string error)
        {
            error = string.Empty;

            if (password.Length < 5)
            {
                error = "Password must be at least 5 characters.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                error = "Password must contain at least one uppercase letter.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                error = "Password must contain at least one digit.";
                return false;
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                error = "Password must contain at least one symbol.";
                return false;
            }

            return true;
        }
    }
}
