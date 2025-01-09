using FluentValidation;
using HolbertonCRM.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Validations
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("It cannot be empty");

        }
    }
}
