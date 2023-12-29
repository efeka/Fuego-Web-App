using Models;

namespace Data.Repository
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
        IRepository<Instructor> Instructor { get; }
    }
}
