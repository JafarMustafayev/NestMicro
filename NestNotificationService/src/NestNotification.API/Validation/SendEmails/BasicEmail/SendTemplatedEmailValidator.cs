namespace NestNotification.API.Validation.SendEmails.BasicEmail;

public class SendTemplatedEmailValidator : AbstractValidator<SendTemplatedEmailDto>
{
    public SendTemplatedEmailValidator()
    {
        RuleFor(x => x.ToEmail)
            .NotEmpty().WithMessage("The 'to' field cannot be empty.")
            .NotNull().WithMessage("The 'to' field cannot be null.")
            .MinimumLength(1).WithMessage("The 'to' field must be at least 2 character long.")
            .MaximumLength(128).WithMessage("The 'to' field must be at most 128 characters long.");

        RuleFor(x => x.TemplateId)
            .NotEmpty().WithMessage("The 'templateId' field cannot be empty.")
            .NotNull().WithMessage("The 'templateId' field cannot be null.")
            .MinimumLength(1).WithMessage("The 'templateId' field must be at least 2 character long.")
            .MaximumLength(128).WithMessage("The 'templateId' field must be at most 128 characters long.");

        RuleFor(x => x.Placeholders)
            .NotEmpty().WithMessage("The 'placeholders' field cannot be empty.")
            .NotNull().WithMessage("The 'placeholders' field cannot be null.");
    }
}