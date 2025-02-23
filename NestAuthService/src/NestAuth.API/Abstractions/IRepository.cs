namespace NestAuth.API.Abstractions;

public interface IRepository<T> where T : BaseEntityID, new()
{
    DbSet<T> Table { get; }

    public (IQueryable<T?> Items, int Count) GetAllByExpression(Expression<Func<T, bool>> expression);

    public Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    Task AddAsync(T entity);

    void Update(T entity);

    Task SaveChangesAsync();
}