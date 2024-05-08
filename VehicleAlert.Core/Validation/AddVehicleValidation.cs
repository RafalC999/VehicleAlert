using FluentValidation;
using FluentValidation.Results;

namespace VehicleAlert.Core.Validation
{
    public class AddVehicleValidation : AbstractValidator<VehiclePagesView>
    {
        private ValidationResult validationResult { get; set; }
        public AddVehicleValidation()
        {
            RuleFor(v => v.NewVehicleName).NotEmpty()
                .WithMessage("Wpisz nazwę!").WithErrorCode("Name");
            RuleFor(v => v.NewVehicleName).MinimumLength(2)
                .WithMessage("Nazwa jest zbyt krótka!").WithErrorCode("Name");
            RuleFor(v => v.NewVehicleName).MaximumLength(50)
                .WithMessage("Nazwa jest zbyt długa!").WithErrorCode("Name");


            RuleFor(v => v.NewVehicleCourse).NotEmpty()
                .WithMessage("Wpisz przebieg.").WithErrorCode("Course");
            RuleFor(v => v.NewVehicleCourse).GreaterThanOrEqualTo(0)
                .WithMessage("Przebieg nie może być ujemny!").WithErrorCode("Course");


            RuleFor(v => v.NewVehiclePlate).NotEmpty()
                .WithMessage("Uzupełnij numer rejestracyjny.").WithErrorCode("Plate");
        }

        public void VehicleValidationErrors(VehiclePagesView vehiclePagesView)
        {
            validationResult = Validate(vehiclePagesView);

            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
            {
                vehiclePagesView.ErrorName = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
            }
            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "Course").Count() != 0)
            {
                vehiclePagesView.ErrorCourse = validationResult.Errors.Where(p => p.ErrorCode == "Course").Single().ErrorMessage;
            }
            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "Plate").Count() != 0)
            {
                vehiclePagesView.ErrorPlate = validationResult.Errors.Where(p => p.ErrorCode == "Plate").Single().ErrorMessage;
            }
            vehiclePagesView.RefreshErrors();
        }

    }
}
