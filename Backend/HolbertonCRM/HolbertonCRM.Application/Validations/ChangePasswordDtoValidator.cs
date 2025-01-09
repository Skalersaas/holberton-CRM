using FluentValidation;
using HolbertonCRM.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Validations
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(l => l).Custom((l, context) =>
            {
                if (l.CurrentPassword == l.NewPassword)
                {
                    context.AddFailure("RePassword", "It cannot be the same with current password");
                }

            });

        }
    }
}
