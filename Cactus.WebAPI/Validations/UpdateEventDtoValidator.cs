using Cactus.WebAPI.Modals.Event;
using FluentValidation;

namespace Cactus.WebAPI.Validations
{
    public class UpdateEventDtoValidator : AbstractValidator<UpdateEventDto>
    {
        public UpdateEventDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Event ID is required.");
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Event name is required.")
           .MaximumLength(100).WithMessage("Event name must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Event description must not exceed 500 characters.");
            //RuleFor(x => x.StartDate)
            //    .NotEmpty().WithMessage("Event start date is required.")
            //    .GreaterThan(DateTime.Now).WithMessage("Event start date must be in the future.");
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Event location is required.")
                .MaximumLength(200).WithMessage("Event location must not exceed 200 characters.");

        }
    }
}
