using Microsoft.EntityFrameworkCore;
using MyTicket.Application.Interfaces.IRepositories;
using System.Linq.Expressions;
using MyTicket.Persistence.Context;
using LinqKit;

namespace MyTicket.Persistence.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query=query.Include(include);
            }
        }
        return await (filter == null ? query.ToListAsync() : query.Where(filter).ToListAsync());
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query=query.Include(include);
            }
        }
        return await (filter == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(filter));
    }

    public void HardDelete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    async Task<bool> IRepository<T>.IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id)
    {
        var dbSet = _context.Set<T>();

        // Dinamik predicate yaradılır
        var predicate = PredicateBuilder.New<T>();

        // Propertinin dəyəri ilə müqayisə edilərək predicate yaradılır
        predicate = predicate.And(Expression.Lambda<Func<T, bool>>(
            Expression.Equal(propertySelector.Body, Expression.Constant(value)),
            propertySelector.Parameters));

        // ID varsa onu istisna edirik
        if (id != 0)
        {
            predicate = predicate.And(e => EF.Property<int>(e, "Id") != id);
        }

        // Predicate tətbiq edilərək yoxlanılır
        return !await dbSet.AsExpandable().AnyAsync(predicate);
    }
}

