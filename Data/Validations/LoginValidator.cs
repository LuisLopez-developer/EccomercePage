using EccomercePage.Data.DTO;
using FluentValidation;

namespace EccomercePage.Data.Validations
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            //Va ldar si es un email o un nombre de usuario
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Nombre de usuario o email es requerido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña es requerida.");
        }
    }
}
