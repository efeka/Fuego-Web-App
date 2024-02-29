using Models;

namespace Data.Repository
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        IRepository<Instructor> Instructor { get; }
        IRepository<CourseType> CourseType { get; }
        IRepository<Course> Course { get; }
        IRepository<CourseUser> CourseUser { get; }
        IRepository<Schedule> Schedule { get; }
        IRepository<Reservation> Reservation { get; }
    }
}
