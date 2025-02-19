namespace NestAuth.API.Validation;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty.")
            .NotNull().WithMessage("Email address cannot be null.")
            .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
            .EmailAddress().WithMessage("Email must be a valid email address format.");

        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage("Reset token cannot be empty.")
            .NotNull().WithMessage("Reset token cannot be null.")
            .MinimumLength(5).WithMessage("Reset token must be at least 5 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password cannot be empty.")
            .NotNull().WithMessage("New Password cannot be null.")
            .MinimumLength(8).WithMessage("New Password must be at least 8 characters long.")
            .MaximumLength(128).WithMessage("New Password must be at most 128 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("Confirm Password must match the New Password.")
            .NotEmpty().WithMessage("Confirm Password cannot be empty.")
            .NotNull().WithMessage("Confirm Password cannot be null.")
            .MinimumLength(8).WithMessage("Confirm Password must be at least 8 characters long.")
            .MaximumLength(128).WithMessage("Confirm Password must be at most 128 characters long.");
    }
}