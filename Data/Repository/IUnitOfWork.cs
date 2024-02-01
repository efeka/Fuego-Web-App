using Models;

namespace Data.Repository
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        IRepository<Instructor> Instructor { get; }
        IRepository<CourseType> CourseType { get; }
        IRepository<Course> Course { get; }
        IRepository<CourseStudent> CourseStudent { get; }
        IRepository<Schedule> Schedule { get; }
    }
}
