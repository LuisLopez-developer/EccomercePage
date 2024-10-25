using EccomercePage.Data.DTO;
using FluentValidation;

namespace EccomercePage.Data.Validations
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Nombre de usuario es requerido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Correo electrónico es requerido.")
                .EmailAddress().WithMessage("Correo electrónico no es válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña es requerida.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$")
                .WithMessage("La contraseña debe tener al menos una mayúscula, un número y un carácter especial.");
        }
    }
}
