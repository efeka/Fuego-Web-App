using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Services
{
    public interface IGenericService<T> where T : class
    {
        Task<JsonResult> GetApiDataAsync();
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
