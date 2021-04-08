using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Mvc;
using ManagementSystemVersionTwo.StatisticsModels;
using ManagementSystemVersionTwo.Services.Data.TypesOfData;

namespace ManagementSystemVersionTwo.Services.Data
{
    public class DataRepository : IDisposable
    {
        private ApplicationDbContext _context;
        public ApplicationUserData ApplicationUser;
        public DepartmentData Department;
        public RoleData Role;
        public WorkerData Worker;
        public ProjectData Project;

        public DataRepository()
        {
            _context = new ApplicationDbContext();
            ApplicationUser = new ApplicationUserData();
            Department = new DepartmentData();
            Role = new RoleData();
            Worker = new WorkerData();
            Project = new ProjectData();
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
            var allroles = Role.AllRoles();
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
            var allDepartments = Department.AllDepartments();
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
                    return Worker.AllWorkers();
            }
        }

        public List<Worker> GetWorkersInRoleForSort(string roleID, List<Worker> data)
        {
            var workers = data.Where(r => r.ApplicationUser.Roles.SingleOrDefault(w => w.RoleId == roleID) != null).ToList();
            return workers;
        }

        public List<Worker> GetWorkersPerDepartmentForSort(int id, List<Worker> data) => data.Where(u => u.DepartmentID == id).ToList();

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

        public List<IdentityRole> GetRoleByName(string searchString, List<IdentityRole> roles) => roles.Where(x => x.Name.Contains(searchString)).ToList();
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
                                .Select(st => new
                                {
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
                    return data;
            }
        }

        public List<Project> StatusProject(string status, List<Project> data)
        {
            switch (status)
            {
                case "Finished":
                    return data.Where(w => (w.Finished) == true).ToList();
                case "Not Finished":
                    return data.Where(w => (w.Finished) == false).ToList();
                default:
                    return data;
            }
        }

        public List<Project> GetProjectsPerDepartmentForSort(int id, List<Project> data) => data.Where(u => u.WorkersInMe.FirstOrDefault().Worker.DepartmentID == id).ToList();

        #endregion


        

        public List<Project> FindProjectsPerWorker(int id) => _context.Projects.Include(s => s.WorkersInMe).Where(p => p.WorkersInMe.FirstOrDefault(w => w.WorkerID == id) != null).ToList();

        public void Dispose()
        {
            _context.Dispose();
            ApplicationUser.Dispose();
            Department.Dispose();
            Role.Dispose();
            Worker.Dispose();
            Project.Dispose();
        }
    }
}