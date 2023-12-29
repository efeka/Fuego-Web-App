using Data.Data;
using Models;

namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IRepository<Instructor> Instructor { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Instructor = new Repository<Instructor>(db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
