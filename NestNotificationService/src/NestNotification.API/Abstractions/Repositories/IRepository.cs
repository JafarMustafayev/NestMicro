namespace NestNotification.API.Abstractions.Repositories;

public interface IRepository<T> where T : BaseEntityID, new()
{
    DbSet<T> Table { get; }

    public IQueryable<T?> GetAll(
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null);

    public IQueryable<T?> GetAll(
        int page = 1,
        int take = 20,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null);

    public (IQueryable<T?> Items, int Count) GetAllByExpression(
        int page,
        int take,
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null);

    public (IQueryable<T?> Items, int Count) GetAllByExpression(
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null);

    public Task<T?> GetByIdAsync(
        string id,
        bool isTracking = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenincludes)>? thenincludes = null);

    public Task<T?> GetByExpressionAsync(
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenincludes)>? thenincludes = null);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    public Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);

    Task AddAsync(T entity);

    Task AddRangeAsync(ICollection<T> entities);

    void Delete(T entity);

    void DeleteRange(ICollection<T> entities);

    void Update(T entity);

    Task SaveChangesAsync();
}