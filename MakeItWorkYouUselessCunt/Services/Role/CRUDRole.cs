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

        public void CreateRole(IdentityRole role)
        {
            if (!_roleManager.RoleExists(role.Name)) {
                _roleManager.Create(role);
            }
            
        }

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