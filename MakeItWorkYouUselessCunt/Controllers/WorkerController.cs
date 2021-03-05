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
            //if (user == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //} Otan Teleiwsei to Front End na to Vgalw apo ta sxolia
            //Elenxei an o User pou dexetai exei eidi rolo An exei akyrwnei tin diadikasia
            if (user.Roles.Count != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreateWorker f2 = new CreateWorker()
            {
                AllDepartments = _data.AllDepartments(),
                userID = user.Id,
                Roles = _data.AllRoles()
            };
            return View(f2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWorkerForApplicationUser(CreateWorker f2)
        {
            if (ModelState.IsValid)
            {
                var dep = _data.FindDepartmentByID(f2.IdOfDepartment);
                _external.CreateWorker(f2, dep, _data.FindRoleByID(f2.SelectedRole).Name);
                return RedirectToAction("ViewAllWorkers", "Display");
            }
            else
            {
                f2 = new CreateWorker()
                {
                    AllDepartments = _data.AllDepartments(),
                    userID = f2.userID
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
            var user = _data.FindUserByID(userID);
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
                f2 = _external.FillEditWorkerViewModel(_data.FindUserByID(f2.UserID));
                return View(f2);
            }
        }

        public ActionResult DeleteWorker(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.FindUserByID(userID);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(_data.FindUserByID(user.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorkerConfirmed(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _data.FindUserByID(userID);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _external.DeleteWorkersApplicationUser(userID);
            return RedirectToAction("ViewAllWorkers", "Display");
        }
    }
}