namespace Nest.Shared.Abstractions;

public interface IBaseException
{
    public int StatusCode { get; }
    public string CustomMessage { get; }
}