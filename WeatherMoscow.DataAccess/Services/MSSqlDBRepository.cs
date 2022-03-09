using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WeatherMoscow.DataAccess.Interfaces;
using WeatherMoscow.Entity.DbContext;

namespace WeatherMoscow.DataAccess.Services;

public class MSSqlDBRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    
    public MSSqlDBRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }
    
    public T GetById(long id)
    {
        return _dbSet.Find(id);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task CreateAsync(T item)
    {
        await _dbSet.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateRangeAsync(ICollection<T> items)
    {
        foreach (var item in items)
        {
            await _dbSet.AddAsync(item);
        }
        await _dbContext.SaveChangesAsync();
    }

    public bool Update(T item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
        return _dbContext.SaveChanges() != 0;
    }

    public List<T> UpdateRange(List<T> items)
    {
        items.ForEach(e =>
        {
            _dbContext.Entry(e).State = EntityState.Modified;
        });
        _dbContext.SaveChanges();
        return items;
    }

    public bool Delete(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            return false;
        }
        
        var deletedItem = _dbSet.Remove(entity);
        _dbContext.SaveChanges();
        
        return deletedItem != null;
    }

    public bool DeleteRange(List<long> ids)
    {
        var items = new List<long>();
        foreach (var item in ids)
        {
            var entity = _dbSet.Find(item);
                         
            if (entity != null)
            {
                items.Add(item);
            }
        }

        _dbSet.RemoveRange((IEnumerable<T>)items);
        var res = _dbContext.SaveChanges();
        return res == items.Count;
    }

    public bool DeleteRange(List<T> items)
    {
        _dbSet.RemoveRange(items);
        var res = _dbContext.SaveChanges();
        return res == items.Count;
    }

    public IQueryable<T> Include(params Expression<Func<T, object>>[] children)
    {
        children.ToList().ForEach(x=>_dbSet.Include(x).Load());
        return _dbSet;
    }
}