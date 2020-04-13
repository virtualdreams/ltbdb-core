using FluentValidation;
using ltbdb.Models;

namespace ltbdb.Validators
{
	public class LoginModelValidator : AbstractValidator<LoginModel>
	{
		public LoginModelValidator()
		{
			RuleFor(r => r.Username)
				.NotEmpty()
				.WithMessage("Bitte gib einen Benutzernamen ein.");

			RuleFor(r => r.Password)
				.NotEmpty()
				.WithMessage("Bitte gib ein Passwort ein.");
		}
	}
}