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

        /// <summary>
        /// Give me the id of a department and I will return all the users of this department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ApplicationUser> UsersPerDepartment(int id) => _context.Users.Include(w => w.Worker).Include(r => r.Roles).Where(u => u.Worker.DepartmentID == id).ToList();


        /// <summary>
        /// Give me an id and I will return the user with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationUser FindUserByID(string id) => _context.Users.Include(w => w.Worker).Include(w => w.Roles).Single(w => w.Id == id);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}