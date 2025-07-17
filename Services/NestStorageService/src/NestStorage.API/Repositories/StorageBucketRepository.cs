namespace NestStorage.API.Repositories;

public class StorageBucketRepository : Repository<StorageBucket>, IStorageBucketRepository
{
    public StorageBucketRepository(AppDbContext context) : base(context)
    {
    }
}