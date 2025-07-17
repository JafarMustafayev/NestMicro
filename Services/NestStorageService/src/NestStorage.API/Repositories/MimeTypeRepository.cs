namespace NestStorage.API.Repositories;

public class MimeTypeRepository : Repository<MimeType>, IMimeTypeRepository
{
    public MimeTypeRepository(AppDbContext context) : base(context)
    {
    }
}