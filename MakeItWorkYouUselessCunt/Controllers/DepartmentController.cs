using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.Services.DepartmentServices;
using Microsoft.AspNet.Identity;

namespace ManagementSystemVersionTwo.Controllers
{
    public class DepartmentController : Controller
    {
        private CRUDDepartment _crud;
        private DataRepository _data;

        public DepartmentController()
        {
            _crud = new CRUDDepartment();
            _data = new DataRepository();
        }

        protected override void Dispose(bool disposing)
        {
            _crud.Dispose();
            _data.Dispose();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                _crud.AddDepartment(department);
                return RedirectToAction("ViewAllDepartments", "Display");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.Department.FindDepartmentByID((int)id);

            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                _crud.EditDepartment(department);
                return RedirectToAction("ViewAllDepartments", "Display");
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.Department.FindDepartmentByID((int)id);
            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDepartment(int id)
        {
            var department = _data.Department.FindDepartmentByID(id);
            _crud.DeleteDepartment(department);
            return RedirectToAction("ViewAllDepartments", "Display");
        }

        [Authorize(Roles = "Supervisor,Employee")]
        public ActionResult Chat()
        {
            var id = User.Identity.GetUserId();
            var user = _data.ApplicationUser.FindUserByID(id);
            var messages = user.Worker.Department.Messages.ToList();
            ViewBag.Name = user.Worker.FullName;
            return View(messages);
        }


    }
}