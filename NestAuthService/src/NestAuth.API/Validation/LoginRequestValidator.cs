namespace NestAuth.API.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .NotNull().WithMessage("Username cannot be null.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters and at most 64 characters long.")
            .MaximumLength(64).WithMessage("Username must be at least 4 characters and at most 64 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .NotNull().WithMessage("Password cannot be null.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128).WithMessage("The password must be at most 128 characters long.");
    }
}