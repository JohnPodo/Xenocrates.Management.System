using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.StatisticsModels;

namespace ManagementSystemVersionTwo.Controllers
{
    public class DisplayController : Controller
    {
        private DataRepository _data;

        public DisplayController()
        {
            _data = new DataRepository();
        }

        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
        }

        public ActionResult ViewAllDepartments(string searchString, string sort)
        {
            var data = _data.AllDepartments();
            if (!string.IsNullOrEmpty(sort))
            {
                data = _data.SortDepartments(sort, data);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                data = _data.GetDepartmentsByCity(searchString, data);
            }

            ViewBag.SortByCity = _data.DepartmentSortingOptionsViewBag();

            ViewBag.Cities = _data.DepartmentsForAutoComplete();

            return View(data);

        }

        public ActionResult ViewAllWorkers(string searchName, string orderBy, string roleSpec, string depID, string viewType)
        {
            var data = _data.AllWorkers();
            if (!string.IsNullOrEmpty(searchName))
            {
                data = _data.FindWorkerByName(searchName, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = _data.SortWorker(orderBy, data);
            }
            if (!string.IsNullOrEmpty(roleSpec))
            {
                data = _data.GetWorkersInRoleForSort(roleSpec, data);
            }
            if (!string.IsNullOrEmpty(depID))
            {
                data = _data.GetWorkersPerDepartmentForSort(int.Parse(depID), data);
            }

            ViewBag.SortOptions = _data.WorkerSortingOptionsViewBag();

            ViewBag.RoleOptions = _data.AvailableRolesFilteringViewBag();

            ViewBag.DepartmentOptions = _data.AvailableDepartmentsFilteringViewBag();

            ViewBag.Names = _data.GetWorkerNamesForAutocomplete(); 

            ViewBag.Parameters = new List<string> {searchName,orderBy,roleSpec,depID };

            if (string.IsNullOrEmpty(viewType))
            {
                return View(data);
            }
            else
            {
                return View("ViewAllWorkersList", data);
            }

        }

        public ActionResult ViewAllRoles(string searchString, string sort)
        {
            var data = _data.AllRoles();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = _data.GetRoleByName(searchString, data);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                data = _data.SortRoles(sort, data);
            }

            ViewBag.SortByRole = _data.RolesSortingOptionsViewBag();

            ViewBag.Roles = _data.RolesForAutoComplete();

            return View(data);
        }
        //View per Role
        

        public ActionResult ViewAllRoles(string searchString, string sort)
        {
            var data = _data.AllProjects();
            if (!string.IsNullOrEmpty(title))
            {
                data = _data.FindProjectByTitle(title, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = _data.SortProject(orderBy, data);
            }
            if (!string.IsNullOrEmpty(depID))
            {
                data = _data.GetProjectsPerDepartmentForSort(int.Parse(depID), data);
            }
            if (!string.IsNullOrEmpty(status))
            {
                data = _data.StatusProject(status, data);
            }

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
            ViewBag.SortOptions = listItems;

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
            ViewBag.StatusOptions = statusItems;

            var titleForAutoComplete = _data.GetProjectNamesForAutocomplete();
            ViewBag.Names = titleForAutoComplete;

            List<SelectListItem> departmentItems = new List<SelectListItem>();
            var allDepartments = _data.AllDepartments();
            foreach (var items in allDepartments)
            {
                departmentItems.Add(new SelectListItem
                {
                    Text = items.City,
                    Value = $"{items.ID}"
                });
            }
            ViewBag.DepartmentOptions = departmentItems;

            return View(data);
        }

        public ActionResult DetailsDepartment(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.FindDepartmentByID((int)id);
            if (department == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(department);
        }

        public ActionResult AdminDashboard()
        {
            return View();
        }

        //Chart For Departments Per City
        public ActionResult ChartsForAdmin()
        {
            var departmentsPerCity = _data.DepartmentsPerCityChart();
            var employeesPerDepartment = _data.EmployeesPerDepartmentChart();
            var averageSalaryPerDepartment = _data.AverageSalaryChart();
            var averageAgePerDepartment = _data.AverageAgeChart();
            var totalSalariesPerMonth = _data.TotalSalariesPerMonthChart();
            var totalSalaryPerDepartment = _data.TotalSalaryPerDepartmentChart();
            var ratioArray = new Ratio[] { departmentsPerCity, employeesPerDepartment, averageSalaryPerDepartment, averageAgePerDepartment, totalSalariesPerMonth, totalSalaryPerDepartment };

            return Json(ratioArray, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SupervisorDashboard()
        {
            return View();
        }

        public ActionResult ChartsForSupervisor()
        {
            var salaryPerEmployee = _data.SalaryPerEmployeeChart();
            var agePerEmployee = _data.AgePerEmployeeChart();
            var genderPerDepartment = _data.GenderPerDepartmentChart();
            var RatioArray = new Ratio[] { salaryPerEmployee, agePerEmployee, genderPerDepartment };


            return Json(RatioArray, JsonRequestBehavior.AllowGet);

        }



        //public ActionResult FinalizeProject(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    _data.FinalizeProject((int)id);
        //        return RedirectToAction("AllProjectsPerEmployee");
        //}


    }
}