using Models;

namespace Data.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        IRepository<Instructor> Instructor { get; }
    }
}
