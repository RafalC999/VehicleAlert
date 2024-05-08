using FluentValidation;
using FluentValidation.Results;


namespace VehicleAlert.Core.Validation
{
    public class AddActionValidation : AbstractValidator<VehiclePagesView>
    {
        private ValidationResult validationResult { get; set; }
        public AddActionValidation()
        {
            RuleFor(v => v.NewDescription).NotEmpty()
                .WithMessage("Wpisz nazwę!").WithErrorCode("Name");
            RuleFor(v => v.NewDescription).MinimumLength(2)
                .WithMessage("Nazwa jest zbyt krótka!").WithErrorCode("Name");
            RuleFor(v => v.NewDescription).MaximumLength(50)
                .WithMessage("Nazwa jest zbyt długa!").WithErrorCode("Name");

            RuleFor(v => v.NewLastActionCourse).NotNull()
                .WithMessage("Wpisz przebieg z ostatniego serwisu!").WithErrorCode("LastActionCourse");
            RuleFor(v => v.NewLastActionCourse)
                .GreaterThan(0)
                .WithMessage("Wartość nie może być ujemna").WithErrorCode("LastActionCourse");
        }

        public void ActionValidationErrors(VehiclePagesView vehiclePagesView)
        {
            validationResult = Validate(vehiclePagesView);

            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
            {
                vehiclePagesView.ErrorDescription = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
            }
            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Count() != 0)
            {
                vehiclePagesView.ErrorLastActionCourse = validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Single().ErrorMessage;
            }
            if (vehiclePagesView.validationResult.Errors.Where(p => p.ErrorCode == "Interval").Count() != 0)
            {
                vehiclePagesView.ErrorInterval = validationResult.Errors.Where(p => p.ErrorCode == "Interval").Single().ErrorMessage;
            }
            vehiclePagesView.RefreshErrors();
        }
    }
}
