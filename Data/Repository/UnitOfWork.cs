using Data.Data;
using Models;

namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IRepository<Instructor> Instructor { get; private set; }
        public IRepository<CourseType> CourseType { get; private set; }
        public IRepository<Course> Course { get; private set; }
        public IRepository<CourseStudent> CourseStudent { get; private set; }
        public IRepository<Schedule> Schedule { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Instructor = new Repository<Instructor>(db);
            CourseType = new Repository<CourseType>(db);
            Course = new Repository<Course>(db);
            CourseStudent = new Repository<CourseStudent>(db);
            Schedule = new Repository<Schedule>(db);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
