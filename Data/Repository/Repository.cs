using Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties
                        .Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;
                query = query.Where(filter);
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties
                        .Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while fetching from database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public void Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while adding to database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while adding to database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public void Update(T entity)
        {
            try
            {
                dbSet.Update(entity);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while updating database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while updating database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while deleting from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while deleting from database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                dbSet.RemoveRange(entities);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occured while deleting from database: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occured while deleting from database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
