using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<Course> Courses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
