namespace NestAuth.API.Validation;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty.")
            .NotNull().WithMessage("UserId cannot be null.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current Password cannot be empty.")
            .NotNull().WithMessage("Current Password cannot be null.");

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