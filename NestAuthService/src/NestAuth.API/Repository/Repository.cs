namespace NestAuth.API.Repository;

public class Repository<T> : IRepository<T> where T : BaseEntityID, new()

{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public (IQueryable<T?> Items, int Count) GetAllByExpression(Expression<Func<T, bool>> expression)
    {
        var query = Table.AsQueryable();

        query = query.Where(expression);
        var count = query.Count();

        return (query, count);
    }

    public async Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        var query = Table.AsQueryable();

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await Table.AnyAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}