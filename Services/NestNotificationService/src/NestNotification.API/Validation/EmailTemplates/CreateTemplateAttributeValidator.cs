namespace NestNotification.API.Validation.EmailTemplates;

public class CreateTemplateAttributeValidator : AbstractValidator<CreateTemplateAttributeDto>
{
    public CreateTemplateAttributeValidator()
    {
        RuleFor(x => x.AttributeName)
            .NotEmpty().WithMessage("Attribute name cannot be empty.")
            .NotNull().WithMessage("Attribute name cannot be null.")
            .MinimumLength(1).WithMessage("Attribute name must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Attribute name must be at most 128 characters long.");

        RuleFor(x => x.AttributeValue)
            .NotEmpty().WithMessage("Attribute value cannot be empty.")
            .NotNull().WithMessage("Attribute value cannot be null.")
            .MinimumLength(1).WithMessage("Attribute value must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Attribute value must be at most 128 characters long.");

        RuleFor(x => x.IsRequired)
            .NotNull().WithMessage("Is required cannot be null.");

        RuleFor(x => x.Description)
            .MaximumLength(10000).WithMessage("Description must be at most 10000 characters long.");
    }
}