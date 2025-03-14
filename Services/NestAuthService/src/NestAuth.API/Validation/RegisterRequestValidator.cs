namespace NestAuth.API.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .NotNull().WithMessage("Username cannot be null.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters and at most 64 characters long.")
            .MaximumLength(64).WithMessage("Username must be at least 4 characters and at most 64 characters long.")
            .Matches("^[A-Za-z0-9._-]{3,65}$").WithMessage("Username can only contain  letters, digits, hyphens (-), underscores (_), and periods (.).");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty.")
            .NotNull().WithMessage("Email address cannot be null.")
            .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
            .EmailAddress().WithMessage("Email must be a valid email address format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .NotNull().WithMessage("Password cannot be null.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128).WithMessage("The password must be at most 128 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Confirm Password must match the Password.")
            .NotEmpty().WithMessage("Confirm Password cannot be empty.")
            .NotNull().WithMessage("Confirm Password cannot be null.")
            .MinimumLength(8).WithMessage("Confirm Password must be at least 8 characters long.")
            .MaximumLength(128).WithMessage("Confirm Password must be at most 128 characters long.");
    }
}