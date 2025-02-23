namespace NestAuth.API.Validation;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token cannot be empty.")
            .NotNull().WithMessage("Refresh token cannot be null.")
            .MinimumLength(1).WithMessage("Refresh token must be at least 1 character long.")
            .MaximumLength(128).WithMessage("Refresh token must be at most 128 characters long.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.")
            .NotNull().WithMessage("User ID cannot be null.")
            .MinimumLength(1).WithMessage("User ID must be at least 1 character long.")
            .MaximumLength(128).WithMessage("User ID must be at most 128 characters long.");
    }
}