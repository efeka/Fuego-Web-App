﻿using System.Linq.Expressions;

namespace Services
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
