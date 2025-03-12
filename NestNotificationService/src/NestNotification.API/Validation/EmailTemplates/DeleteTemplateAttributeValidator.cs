namespace NestNotification.API.Validation.EmailTemplates;

public class DeleteTemplateAttributeValidator : AbstractValidator<DeleteTemplateAttributeDto>
{
    public DeleteTemplateAttributeValidator()
    {
        RuleFor(x => x.AttributeId)
            .NotEmpty().WithMessage("Attribute ID cannot be empty.")
            .NotNull().WithMessage("Attribute ID cannot be null.")
            .MinimumLength(1).WithMessage("Attribute ID must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Attribute ID must be at most 128 characters long.");

        RuleFor(x => x.DeletedBy)
            .NotEmpty().WithMessage("Deleted by cannot be empty.")
            .NotNull().WithMessage("Deleted by cannot be null.")
            .MinimumLength(1).WithMessage("Deleted by must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Deleted by must be at most 128 characters long.");
    }
}