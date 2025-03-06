namespace NestNotification.API.Validation.EmailTemplates;

public class UpdateEmailTemplateValidator : AbstractValidator<UpdateEmailTemplateDto>
{
    public UpdateEmailTemplateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Email template id cannot be empty.")
            .NotNull().WithMessage("Email template id cannot be null.")
            .MinimumLength(1).WithMessage("Email template id must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Email template id must be at most 128 characters long.");

        RuleFor(x => x.TemplateName)
            .NotEmpty().WithMessage("Email template cannot be empty.")
            .NotNull().WithMessage("Email template cannot be null.")
            .MinimumLength(1).WithMessage("Email template must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Email template must be at most 128 characters long.");
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Email subject cannot be empty.")
            .NotNull().WithMessage("Email subject cannot be null.")
            .MinimumLength(1).WithMessage("Email subject must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Email subject must be at most 128 characters long.");
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Email body cannot be empty.")
            .NotNull().WithMessage("Email body cannot be null.");
        RuleFor(x => x.LastModifiedBy)
            .NotEmpty().WithMessage("Last modified by cannot be empty.")
            .NotNull().WithMessage("Last modified by cannot be null.")
            .MinimumLength(1).WithMessage("Last modified by must be at least 2 character long.")
            .MaximumLength(128).WithMessage("Last modified by must be at most 128 characters long.");
        RuleFor(x => x.IsHtml)
            .NotNull().WithMessage("IsHtml cannot be null.");
    }
}