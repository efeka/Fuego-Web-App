using Data.Data;
using Exceptions;
using Models;

namespace Services
{
    public class ApplicationUserService
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<ApplicationUser> GetAll()
        {
            List<ApplicationUser> userList = _db.ApplicationUsers.ToList();

            var roles = _db.Roles.ToList();
            var userRoles = _db.UserRoles.ToList();

            foreach (var user in userList)
            {
                // Get the row with the userId from the AspNetUserRoles table
                var userRole = userRoles.FirstOrDefault(u => u.UserId == user.Id);
                if (userRole == null)
                    continue;

                // Get the row with the roleId from the AspNetRoles table
                var role = roles.FirstOrDefault(u => u.Id == userRole.RoleId);
                if (role == null)
                    continue;

                user.RoleName = role.Name ?? "";
            }

            return userList;
        }

        public ApplicationUser? Get(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public void DeleteAsync(string id)
        {
            ApplicationUser? appUser = _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null)
                throw new EntityNotFoundException();

            _db.ApplicationUsers.Remove(appUser);
            _db.SaveChanges();
        }

    }
}
