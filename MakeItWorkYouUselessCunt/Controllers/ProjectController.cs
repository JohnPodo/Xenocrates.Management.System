using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.Services.ProjectServices;
using ManagementSystemVersionTwo.ViewModels;
using Microsoft.AspNet.Identity;

namespace ManagementSystemVersionTwo.Controllers
{
    public class ProjectController : Controller
    {
        DataRepository _data;
        ExternalProjectService _external;

        public ProjectController()
        {
            _data=new DataRepository();
            _external = new ExternalProjectService();
        }
        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
            _external.Dispose();
        }
        
        public ActionResult CreateProject()
        {
            var employees = _data.ApplicationUser.UsersPerDepartment(_data.ApplicationUser.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);

            var roleId = _data.Role.FindRoleByName("Employee").Id;

            var f3 = _external.FillTheListOfDummies(employees, roleId);
           
            CreateProjectViewModel f2 = new CreateProjectViewModel()
            {
                Users=f3
            };

            return View(f2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(CreateProjectViewModel f2)
        {
            if (ModelState.IsValid&&f2.Users.Count!=0)
            {
                _external.CreateProject(f2,User.Identity.GetUserId());

                return RedirectToAction("ViewAllProjects", "Display");
            }
            return RedirectToAction("CreateProject");
        }

        public ActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pro = _data.Project.FindProjectById((int)id);
            if (pro == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(int id)
        {
            _external.DeleteProject(id);

            return RedirectToAction("ViewAllProjects","Display");
        }

        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var projectToEdit = _data.Project.FindProjectById((int)id);

            if (projectToEdit == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employees = _data.ApplicationUser.UsersPerDepartment(_data.ApplicationUser.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);
            
            var roleId = _data.Role.FindRoleByName("Employee").Id;
            
            var f3 = _external.FillTheListOfDummiesForEdit(employees, roleId, projectToEdit);
           
            EditProjectViewModel f2 = new EditProjectViewModel()
            {
                Users =new List<DummyForProject>()
            };

            f2.Project = projectToEdit;

            f2.Users = f3;

            return View(f2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(EditProjectViewModel f2)
        {
            if (ModelState.IsValid && f2.Users.Count != 0)
            {
                _external.EditProject(f2);

                return RedirectToAction("ViewAllProjects", "Display");
            }
            return View(f2);
        }

        public ActionResult FinalizeProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var projectToFinalize = _data.Project.FindProjectById((int)id);

            if (projectToFinalize is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _external.FinalizeProject(projectToFinalize);

            return RedirectToAction("ViewAllProjects", "Display");
        }

        public FileResult DownloadFile(string fileName)
        {
            string path = HttpContext.Server.MapPath("~/ProjectFiles/" + fileName);

            return File(path,"application/force-download",Path.GetFileName(path));
        }
    }
}