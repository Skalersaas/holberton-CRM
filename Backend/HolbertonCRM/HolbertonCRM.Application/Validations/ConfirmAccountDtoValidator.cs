using FluentValidation;
using HolbertonCRM.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Validations
{
    public class ConfirmAccountDtoValidator : AbstractValidator<ConfirmAccountDto>
    {
        public ConfirmAccountDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(x => x.OTP).NotEmpty().WithMessage("It cannot be empty");
        }
    }
}
