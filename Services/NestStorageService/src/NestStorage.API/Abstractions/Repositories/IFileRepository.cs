using File = NestStorage.API.Entities.File;

namespace NestStorage.API.Abstractions.Repositories;

public interface IFileRepository : IRepository<File>
{
}