using System;
using asp_net_with_angular_client_template.Models.Dto;
using FluentValidation;

namespace asp_net_with_angular_client_template.Validators
{
	public class LoginValidator: AbstractValidator<LoginDto>
	{
		public LoginValidator()
		{
			RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").
			   EmailAddress().WithMessage("Email is not valid.");

			RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
		}
    }
}

