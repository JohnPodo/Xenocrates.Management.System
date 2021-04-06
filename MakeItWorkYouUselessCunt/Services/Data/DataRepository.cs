using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Mvc;
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

        #region DataForViewbags
        public List<SelectListItem> WorkerSortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "City Of Department",
                Value = "City Of Department"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Full Name",
                Value = "Full Name"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Age",
                Value = "Age"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Salary",
                Value = "Salary"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Projects",
                Value = "Projects"
            });
            return listItems;
        }


        public List<SelectListItem> RolesSortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Role",
                Value = "Role"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Role_desc",
                Value = "Role_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "High-Low",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Low-High",
                Value = "Low-High"
            });
            return listItems;
        }

        public List<SelectListItem> SalarySortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Salary Desc",
                Value = "Salary Desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Salary Asc",
                Value = "Salary Asc"
            });
            return listItems;
        }

        public List<SelectListItem> DepartmentSortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "City",
                Value = "City"
            });
            listItems.Add(new SelectListItem
            {
                Text = "City_desc",
                Value = "City_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "High-Low",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Low-High",
                Value = "Low-High"
            });
            return listItems;
        }

        public List<SelectListItem> AvailableRolesFilteringViewBag()
        {
            List<SelectListItem> roleItems = new List<SelectListItem>();
            var allroles = AllRoles();
            foreach (var items in allroles)
            {
                roleItems.Add(new SelectListItem
                {
                    Text = items.Name,
                    Value = items.Id
                });
            }
            return roleItems;
        }


        public List<SelectListItem> AvailableDepartmentsFilteringViewBag()
        {
            List<SelectListItem> departmentItems = new List<SelectListItem>();
            var allDepartments = AllDepartments();
            foreach (var items in allDepartments)
            {
                departmentItems.Add(new SelectListItem
                {
                    Text = items.City,
                    Value = $"{items.ID}"
                });
            }
            return departmentItems;
        }

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

        public List<string> DepartmentsForAutoComplete() => _context.Departments.Select(x => x.City).ToList();

        public List<string> RolesForAutoComplete() => _context.Roles.Select(x => x.Name).ToList();


        #endregion

        #region Sorting And Filtering



        public List<Worker> FindWorkerByName(string search, List<Worker> data) => data.Where(w => (w.FirstName + " " +w.LastName).Contains(search)).ToList();

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

        public List<Worker> GetWorkersPerDepartmentForSort(int id, List<Worker> data) =>data.Where(u => u.DepartmentID == id).ToList();

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

        public List<Department> GetDepartmentsByCity(string searchString, List<Department> departments) => departments.Where(x => x.City.Contains(searchString)).ToList();

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

        #region Sorting And Filtering Payments
        public List<Worker> SortSalary(string sort, List<Worker> payments)
        {
            switch (sort)
            {
                case "Salary Desc":
                    return payments.OrderByDescending(x => x.Salary).ToList();
                case "Salary Asc":
                    return payments.OrderBy(x => x.Salary).ToList();
                default:
                    return payments;
            }
        }

        public List<PaymentDetails> SortDate(string sort, List<PaymentDetails> date)
        {
            switch (sort)
            {
                case "Date Asc":
                    return date.OrderBy(x => x.Date).ToList();
                case "Date Desc":
                    return date.OrderByDescending(x => x.Date).ToList();
                default:
                    return date;
            }
        }

        #endregion


        #region Sorting And Filtering Projects
        public List<string> GetProjectNamesForAutocomplete()
        {
            var titles = _context.Projects
                                .Select(st => new {
                                    Title = st.Title
                                }).ToList();
            List<string> data = new List<string>();
            foreach (var title in titles)
            {
                data.Add(title.Title);
            }
            return data;
        }
        public List<Project> FindProjectByTitle(string search, List<Project> data) => data.Where(w => (w.Title).Contains(search)).ToList();

        public List<Project> SortProject(string sort, List<Project> data)
        {
            switch (sort)
            {
                case "Employees":
                    return data.OrderBy(w => w.WorkersInMe.Count).ToList();
                case "Title":
                    return data.OrderBy(w => (w.Title).ToUpper()).ToList();
                case "Start Date":
                    return data.OrderBy(w => w.StartDate).ToList();
                case "End Date":
                    return data.OrderBy(w => w.EndDate).ToList();
                default:
                    return AllProjects();
            }
        }

        public List<Project> StatusProject(string status, List<Project> data)
        {
            switch (status)
            {
                case "Finished":
                    return data.Where( w => (w.Finished) == true).ToList();
                case "Not Finished":
                    return data.Where(w => (w.Finished) == false).ToList();
                default:
                    return AllProjects();
            }
        }

        public List<Project> GetProjectsPerDepartmentForSort(int id, List<Project> data) => data.Where(u => u.ID == id).ToList();

        #endregion

        #region DepartmentData
        public List<Department> AllDepartments() => _context.Departments.Include(s => s.WorkersInThisDepartment).ToList();

        public Department FindDepartmentByID(int id) => _context.Departments.Include(s => s.WorkersInThisDepartment).SingleOrDefault(s => s.ID == id);

        public Department FindDepartmentByCity(string City) => _context.Departments.Include(s => s.WorkersInThisDepartment).Single(s => s.City == City);


        #endregion

        #region RoleData

        public IdentityRole FindRoleByName(string name) => _context.Roles.Include(r => r.Users).Single(s => s.Name == name);

        public List<IdentityRole> AllRoles() => _context.Roles.Include(r => r.Users).Where(s=>s.Name!="Admin").ToList();

        public IdentityRole FindRoleByID(string id) => _context.Roles.Include(r => r.Users).Single(s => s.Id == id);

        public List<IdentityRole> GetRoleByName(string searchString, List<IdentityRole> roles) => roles.Where(x => x.Name.Contains(searchString)).ToList();


        #endregion

        #region WorkerData

        public Worker FindWorkerByID(int id) => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(w => w.MyProjects).Include(w => w.Payments).SingleOrDefault(w => w.ID == id);

        public List<Worker> WorkerPaymentDate(DateTime date)
        {
            var paymentDate = _context.Workers.Include(w => w.Payments.Where(d => d.Date == date)).ToList();
            return paymentDate;
        }
        public List<Worker> AllWorkers() => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(p=>p.MyProjects).Include(u => u.Payments).ToList();
        
        public List<Worker> GetWorkersInRole(string roleID)
        {
            var workers = _context.Workers.Where(r => r.ApplicationUser.Roles.SingleOrDefault(w => w.RoleId == roleID) != null).ToList();
            return workers;
        }



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
            int count;
            var departments = _context.Departments.Select(d => d.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departments)
            {
                
                count = departments.Count(x => x == item);
                obj.Count.Add(count);
            }
            foreach (var item in departments)
            {
                obj.Names.Add(item);
            }

            return obj;
        }

        //Gets the number of Employees Per Department
        public Ratio EmployeesPerDepartmentChart()
        {
            var departmentNames = _context.Departments.Select(x => x.City + " " + x.Adress).ToList();
            var employeesCount = _context.Departments.Select(x => x.WorkersInThisDepartment.Count()).ToList();

            Ratio obj = new Ratio();
            obj.Names = departmentNames;
            obj.Count = employeesCount;
            return obj;
        }

        //Gets the Average Salary of Employees Per Department
        public Ratio AverageSalaryChart()
        {
            decimal averageSalary;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                averageSalary = _context.Workers.Where(x => x.Department.City == item).Select(x => x.Salary).Average();
                obj.Salaries.Add(averageSalary);
            }
            obj.Names = departmentNames;
            return obj;
        }

        //Gets the Average Age of Employees Per Department
        public Ratio AverageAgeChart()
        {
            double ages;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                ages = _context.Workers.Where(x => x.Department.City == item).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).Average();
                obj.AverageAge.Add(ages);
            }
            obj.Names = departmentNames;
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
            Ratio obj = new Ratio();

            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            var departmentIds = _context.Departments.Select(x => x.ID).ToList();
            var dep = _context.Workers.ToList();
            foreach (var id in departmentIds)
            {
                var males = dep.Where(x => x.DepartmentID == id).Count(x => x.Gender == "Male");
                obj.Count.Add(males);
                var females = dep.Where(x => x.DepartmentID == id).Count(x => x.Gender == "Female");
                obj.Ages.Add(females);
            }
            
            obj.Names = departmentNames;
            return obj;

        }

        //Total Amount of Payments Per Month
        public Ratio TotalSalariesPerMonthChart()
        {
            decimal salary;
            var months = _context.Payments.Select(x => x.Date.Month).Distinct().ToList();
            Ratio obj = new Ratio();
            foreach (var item in months)
            {
                salary = _context.Payments.Where(x => x.Date.Month == item).Select(x => x.Amount).Sum();
                obj.Salaries.Add(salary);
            }
            obj.Names = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            
            return obj;
        }

        public Ratio TotalSalaryPerDepartmentChart()
        {
            
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            var departmentIds = _context.Departments.Select(x => x.ID).ToList();
            Ratio obj = new Ratio();
            obj.Names = departmentNames;

            foreach (var item in departmentIds)
            {
                obj.PaymentsPerMonth = new List<decimal>();
                for (int i = 1; i <= 12; i++)
                {
                    
                    var payments = _context.Payments.Where(x => x.Worker.DepartmentID == item && x.Date.Month == i).Sum(x=>x.Amount);
                    obj.PaymentsPerMonth.Add(payments);
                }
                obj.DepartmentsPaymentsPerMonth.Add(obj.PaymentsPerMonth);
            }
            
            return obj;
        }

        public List<Project> FindProjectsPerWorker(int id) => _context.Projects.Include(s => s.WorkersInMe).Where(p => p.WorkersInMe.FirstOrDefault(w=>w.WorkerID==id)!=null).ToList();

        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}