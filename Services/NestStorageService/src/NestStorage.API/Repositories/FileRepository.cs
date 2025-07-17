using File = NestStorage.API.Entities.File;

namespace NestStorage.API.Repositories;

public class FileRepository : Repository<File>, IFileRepository
{
    public FileRepository(AppDbContext context) : base(context)
    {
    }
}