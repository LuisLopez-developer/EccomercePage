using EccomercePage.Data.DTO.Profile;
using FluentValidation;

namespace EccomercePage.Data.Validations
{
    public class AddPeopleValidator : AbstractValidator<AddPeopleDTO>
    {
        public AddPeopleValidator()
        {
            // Validar DNI, que tenga solo numeros y que tenga 8 caracteres
            RuleFor(x => x.DNI)
                .NotEmpty().WithMessage("DNI es requerido.")
                .Matches("^[0-9]{8}$").WithMessage("DNI debe tener 8 caracteres y solo numeros.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nombre es requerido.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Apellido es requerido.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Dirección es requerida.");
        }
    }
}
