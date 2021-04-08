using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.Services.WorkerServices;
using ManagementSystemVersionTwo.ViewModels;
using Microsoft.AspNet.Identity;

namespace ManagementSystemVersionTwo.Controllers
{
    public class WorkerController : Controller
    {
        ExternalServicesWorker _external;
        DataRepository _data;
        public WorkerController()
        {
            _external = new ExternalServicesWorker();
            _data = new DataRepository();
        }

        protected override void Dispose(bool disposing)
        {
            _external.Dispose();
            _data.Dispose();
        }

        public ActionResult CreateWorkerForApplicationUser(ApplicationUser user)
        {
            if (user is null || user.Roles.Count != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CreateWorker f2 = new CreateWorker()
            {
                AllDepartments = _data.Department.AllDepartments(),
                userID = user.Id,
                Roles = _data.Role.AllRoles(),
                DropDownDataForGender = new List<SelectListItem>() {
                                                new SelectListItem(){
            Text="Male",
            Value="Male"
            },
                                                new SelectListItem(){
            Text="Female",
            Value="Female"
            }
                }
            };
            return View(f2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWorkerForApplicationUser(CreateWorker f2)
        {
            if (ModelState.IsValid)
            {
                var dep = _data.Department.FindDepartmentByID(f2.IdOfDepartment);
                _external.CreateWorker(f2, dep, _data.Role.FindRoleByID(f2.SelectedRole).Name);
                return RedirectToAction("ViewAllWorkers", "Display");
            }
            else
            {
                f2 = new CreateWorker()
                {
                    AllDepartments = _data.Department.AllDepartments(),
                    userID = f2.userID,
                    Roles = _data.Role.AllRoles(),
                    DropDownDataForGender = new List<SelectListItem>() {
                                                new SelectListItem(){
            Text="Male",
            Value="Male"
            },
                                                new SelectListItem(){
            Text="Female",
            Value="Female"
            }
                }
                };
                return View(f2);
            }
        }
        public ActionResult EditWorker(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.ApplicationUser.FindUserByID(userID);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditWorker f2 = _external.FillEditWorkerViewModel(user);
            return View(f2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWorker(EditWorker f2)
        {
            if (ModelState.IsValid)
            {
                _external.EditWorker(f2);
                return RedirectToAction("ViewAllWorkers", "Display");
            }
            else
            {
                f2 = _external.FillEditWorkerViewModel(_data.ApplicationUser.FindUserByID(f2.UserID));
                return View(f2);
            }
        }

        public ActionResult DeleteWorker(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.ApplicationUser.FindUserByID(userID);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorkerConfirmed(ApplicationUser userToDelete)
        {
            if (string.IsNullOrEmpty(userToDelete.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.ApplicationUser.FindUserByID(userToDelete.Id);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _external.DeleteWorkersApplicationUser(userToDelete.Id);
            return RedirectToAction("ViewAllWorkers", "Display");
        }

        public ActionResult Calendar(int? id)
        {
            ViewBag.Role = User.IsInRole("Employee");
            if (ViewBag.Role)
            {
                var user = _data.ApplicationUser.FindUserByID(User.Identity.GetUserId());
                id = user.Worker.ID;
            }
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var worker = _data.Worker.FindWorkerByID((int)id);
            if (worker is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.WorkerID = worker.ID;
            return View();
        }

        public JsonResult GetProjects(int? workerid)
        {
            var worker= new Worker();
            if (!(workerid is null))
            {
                worker = _data.Worker.FindWorkerByID((int)workerid);
            }
            
            List<WorkingDays> projects = new List<WorkingDays>();
            if (!(worker is null))
            {
                var workersprojects = _data.Project.FindProjectsPerWorker(worker.ID);
                foreach (var p in workersprojects)
                {
                    projects.Add(new WorkingDays()
                    {
                        Start = p.StartDate.Date.ToString("yyyy-MM-dd"),
                        End = p.EndDate.Date.ToString("yyyy-MM-dd"),
                        Title = p.Title
                    });
                }

                return new JsonResult { Data = projects, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { };
        }

        [HttpPost]
        public void SaveWorkingDays(WorkingDays[] tosave,int workerid)
        {
            _external.SaveWorkingDays(tosave, workerid);
        }

        public JsonResult GetWorkDays(int? workerid)
        {
            var worker = new Worker();
            if (!(workerid is null))
            {
                worker = _data.Worker.FindWorkerByID((int)workerid);
            }
            var days = new List<WorkingDays>();
            if (!(worker is null))
            {
                foreach(var day in worker.Days)
                {
                    days.Add(new WorkingDays
                    {
                        Start=day.Start,
                        Title=day.Title,
                        Display=day.Display,
                        BackgroundColor=day.BackgroundColor,
                        ID=day.ID,
                        End=day.End
                    });
                }

                return new JsonResult { Data = days, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { };
        }

        [HttpPost]
        public void DeleteWorkDays(WorkingDays[] days,int workerid)
        {
            _external.DeleteWorkingDays(days, workerid);
        }
    }
}