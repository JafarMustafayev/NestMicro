namespace NestStorage.API.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntityId, new()
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        this._context = context;
    }

    public DbSet<T> Table => this._context.Set<T>();

    public IQueryable<T?> GetAll(
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        if (orderBy != null)
        {
            query = isDesingOrder ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return query;
    }

    public IQueryable<T?> GetAll(
        int page = 1, int take = 20,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        if (orderBy != null)
        {
            query = isDesingOrder ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        query = query.Skip((page - 1) * take).Take(take);

        return query;
    }

    public (IQueryable<T?> Items, int Count) GetAllByExpression(
        int page,
        int take,
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false, List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        query = query.Where(expression);

        if (orderBy != null)
        {
            query = isDesingOrder ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        var count = query.Count();

        query = query.Skip((page - 1) * take).Take(take);

        return (query, count);
    }

    public (IQueryable<T?> Items, int Count) GetAllByExpression(
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        Expression<Func<T, object>>? orderBy = null,
        bool isDesingOrder = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        query = query.Where(expression);

        if (orderBy != null)
        {
            query = isDesingOrder ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return (query, query.Count());
    }

    public async Task<T?> GetByIdAsync(
        string id,
        bool isTracking = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        return await Table.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T?> GetByExpressionAsync(
        Expression<Func<T, bool>> expression,
        bool isTracking = false,
        List<Expression<Func<T, object>>>? includes = null,
        List<(Expression<Func<T, object>> includes, Expression<Func<object, object>> thenIncludes)>? thenIncludes = null)
    {
        var query = isTracking ? Table.AsQueryable() : Table.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                Table.Include(include);
            }
        }

        if (thenIncludes != null)
        {
            foreach (var include in thenIncludes)
            {
                Table.Include(include.includes).ThenInclude(include.thenIncludes);
            }
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await Table.AnyAsync(expression);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
    {
        if (expression != null)
        {
            return await Table.Where(expression).CountAsync();
        }

        return await Table.CountAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public async Task AddRangeAsync(ICollection<T> entities)
    {
        await Table.AddRangeAsync(entities);
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public void DeleteRange(ICollection<T> entities)
    {
        Table.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}