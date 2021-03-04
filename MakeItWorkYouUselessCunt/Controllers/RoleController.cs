using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.Services.Role;
using ManagementSystemVersionTwo.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Controllers
{
    public class RoleController : Controller
    {
        private CRUDRole _crud;
        private DataRepository _data;
        private ExternalServices _external;

        public RoleController()
        {
            _crud=new CRUDRole();
            _data=new DataRepository();
            _external = new ExternalServices();
        }

        protected override void Dispose(bool disposing)
        {
            _crud.Dispose();
            _data.Dispose();
            _external.Dispose();
        }

        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(IdentityRole role)
        {
            _crud.CreateRole(role);
            return RedirectToAction("ViewAllRoles","Display");
        }

        public ActionResult DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _data.FindRoleByID(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRole(IdentityRole role)
        {
            _crud.DeleteRole(role);
            return RedirectToAction("ViewAllRoles", "Display");
        }

        public ActionResult AddRoleToApplicationUser(ApplicationUser user)
        {
            
            if (string.IsNullOrEmpty(user.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Elenxei an o User pou dexetai exei eidi rolo An exei akyrwnei tin diadikasia
            if (user.Roles.Count != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddRoleToUser f2 = new AddRoleToUser()
            {
                UserID = user.Id,
                Roles= _data.AllRoles()
            };
            return View(f2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToApplicationUser(AddRoleToUser f2)
        {
            _external.AddRoleToUser(f2.UserID, _data.FindRoleByID(f2.SelectedRole).Name);
            return RedirectToAction("CreateWorkerForApplicationUser", "Worker",_data.FindUserByID(f2.UserID));
        }
    }
}