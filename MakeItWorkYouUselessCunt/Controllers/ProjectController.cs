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
            var employees = _data.UsersPerDepartment(_data.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);
            var roleId = _data.FindRoleByName("Employee").Id;
            List<DummyForProject> f3 = new List<DummyForProject>();
            for(int i=0;i<employees.Count;i++)
            {
                if (!(employees[i].Roles.SingleOrDefault(r=>r.RoleId==roleId)==null)) {
                    f3.Add(new DummyForProject()
                    {
                        ID = employees[i].Id,
                        Fullname = employees[i].Worker.FullName,
                        CV = employees[i].Worker.CV,
                        Pic = employees[i].Worker.Pic
                    });
                }
            }
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
            return RedirectToAction("ViewAllProjects","Display");
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
            var employees = _data.UsersPerDepartment(_data.FindUserByID(User.Identity.GetUserId()).Worker.DepartmentID);
            var roleId = _data.FindRoleByName("Employee").Id;
            List<DummyForProject> f3 = new List<DummyForProject>();
            for(int i=0;i<employees.Count;i++)
            {
                if (employees[i].Roles.SingleOrDefault(r => r.RoleId == roleId) == null)
                {
                    employees.Remove(employees[i]);
                }
                else
                {
                    if (pro.WorkersInMe.FirstOrDefault(s=>s.WorkerID== employees[i].Worker.ID)!=null)
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = employees[i].Id,
                            Fullname = employees[i].Worker.FullName,
                            CV = employees[i].Worker.CV,
                            Pic = employees[i].Worker.Pic,
                            IsSelected = true
                        });
                    }
                    else
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = employees[i].Id,
                            Fullname = employees[i].Worker.FullName,
                            CV = employees[i].Worker.CV,
                            Pic = employees[i].Worker.Pic,
                            IsSelected = false
                        });
                    }
                }
            }
            EditProjectViewModel f2 = new EditProjectViewModel()
            {
                Users =new List<DummyForProject>()
            };
            f2.Project = pro;
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

        public FileResult DownloadFile(string fileName)
        {
            string path = HttpContext.Server.MapPath("~/ProjectFiles/" + fileName);
            return File(path,"application/force-download",Path.GetFileName(path));
        }
    }
}