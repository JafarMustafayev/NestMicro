namespace NestNotification.API.Validation.EmailTemplates;

public class DeleteEmailTemplateValidator : AbstractValidator<DeleteEmailTemplateDto>
{
    public DeleteEmailTemplateValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithMessage("Template ID cannot be empty.")
            .NotNull().WithMessage("Template ID cannot be null.")
            .MinimumLength(1).WithMessage("Template ID must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Template ID must be at most 128 characters long.");

        RuleFor(x => x.DeletedBy)
            .NotEmpty().WithMessage("Deleted by cannot be empty.")
            .NotNull().WithMessage("Deleted by cannot be null.")
            .MinimumLength(1).WithMessage("Deleted by must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Deleted by must be at most 128 characters long.");
    }
}