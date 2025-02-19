namespace NestAuth.API.Validation;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty.")
            .NotNull().WithMessage("Email address cannot be null.")
            .EmailAddress().WithMessage("Email must be a valid email address format.");
    }
}