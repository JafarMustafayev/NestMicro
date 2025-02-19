namespace Nest.Shared.Abstractions;

public interface IBaseException
{
    public string ErrorMessage { get; }
    public int StatusCode { get; }
}