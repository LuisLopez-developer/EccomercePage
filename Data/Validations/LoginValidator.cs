using EccomercePage.Data.DTO;
using FluentValidation;

namespace EccomercePage.Data.Validations
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Correo electrónico es requerido.")
                .EmailAddress().WithMessage("Correo electrónico no es válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña es requerida.");
        }
    }
}
