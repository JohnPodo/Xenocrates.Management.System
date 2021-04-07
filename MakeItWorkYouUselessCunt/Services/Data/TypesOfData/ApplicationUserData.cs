using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.Services.Data.TypesOfData
{

    public class ApplicationUserData:IDisposable
    {
        private ApplicationDbContext _context;

        public ApplicationUserData()
        {
            _context = new ApplicationDbContext();
        }

        public List<ApplicationUser> UsersPerDepartment(int id) => _context.Users.Include(w => w.Worker).Include(r => r.Roles).Where(u => u.Worker.DepartmentID == id).ToList();

        public ApplicationUser FindUserByID(string id) => _context.Users.Include(w => w.Worker).Include(w => w.Roles).Single(w => w.Id == id);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}