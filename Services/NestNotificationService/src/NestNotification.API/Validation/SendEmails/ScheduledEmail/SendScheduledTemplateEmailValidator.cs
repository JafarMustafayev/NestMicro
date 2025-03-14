namespace NestNotification.API.Validation.SendEmails.ScheduledEmail;

public class SendScheduledTemplateEmailValidator : AbstractValidator<SendScheduledTemplateEmailDto>
{
    public SendScheduledTemplateEmailValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty().WithMessage("The 'to' field cannot be empty.")
            .NotNull().WithMessage("The 'to' field cannot be null.")
            .EmailAddress().WithMessage("The 'to' field must be a valid email address.");

        RuleFor(x => x.TemplateId)
            .NotEmpty().WithMessage("The 'templateId' field cannot be empty.")
            .NotNull().WithMessage("The 'templateId' field cannot be null.")
            .MinimumLength(1).WithMessage("The 'templateId' field must be at least 2 character long.")
            .MaximumLength(128).WithMessage("The 'templateId' field must be at most 128 characters long.");

        RuleFor(x => x.Placeholders)
            .NotEmpty().WithMessage("The 'placeholders' field cannot be empty.")
            .NotNull().WithMessage("The 'placeholders' field cannot be null.");

        RuleFor(x => x.ScheduledAt)
            .NotNull().WithMessage("The 'scheduledAt' field cannot be null.");
    }
}