using FluentValidation;
using HolbertonCRM.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Validations
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("It cannot be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("It cannot be empty").EmailAddress().WithMessage("Please enter your email address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("It cannot be empty").EmailAddress().WithMessage("Please enter your email address");
        }
    }
}
