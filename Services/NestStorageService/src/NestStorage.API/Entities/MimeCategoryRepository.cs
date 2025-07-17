namespace NestStorage.API.Entities;

public class MimeCategoryRepository : Repository<MimeCategory>, IMimeCategoryRepository
{
    public MimeCategoryRepository(AppDbContext context) : base(context)
    {
    }
}