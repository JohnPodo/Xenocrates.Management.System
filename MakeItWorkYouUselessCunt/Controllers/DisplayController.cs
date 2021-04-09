using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Services.Data;
using Microsoft.AspNet.Identity;
using ManagementSystemVersionTwo.StatisticsModels;
using ManagementSystemVersionTwo.Services.StatisticsServices;
using ManagementSystemVersionTwo.Services.Sorting;
using ManagementSystemVersionTwo.Services.Filtering;
using ManagementSystemVersionTwo.Services.ViewBags;
using ManagementSystemVersionTwo.Services.SortingAndFiltering;

namespace ManagementSystemVersionTwo.Controllers
{
    public class DisplayController : Controller
    {
        private DataRepository _data;
        private StatisticsForDashboard _stats;
        private FillViewBags _fillViewBag;

        public DisplayController()
        {
            _data = new DataRepository();
            _stats = new StatisticsForDashboard();
            _fillViewBag = new FillViewBags();
        }

        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
            _fillViewBag.Dispose();
            _stats.Dispose();
        }

        public ActionResult ViewAllDepartments(string searchString, string sort)
        {
            var data = _data.Department.AllDepartments();

            data = SortingAndFilteringData.SortAndFilterDepartments(searchString, sort, data);

            ViewBag.SortByCity = _fillViewBag.DepartmentSortingOptionsViewBag();

            ViewBag.Cities = _fillViewBag.DepartmentsForAutoComplete();

            return View(data);

        }

        public ActionResult ViewAllWorkers(string searchName, string orderBy, string roleSpec, string depID, string viewType)
        {
            ViewBag.Supervisor = User.IsInRole("Supervisor");

            var data = _data.Worker.AllWorkers();

            data = SortingAndFilteringData.SortAndFilterWorkers(searchName, orderBy, roleSpec, depID, data);

            if (User.IsInRole("Supervisor"))
            {
                var user = _data.ApplicationUser.FindUserByID(User.Identity.GetUserId());
                data = FilteringServices.FilterWorkersPerDepartment(user.Worker.DepartmentID, data);
            }

            ViewBag.SortOptions = _fillViewBag.WorkerSortingOptionsViewBag();

            ViewBag.RoleOptions = _fillViewBag.AvailableRolesFilteringViewBag();

            ViewBag.DepartmentOptions = _fillViewBag.AvailableDepartmentsFilteringViewBag();

            ViewBag.Names = _fillViewBag.GetWorkerNamesForAutocomplete(data);

            ViewBag.Parameters = new List<string> { searchName, orderBy, roleSpec, depID };

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
            var data = _data.Role.AllRoles();

            data = SortingAndFilteringData.SortAndFilterRoles(searchString, sort, data);

            ViewBag.SortByRole = _fillViewBag.RolesSortingOptionsViewBag();

            ViewBag.Roles = _fillViewBag.RolesForAutoComplete();

            return View(data);
        }

        public ActionResult ViewAllProjects(string title, string orderBy, string depID, string status)
        {
            ViewBag.Admin = User.IsInRole("Admin");

            ViewBag.Supervisor = User.IsInRole("Supervisor");

            var data = _data.Project.AllProjects();

            data = SortingAndFilteringData.SortAndFilterProjects(title,orderBy,depID,status,data);
           
            if (ViewBag.Supervisor)
            {
                var user = _data.ApplicationUser.FindUserByID(User.Identity.GetUserId());
                data = FilteringServices.FilterProjectsPerDepartment(user.Worker.DepartmentID, data);
            }
            else if (!ViewBag.Admin && !ViewBag.Supervisor)
            {
                var user = _data.ApplicationUser.FindUserByID(User.Identity.GetUserId());
                data = FilteringServices.FilterProjectsPerDepartment(user.Worker.DepartmentID, data);
            }

            ViewBag.SortOptions = _fillViewBag.GetProjectSortOptions();

            ViewBag.StatusOptions = _fillViewBag.StatusSortOptionsViewBag();

            ViewBag.DepartmentOptions = _fillViewBag.ProjectInDepartmentsSortViewBag();

            ViewBag.Names = _fillViewBag.GetProjectNamesForAutocomplete(data);

            return View(data);
        }

        public ActionResult DetailsWorker(string id)
        {
            ViewBag.Role = User.IsInRole("Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var worker = _data.ApplicationUser.FindUserByID(id);
            if (worker == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(worker);
        }

        public ActionResult DetailsProject(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _data.Project.FindProjectById((int)id);
            if (project is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(project);
        }

        public ActionResult DetailsDepartment(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.Department.FindDepartmentByID((int)id);
            if (department == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(department);
        }

        public ActionResult Dashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return View("AdminDashboard");
            }
            else if (User.IsInRole("Supervisor"))
            {
                return View("SupervisorDashboard");
            }
            else
            {
                return View("EmployeeDashboard");
            }
        }

        //Chart For Departments Per City
        public ActionResult ChartsForAdmin()
        {
            var departmentsPerCity = _stats.DepartmentsPerCityChart();
            var employeesPerDepartment = _stats.EmployeesPerDepartmentChart();
            var averageSalaryPerDepartment = _stats.AverageSalaryChart();
            var averageAgePerDepartment = _stats.AverageAgeChart();
            var totalSalariesPerMonth = _stats.TotalSalariesPerMonthChart();
            var totalSalaryPerDepartment = _stats.TotalSalaryPerDepartmentChart();
            var genderPerDepartment = _stats.GenderPerDepartmentChart();
            var ratioArray = new Ratio[] { departmentsPerCity, employeesPerDepartment, averageSalaryPerDepartment, averageAgePerDepartment, totalSalariesPerMonth, totalSalaryPerDepartment, genderPerDepartment };

            return Json(ratioArray, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ChartsForSupervisor()
        {
            var salaryPerEmployee = _stats.SalaryPerEmployeeChart(User.Identity.GetUserId());
            var agePerEmployee = _stats.AgePerEmployeeChart(User.Identity.GetUserId());
            var projectsPerMonth = _stats.ProjectsPerMonthChart(User.Identity.GetUserId());

            var RatioArray = new Ratio[] { salaryPerEmployee, agePerEmployee, projectsPerMonth };

            return Json(RatioArray, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NavbarPartial()
        {
            var user = _data.ApplicationUser.FindUserByID(User.Identity.GetUserId());
            var ifIsAdmin = User.IsInRole("Admin");
            var ifIsSupervisor = User.IsInRole("Supervisor");
            ViewBag.roleIdentity = ifIsAdmin;
            if (ifIsAdmin)
            {
                ViewBag.roleIcon = "/EIKONES/boss.png";
            }
            else if (!ifIsAdmin && ifIsSupervisor)
            {
                ViewBag.roleIcon = "/EIKONES/boss.png";
            }
            else
            {
                ViewBag.roleIcon = "/EIKONES/boss.png";
            }
            return PartialView(user);
        }

        public ActionResult SidebarPartial()
        {
            var ifIsAdmin = User.IsInRole("Admin");
            var ifIsSupervisor = User.IsInRole("Supervisor");
            ViewBag.Admin = ifIsAdmin;
            ViewBag.Supervisor = ifIsSupervisor;
            return PartialView();
        }




    }
}