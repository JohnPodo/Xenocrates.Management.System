using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ManagementSystemVersionTwo.Services.Data
{
    public class DataRepository : IDisposable
    {
        private ApplicationDbContext _context;

        public DataRepository()
        {
            _context = new ApplicationDbContext();
        }

        #region DepartmentData
        public List<Department> AllDepartments() => _context.Departments.Include(s => s.WorkersInThisDepartment).ToList();

        public Department FindDepartmentByID(int id) => _context.Departments.Include(s => s.WorkersInThisDepartment).SingleOrDefault(s => s.ID == id);

        public Department FindDepartmentByCity(string City) => _context.Departments.Include(s => s.WorkersInThisDepartment).Single(s => s.City == City);
        #endregion

        #region RoleData
        public List<IdentityRole> AllRoles() => _context.Roles.Include(r => r.Users).ToList();

        public IdentityRole FindRoleByID(string id) => _context.Roles.Include(r => r.Users).Single(s => s.Id == id);

        #endregion

        #region WorkerData

        public List<Worker> AllWorkers() => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).ToList();
        
        public List<Worker> FindWorkerByName(string search) => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Where(w=>w.FullName.Contains(search)).ToList();

        public List<Worker> SortWorker(string sort) {
            switch (sort)
            {
                case "City Of Department":
                    return _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).OrderBy(w => w.Department.City).ToList();
                case "Full Name":
                    return _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).OrderBy(w=>w.FullName).ToList();
                default:
                    return AllWorkers();
            }
        }

        #endregion

        #region ApplicationUserData
        public ApplicationUser FindUserByID(string id) => _context.Users.Find(id);

        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}