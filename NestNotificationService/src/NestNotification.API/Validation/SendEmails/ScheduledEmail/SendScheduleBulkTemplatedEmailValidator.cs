namespace NestNotification.API.Validation.SendEmails.ScheduledEmail;

public class SendScheduleBulkTemplatedEmailValidator : AbstractValidator<SendScheduleBulkTemplatedEmailDto>
{
    public SendScheduleBulkTemplatedEmailValidator()
    {
        RuleFor(x => x.Recipients)
            .NotEmpty().WithMessage("The 'recipients' field cannot be empty.")
            .NotNull().WithMessage("The 'recipients' field cannot be null.")
            .Must(x => x.Count > 0).WithMessage("The 'recipients' field must contain at least one recipient.");

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