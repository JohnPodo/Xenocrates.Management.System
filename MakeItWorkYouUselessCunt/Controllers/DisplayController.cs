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
            _data=new DataRepository();
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
            ViewBag.SortByCity = listItems;


            var citiesForAutoComplete = _data.DepartmentsForAutoComplete();
            ViewBag.Cities = citiesForAutoComplete;
            return View(data);

        }




        public ActionResult ViewAllWorkers()
        {
            return View(_data.AllWorkers());
        }
        //View per Role
        public ActionResult ViewAllWorkersPerRole(string roleName)
        {
            return View();
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
            ViewBag.SortByRole = listItems;
            var rolesForAutoComplete = _data.RolesForAutoComplete();
            ViewBag.Roles = rolesForAutoComplete;

            
            return View(data);
        }

        public ActionResult FindWorkerByName(string searchName)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                return View("ViewAllWorkers", _data.AllWorkers());
            }
            else
            {
                return View("ViewAllWorkers", _data.FindWorkerByName(searchName));
            }
        }

        public ActionResult SortedWorkers(string sorting)
        {
            return View("ViewAllWorkers", _data.SortWorker(sorting));
        }

        public ActionResult ViewDepartmentWithWorkers(int? id, string city)
        {
            if (id == null && string.IsNullOrEmpty(city))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                var dep = _data.FindDepartmentByID((int)id);
                if (dep == null)
                {
                    return HttpNotFound();
                }
                return View(dep);
            }
            if (!string.IsNullOrEmpty(city))
            {
                var dep = _data.FindDepartmentByCity(city);
                if (dep == null)
                {
                    return HttpNotFound();
                }
                return View(dep);
            }
            return View("ViewAllDepartments");
        }

        public ActionResult ViewSupervisorsPerDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var supervisors = _data.FindUserPerDepartment((int)id,"Supervisor");
            
            if (supervisors == null)
            {
                return HttpNotFound();
            }
            return View(supervisors);
        }
        public ActionResult ViewAllProjects()
        {
            return View(_data.AllProjects());
        }

        public ActionResult ViewAllProjects(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projects = _data.ProjectsPerEmployee((int)id);

            return View("AllProjectsPerEmployee");
        }

        public ActionResult ViewAllActiveProjects()
        {
            var activeProjects = _data.AllActiveProjects();
            return View("ActiveProjectsPerEmployee");
        }

        public ActionResult DetailsDepartment(int? id)
        {
            
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.FindDepartmentByID((int)id);
            if(department == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(department);
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