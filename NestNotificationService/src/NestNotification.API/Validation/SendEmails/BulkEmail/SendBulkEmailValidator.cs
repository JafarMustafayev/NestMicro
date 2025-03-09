namespace NestNotification.API.Validation.SendEmails.BulkEmail;

public class SendBulkEmailValidator : AbstractValidator<SendBulkEmailDto>
{
    public SendBulkEmailValidator()
    {
        RuleForEach(x => x.Recipients)
            .NotEmpty().WithMessage("The 'recipients' field cannot be empty.")
            .NotNull().WithMessage("The 'recipients' field cannot be null.")
            .EmailAddress().WithMessage("The 'recipients' field must be a valid email address format.");

        RuleFor(x => x.Recipients)
            .Must(x => x.Count > 0).WithMessage("The 'recipients' field cannot be empty.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("The 'subject' field cannot be empty.")
            .NotNull().WithMessage("The 'subject' field cannot be null.")
            .MinimumLength(1).WithMessage("The 'subject' field must be at least 2 character long.")
            .MaximumLength(128).WithMessage("The 'subject' field must be at most 128 characters long.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("The 'body' field cannot be empty.")
            .NotNull().WithMessage("The 'body' field cannot be null.");

        RuleFor(x => x.IsHtml)
            .NotNull().WithMessage("The 'isHtml' field cannot be null.");
    }
}