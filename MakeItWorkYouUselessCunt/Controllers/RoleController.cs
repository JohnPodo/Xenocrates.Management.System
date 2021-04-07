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

        public RoleController()
        {
            _crud=new CRUDRole();
            _data=new DataRepository();
        }

        protected override void Dispose(bool disposing)
        {
            _crud.Dispose();
            _data.Dispose();
        }

        [Authorize(Roles = "None")]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "None")]
        public ActionResult CreateRole(IdentityRole role)
        {

            _crud.CreateRole(role);
            return RedirectToAction("ViewAllRoles","Display");
        }

        [Authorize(Roles = "None")]
        public ActionResult DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _data.Role.FindRoleByID(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "None")]
        public ActionResult DeleteRole(IdentityRole role)
        {
            _crud.DeleteRole(role);
            return RedirectToAction("ViewAllRoles", "Display");
        }
    }
}