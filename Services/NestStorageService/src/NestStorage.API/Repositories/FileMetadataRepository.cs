namespace NestStorage.API.Repositories;

public class FileMetadataRepository : Repository<FileMetadata>, IFileMetadataRepository
{
    public FileMetadataRepository(AppDbContext context) : base(context)
    {
    }
}