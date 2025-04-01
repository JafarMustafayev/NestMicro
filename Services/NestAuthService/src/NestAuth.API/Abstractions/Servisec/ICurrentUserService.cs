namespace NestAuth.API.Abstractions.Servisec;

public interface ICurrentUserDataService
{
    string? GetUserId { get; }

    string? GetEmail { get; }

    string? GetUsername { get; }

    bool IsAuthenticated { get; }
}