using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ManagementSystemVersionTwo.StatisticsModels;

namespace ManagementSystemVersionTwo.Services.Data
{
    public class DataRepository : IDisposable
    {
        private ApplicationDbContext _context;

        public DataRepository()
        {
            _context = new ApplicationDbContext();
        }

        #region Sorting And Filtering

        public List<string> GetWorkerNamesForAutocomplete()
        {
            var names = _context.Workers
                                .Select(st => new
                                {
                                    FirstName = st.FirstName,
                                    LastName = st.LastName
                                }).ToList();
            List<string> data = new List<string>();
            foreach (var name in names)
            {
                data.Add(name.FirstName + " " + name.LastName);
            }
            return data;
        }

        public List<Worker> FindWorkerByName(string search, List<Worker> data) => data.Where(w => (w.FirstName + " " + w.LastName).Contains(search)).ToList();

        public List<Worker> SortWorker(string sort, List<Worker> data)
        {

            switch (sort)
            {
                case "City Of Department":
                    return data.OrderBy(w => w.Department.City).ToList();
                case "Full Name":
                    return data.OrderBy(w => (w.FirstName + " " + w.LastName).ToUpper()).ToList();
                case "Age":
                    return data.OrderBy(w => w.DateOfBirth).ToList();
                case "Salary":
                    return data.OrderBy(w => w.Salary).ToList();
                case "Projects":
                    return data.OrderBy(w => w.MyProjects.Count).ToList();
                default:
                    return AllWorkers();
            }
        }

        public List<Worker> GetWorkersInRoleForSort(string roleID, List<Worker> data)
        {
            var workers = data.Where(r => r.ApplicationUser.Roles.SingleOrDefault(w => w.RoleId == roleID) != null).ToList();
            return workers;
        }

        public List<Worker> GetWorkersPerDepartmentForSort(int id, List<Worker> data) => data.Where(u => u.DepartmentID == id).ToList();


        #endregion

        #region DepartmentData
        public List<Department> AllDepartments() => _context.Departments.Include(s => s.WorkersInThisDepartment).ToList();

        public Department FindDepartmentByID(int id) => _context.Departments.Include(s => s.WorkersInThisDepartment).SingleOrDefault(s => s.ID == id);

        public Department FindDepartmentByCity(string City) => _context.Departments.Include(s => s.WorkersInThisDepartment).Single(s => s.City == City);

        //Filtering and Sorting
        public List<Department> GetDepartmentsByCity(string searchString, List<Department> departments) => departments.Where(x => x.City.Contains(searchString)).ToList();

        public List<string> DepartmentsForAutoComplete() => _context.Departments.Select(x => x.City).ToList();



        public List<Department> SortDepartments(string sort, List<Department> departments)
        {
            switch (sort)
            {
                case "City":
                    return departments.OrderBy(x => x.City).ToList();

                case "City_desc":
                    return departments.OrderByDescending(x => x.City).ToList();
                case "Low-High":
                    return departments.OrderBy(x => x.WorkersInThisDepartment.Count).ToList();

                case "High-Low":
                    return departments.OrderByDescending(x => x.WorkersInThisDepartment.Count).ToList();
                default:
                    return departments;

            }
        }



        #endregion

        #region RoleData

        public IdentityRole FindRoleByName(string name) => _context.Roles.Include(r => r.Users).Single(s => s.Name == name);

        public List<IdentityRole> AllRoles() => _context.Roles.Include(r => r.Users).Where(s => s.Name != "Admin").ToList();

        public IdentityRole FindRoleByID(string id) => _context.Roles.Include(r => r.Users).Single(s => s.Id == id);

        public List<IdentityRole> GetRoleByName(string searchString, List<IdentityRole> roles) => roles.Where(x => x.Name.Contains(searchString)).ToList();

        public List<string> RolesForAutoComplete() => _context.Roles.Select(x => x.Name).ToList();
        public List<IdentityRole> SortRoles(string sort, List<IdentityRole> roles)
        {
            switch (sort)
            {
                case "Role":
                    return roles.OrderBy(x => x.Name).ToList();

                case "Role_desc":
                    return roles.OrderByDescending(x => x.Name).ToList();
                case "Low-High":
                    return roles.OrderBy(x => x.Users.Count).ToList();

                case "High-Low":
                    return roles.OrderByDescending(x => x.Users.Count).ToList();
                default:
                    return roles;

            }
        }

        #endregion

        #region WorkerData

        public Worker FindWorkerByID(int id) => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(w => w.MyProjects).Include(w => w.Payments).Single(w => w.ID == id);

        public List<Worker> AllWorkers() => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(p => p.MyProjects).ToList();

        public List<Worker> GetWorkersInRole(string roleID)
        {
            var workers = _context.Workers.Where(r => r.ApplicationUser.Roles.SingleOrDefault(w => w.RoleId == roleID) != null).ToList();
            return workers;
        }

        public List<Worker> GetWorkersPerDepartment(int id) => _context.Workers.Include(w => w.ApplicationUser).Include(s => s.Department).Where(u => u.DepartmentID == id).ToList();


        #endregion

        #region ApplicationUserData
        public List<ApplicationUser> UsersPerDepartment(int id) => _context.Users.Include(w => w.Worker).Include(r => r.Roles).Where(u => u.Worker.DepartmentID == id).ToList();

        public ApplicationUser FindUserByID(string id) => _context.Users.Include(w => w.Worker).Include(w => w.Roles).Single(w => w.Id == id);
        public List<ProjectsAssignedToEmployee> ProjectsPerEmployee(int id) => _context.ProjectsToEmployees.Include(s => s.Project).Include(w => w.Worker).Where(s => s.WorkerID == id).ToList();

        public List<ProjectsAssignedToEmployee> ActiveProjectsPerEmployee(int workerId, int projectId) => _context.ProjectsToEmployees.Include(s => s.Worker).Include(p => p.Project).Where(s => s.WorkerID == workerId).Where(p => p.ProjectID == projectId).ToList();

        #endregion

        #region ProjectData

        public Project FindProjectById(int id) => _context.Projects.Include(s => s.WorkersInMe).SingleOrDefault(p => p.ID == id);

        public List<Project> AllProjects() => _context.Projects.Include(s => s.WorkersInMe).ToList();

        public List<Project> AllActiveProjects() => _context.Projects.Include(s => s.WorkersInMe).Where(s => s.Finished == false).ToList();

        public List<Project> AllFinishedProjects() => _context.Projects.Include(s => s.WorkersInMe).Where(s => s.Finished == true).ToList();
        #endregion

        #region Statistics

        //Gets the number of Departments Per City
        public Ratio DepartmentsPerCityChart()
        {
            int athens = _context.Departments.Where(d => d.City == "Athens").Count();
            int thessaloniki = _context.Departments.Where(d => d.City == "Thessaloniki").Count();
            Ratio obj = new Ratio();
            obj.Athens = athens;
            obj.Thessaloniki = thessaloniki;
            return obj;
        }

        //Gets the number of Employees Per Department
        public Ratio EmployeesPerDepartmentChart()
        {
            int athens = _context.Workers.Where(d => d.Department.City == "Athens").Count();
            int thessaloniki = _context.Workers.Where(d => d.Department.City == "Thessaloniki").Count();
            Ratio obj = new Ratio();
            obj.Athens = athens;
            obj.Thessaloniki = thessaloniki;
            return obj;
        }

        //Gets the Average Salary of Employees Per Department
        public Ratio AverageSalaryChart()
        {
            var athens = _context.Workers.Where(d => d.Department.City == "Athens").Select(x => x.Salary).Average();
            var thessaloniki = _context.Workers.Where(d => d.Department.City == "Thessaloniki").Select(x => x.Salary).Average();
            Ratio obj = new Ratio();
            obj.SalaryAthens = athens;
            obj.SalaryThessaloniki = thessaloniki;
            return obj;
        }

        //Gets the Average Age of Employees Per Department
        public Ratio AverageAgeChart()
        {
            var averageAgeAthens = _context.Workers.Where(d => d.DepartmentID == 1).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).Average();

            var averageAgeThessaloniki = _context.Workers.Where(d => d.DepartmentID == 2).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).Average();
            Ratio obj = new Ratio();
            obj.AgeAthens = (double)averageAgeAthens;
            obj.AgeThessaloniki = (double)averageAgeThessaloniki;
            return obj;

        }

        //Gets the Salary of Each Employee
        public Ratio SalaryPerEmployeeChart()
        {
            var salaries = _context.Workers.Select(x => x.Salary).ToList();
            var names = _context.Workers.Select(w => (w.FirstName + " " + w.LastName)).ToList();
            Ratio obj = new Ratio();
            obj.Salaries = salaries;
            obj.Names = names;
            return obj;
        }

        //Gets The Age of each Employee
        public Ratio AgePerEmployeeChart()
        {
            var ages = _context.Workers.Select(x => DateTime.Now.Year - x.DateOfBirth.Year).ToList();
            var names = _context.Workers.Select(w => (w.FirstName + " " + w.LastName)).ToList();
            Ratio obj = new Ratio();
            obj.Ages = ages;
            obj.Names = names;
            return obj;
        }

        //Gets the Number of Male and Female Employees Per Department 
        public Ratio GenderPerDepartmentChart()
        {
            var maleAthens = _context.Workers.Where(d => d.DepartmentID == 1).Where(x => x.Gender == "Male").Count();
            var maleThessaloniki = _context.Workers.Where(d => d.DepartmentID == 2).Where(x => x.Gender == "Male").Count();

            var femaleAthens = _context.Workers.Where(d => d.DepartmentID == 1).Where(x => x.Gender == "Female").Count();
            var femaleThessaloniki = _context.Workers.Where(d => d.DepartmentID == 2).Where(x => x.Gender == "Female").Count();
            Ratio obj = new Ratio();
            obj.MaleAthens = maleAthens;
            obj.MaleThessaloniki = maleThessaloniki;
            obj.FemaleAthens = femaleAthens;
            obj.FemaleThessaloniki = femaleThessaloniki;
            return obj;

        }

        //Total Amount of Payments Per Department
        public Ratio TotalSalariesPerMonthChart()
        {
            var january = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 01).Select(x => x.Amount).Sum();
            var february = _context.Payments.Where(x => x.Date.Month == 02).Select(x => x.Amount).Sum();
            var march = _context.Payments.Where(x => x.Date.Month == 03).Select(x => x.Amount).Sum();
            var april = _context.Payments.Where(x => x.Date.Month == 04).Select(x => x.Amount).Sum();
            var may = _context.Payments.Where(x => x.Date.Month == 05).Select(x => x.Amount).Sum();
            var june = _context.Payments.Where(x => x.Date.Month == 06).Select(x => x.Amount).Sum();
            var july = _context.Payments.Where(x => x.Date.Month == 07).Select(x => x.Amount).Sum();
            var august = _context.Payments.Where(x => x.Date.Month == 08).Select(x => x.Amount).Sum();
            var september = _context.Payments.Where(x => x.Date.Month == 09).Select(x => x.Amount).Sum();
            var october = _context.Payments.Where(x => x.Date.Month == 10).Select(x => x.Amount).Sum();
            var november = _context.Payments.Where(x => x.Date.Month == 11).Select(x => x.Amount).Sum();
            var december = _context.Payments.Where(x => x.Date.Month == 12).Select(x => x.Amount).Sum();
            Ratio obj = new Ratio();
            obj.Months = new List<decimal>() { january, february, march, april, may, june, july, august, september, october, november, december };
            return obj;
        }

        public Ratio TotalSalaryPerDepartmentChart()
        {
            var januaryAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 01).Select(x => x.Amount).Sum();
            var januaryThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 01).Select(x => x.Amount).Sum();
            var februaryAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 02).Select(x => x.Amount).Sum();
            var februaryThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 02).Select(x => x.Amount).Sum();
            var marchAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 03).Select(x => x.Amount).Sum();
            var marchThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 03).Select(x => x.Amount).Sum();
            var aprilAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 04).Select(x => x.Amount).Sum();
            var aprilThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 04).Select(x => x.Amount).Sum();
            var mayAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 05).Select(x => x.Amount).Sum();
            var mayThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 05).Select(x => x.Amount).Sum();
            var juneAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 06).Select(x => x.Amount).Sum();
            var juneThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 06).Select(x => x.Amount).Sum();
            var julyAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 07).Select(x => x.Amount).Sum();
            var julyThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 07).Select(x => x.Amount).Sum();
            var augustAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 08).Select(x => x.Amount).Sum();
            var augustThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 08).Select(x => x.Amount).Sum();
            var septemberAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 09).Select(x => x.Amount).Sum();
            var septemberThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 09).Select(x => x.Amount).Sum();
            var octoberAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 10).Select(x => x.Amount).Sum();
            var octoberThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 10).Select(x => x.Amount).Sum();
            var novemberAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 11).Select(x => x.Amount).Sum();
            var novemberThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 11).Select(x => x.Amount).Sum();
            var decemberAthens = _context.Payments.Where(x => x.Worker.DepartmentID == 1 && x.Date.Month == 12).Select(x => x.Amount).Sum();
            var decemberThessaloniki = _context.Payments.Where(x => x.Worker.DepartmentID == 2 && x.Date.Month == 12).Select(x => x.Amount).Sum();
            Ratio obj = new Ratio();
            obj.SalariesPerMonthAthens = new List<decimal>() { januaryAthens, februaryAthens, marchAthens, aprilAthens, marchAthens, januaryAthens, januaryAthens, augustAthens, septemberAthens, octoberAthens, novemberAthens, decemberAthens};
            obj.SalariesPerMonthThessaloniki = new List<decimal>() { januaryThessaloniki, februaryThessaloniki, marchThessaloniki, aprilThessaloniki, mayThessaloniki, juneThessaloniki, julyThessaloniki, augustThessaloniki, septemberThessaloniki, octoberThessaloniki, novemberThessaloniki, decemberThessaloniki};
            return obj;
        }

        #endregion

        public int GetPaymentsCount() => _context.Payments.Count();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}