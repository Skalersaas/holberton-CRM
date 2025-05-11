using System.ComponentModel.DataAnnotations;

namespace Domain.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string password || string.IsNullOrWhiteSpace(password))
                return new ValidationResult("Password is required.");

            var rules = new (Func<string, bool> Check, string Error)[]
            {
                (p => p.Length >= 5, "Password must be at least 5 characters."),
                (p => p.Any(char.IsUpper), "Password must contain at least one uppercase letter."),
                (p => p.Any(char.IsDigit), "Password must contain at least one digit."),
                (p => p.Any(c => !char.IsLetterOrDigit(c)), "Password must contain at least one symbol.")
            };

            foreach (var (check, error) in rules)
            {
                if (!check(password))
                    return new ValidationResult(error);
            }

            return ValidationResult.Success;
        }
    }
}
