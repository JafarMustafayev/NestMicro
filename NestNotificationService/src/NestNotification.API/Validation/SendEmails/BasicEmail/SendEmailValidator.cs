namespace NestNotification.API.Validation.SendEmails.BasicEmail;

public class SendEmailValidator : AbstractValidator<SendEmailDto>
{
    public SendEmailValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty().WithMessage("The 'to' field cannot be empty.")
            .NotNull().WithMessage("The 'to' field cannot be null.")
            .MinimumLength(1).WithMessage("The 'to' field must be at least 2 character long.")
            .MaximumLength(128).WithMessage("The 'to' field must be at most 128 characters long.");

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