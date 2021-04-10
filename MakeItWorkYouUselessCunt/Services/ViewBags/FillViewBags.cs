using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.Services.ViewBags
{
    public class FillViewBags : IDisposable
    {
        private ApplicationDbContext _context;

        public FillViewBags()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Returns a List<SelectListItem> of available sorting option for worker
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a List<SelectListItem> of available sorting option for Role
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> RolesSortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Name ⯅",
                Value = "Role"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Name ⯆",
                Value = "Role_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Count ⯅",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Count ⯆",
                Value = "Low-High"
            });
            return listItems;
        }

        /// <summary>
        /// Returns a List<SelectListItem> of available sorting option for PaymentDetails
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> SalarySortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Salary ⯆",
                Value = "Salary Desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Salary ⯅",
                Value = "Salary Asc"
            });
            return listItems;
        }

        /// <summary>
        /// Returns a List<SelectListItem> of available sorting option for Department
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> DepartmentSortingOptionsViewBag()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "City ⯅",
                Value = "City"
            });
            listItems.Add(new SelectListItem
            {
                Text = "City ⯆",
                Value = "City_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Count ⯅",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Count ⯆",
                Value = "Low-High"
            });
            return listItems;
        }

        /// <summary>
        /// Returns a List<SelectListItem> of available filtering option for Roles
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> AvailableRolesFilteringViewBag()
        {
            List<SelectListItem> roleItems = new List<SelectListItem>();
            var allroles = _context.Roles.Where(r=>r.Name!="Admin").ToList();
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

        /// <summary>
        /// Returns a List<SelectListItem> of available filtering option for Departments
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> AvailableDepartmentsFilteringViewBag()
        {
            List<SelectListItem> departmentItems = new List<SelectListItem>();
            var allDepartments = _context.Departments.ToList();
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

        /// <summary>
        /// Give me the list of data that reach the controller to fill the autocomplete with Fullnames
        /// </summary>
        /// <returns></returns>
        public List<string> GetWorkerNamesForAutocomplete(List<Worker> data)
        {
            var namesInAnonymous = data.Select(name => new
            {
                FirstName = name.FirstName,
                LastName = name.LastName
            }).ToList();
            List<string> names = new List<string>();
            foreach(var name in namesInAnonymous)
            {
                names.Add(name.FirstName+" "+name.LastName);
            }
            return names;
        }

        /// <summary>
        /// Find the available cities of Departments
        /// </summary>
        /// <returns></returns>
        public List<string> DepartmentsForAutoComplete() => _context.Departments.Select(x => x.City).ToList();

        /// <summary>
        /// Find the available names of Roles
        /// </summary>
        public List<string> RolesForAutoComplete() => _context.Roles.Where(r => r.Name != "Admin").Select(x => x.Name).ToList();


        /// <summary>
        /// Give me the list of data that reach the controller to fill the autocomplete with Titles
        /// </summary>
        public List<string> GetProjectNamesForAutocomplete(List<Project> data)
        {
            var titlesInAnonymous = data.Select(title => new
            {
                Title = title.Title
            }).ToList();
            List<string> titles = new List<string>();
            foreach (var title in titlesInAnonymous)
            {
                titles.Add(title.Title);
            }
            return titles;
        }

        /// <summary>
        /// Fill ViewBag with available sort options for dropdown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetProjectSortOptions()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Title",
                Value = "Title"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Employees",
                Value = "Employees"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Start Date",
                Value = "Start Date"
            });
            listItems.Add(new SelectListItem
            {
                Text = "End Date",
                Value = "End Date"
            });
            return listItems;
        }

        /// <summary>
        /// Status option For Filter
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> StatusSortOptionsViewBag()
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            statusItems.Add(new SelectListItem
            {
                Text = "Finished",
                Value = "Finished"
            });
            statusItems.Add(new SelectListItem
            {
                Text = "Not Finished",
                Value = "Not Finished"
            });
            return statusItems;
        }

        /// <summary>
        /// I fill the options to choose Department to See Projects
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ProjectInDepartmentsSortViewBag()
        {
            List<SelectListItem> departmentItems = new List<SelectListItem>();
            var allDepartments = _context.Departments.ToList();
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}