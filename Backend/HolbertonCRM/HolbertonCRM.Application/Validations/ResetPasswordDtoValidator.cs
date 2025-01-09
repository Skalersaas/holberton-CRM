using FluentValidation;
using HolbertonCRM.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Validations
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(l => l).Custom((l, context) =>
            {
                if (l.Password != l.ConfirmPassword)
                {
                    context.AddFailure("RePassword", "It must be the same");
                }

            });

        }
    }
}
