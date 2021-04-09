using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.Role
{
    public class CRUDRole :IDisposable
    {
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;

        public CRUDRole()
        {
            _context = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        /// <summary>
        /// Give me the Role you want to Create, I will check if exists and if not then I will Create it and store it
        /// </summary>
        /// <param name="role"></param>
        public void CreateRole(IdentityRole role)
        {
            if (!_roleManager.RoleExists(role.Name)) {
                _roleManager.Create(role);
            }
            
        }

        /// <summary>
        /// Give me the Role to Update
        /// </summary>
        /// <param name="role"></param>
        public void UpdateRole(IdentityRole role)
        {
            _roleManager.Update(role);
        }

        /// <summary>
        /// Give me the Role to Delete
        /// </summary>
        /// <param name="role"></param>
        public void DeleteRole(IdentityRole role)
        {
            _roleManager.Delete(role);
        }

        public void Dispose()
        {
            _context.Dispose();
            _roleManager.Dispose();
        }
    }
}