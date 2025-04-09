using System;
using System.Linq;

namespace Utilities.Services
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password, out string error)
        {
            error = string.Empty;

            var rules = new (Func<string, bool> condition, string errorMessage)[]
            {
                (p => p.Length >= 5, "Password must be at least 5 characters."),
                (p => p.Any(char.IsUpper), "Password must contain at least one uppercase letter."),
                (p => p.Any(char.IsDigit), "Password must contain at least one digit."),
                (p => p.Any(c => !char.IsLetterOrDigit(c)), "Password must contain at least one symbol.")
            };

            foreach (var rule in rules)
            {
                if (!rule.condition(password))
                {
                    error = rule.errorMessage;
                    return false;
                }
            }

            return true;
        }
    }
}
