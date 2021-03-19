using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Services.Data;
using Microsoft.AspNet.Identity;

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

        public ActionResult ViewAllDepartments()
        {
            return View(_data.AllDepartments());
        }

        


        public ActionResult ViewAllWorkers()
        {
            ViewBag.User = _data.FindUserByID(User.Identity.GetUserId()).UserName;
            return View(_data.AllWorkers());
        }

        public ActionResult ViewAllWorkersList()
        {
            return View(_data.AllWorkers());
        }

        public ActionResult ViewAllRoles()
        {
            return View(_data.AllRoles());
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






    }
}