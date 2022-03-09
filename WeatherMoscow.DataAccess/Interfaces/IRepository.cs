using System.Linq.Expressions;

namespace WeatherMoscow.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    T GetById(long id);
    public IQueryable<T> GetAll();
    Task CreateAsync(T item);
    public Task CreateRangeAsync(ICollection<T> items);
    bool Update(T item);
    public List<T> UpdateRange(List<T> items);
    bool Delete(long id);
    bool DeleteRange(List<long> ids);
    bool DeleteRange(List<T> items);
    public IQueryable<T> Include(params Expression<Func<T, object>>[] children);
}