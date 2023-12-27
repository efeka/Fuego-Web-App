using System.Linq.Expressions;

namespace Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
