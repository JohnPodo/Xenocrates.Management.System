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

        public IdentityRole FindRoleByName(string name) => _context.Roles.Include(r => r.Users).Single(s => s.Name == name);

        public List<IdentityRole> AllRoles() => _context.Roles.Include(r => r.Users).Where(s=>s.Name!="Admin").ToList();

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
        public List<ApplicationUser> WorkersPerDepartment(int id) => _context.Users.Include(w => w.Worker).Include(r => r.Roles).Where(u => u.Worker.DepartmentID == id).ToList();

        public ApplicationUser FindUserByID(string id) => _context.Users.Include(w=>w.Worker).Include(w=>w.Roles).Single(w=>w.Id==id);

        public List<ApplicationUser> FindUserPerDepartment(int depId, string name) => _context.Users.Include(w => w.Worker).Where(w => w.Worker.DepartmentID == depId).ToList();

        public List<ProjectsAssignedToEmployee> ProjectsPerEmployee(int id) => _context.ProjectsToEmployees.Include(s => s.Project).Include(w=>w.Worker).Where(s => s.WorkerID == id).ToList();
        
        public List<ProjectsAssignedToEmployee> ActiveProjectsPerEmployee(int workerId, int projectId) => _context.ProjectsToEmployees.Include(s => s.Worker).Include(p => p.Project).Where(s => s.WorkerID == workerId).Where(p => p.ProjectID == projectId).ToList();

        #endregion

        #region ProjectData

        public Project FindProjectById(int id) => _context.Projects.Include(s => s.WorkersInMe).SingleOrDefault(p => p.ID == id);

        public List<Project> AllProjects() => _context.Projects.Include(s => s.WorkersInMe).ToList();

        public List<Project> AllActiveProjects() => _context.Projects.Include(s => s.WorkersInMe).Where(s=>s.Finished==false).ToList();

        public List<Project> AllFinishedProjects() => _context.Projects.Include(s => s.WorkersInMe).Where(s=>s.Finished==true).ToList();
        #endregion


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}