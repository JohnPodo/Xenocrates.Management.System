using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Services.Data;

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

        public ActionResult ViewAllWorkers(string searchName, string orderBy, string roleSpec, string depID)
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

            ViewBag.Names = _data.GetWorkerNamesForAutocomplete(); ;



            return View(data);
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

        public ActionResult ViewAllProjects()
        {
            return View(_data.AllProjects());
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

        public ActionResult DetailsWorker(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.FindUserByID(id);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(user);
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