using System;
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
            var employees = _data.WorkersPerDepartment(_data.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);
            var roleId = _data.FindRoleByName("Employee").Id;
            List<DummyForProject> f3 = new List<DummyForProject>();
            foreach(var worker in employees)
            {
                if (worker.Roles.SingleOrDefault(r=>r.RoleId==roleId)==null) {
                    employees.Remove(worker);
                }
                else
                {
                    f3.Add(new DummyForProject()
                    {
                        ID = worker.Id,
                        Fullname = worker.Worker.FullName,
                        CV = worker.Worker.CV
                    });
                }
            }
            CreateProjectViewModel f2 = new CreateProjectViewModel()
            {
                Project=new Project(),
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
            var pro = _data.FindProjectById((int)id);
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
            return View();
        }

        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pro = _data.FindProjectById((int)id);
            if (pro == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employees = _data.WorkersPerDepartment(_data.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);
            var roleId = _data.FindRoleByName("Employee").Id;
            List<DummyForProject> f3 = new List<DummyForProject>();
            foreach (var worker in employees)
            {
                if (worker.Roles.SingleOrDefault(r => r.RoleId == roleId) == null)
                {
                    employees.Remove(worker);
                }
                else
                {
                    if (worker.Worker.MyProjects.SingleOrDefault(p => p.ID == pro.ID) != null)
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = worker.Id,
                            Fullname = worker.Worker.FullName,
                            CV = worker.Worker.CV,
                            IsSelected = true
                        });
                    }
                    else
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = worker.Id,
                            Fullname = worker.Worker.FullName,
                            CV = worker.Worker.CV,
                            IsSelected = false
                        });
                    }
                }
            }
            CreateProjectViewModel f2 = new CreateProjectViewModel()
            {
                Project=pro,
                Users = f3
            };
            return View(f3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(CreateProjectViewModel f2)
        {
            if (ModelState.IsValid)
            {
                _external.EditProject(f2);
            }
            return View();
        }
    }
}