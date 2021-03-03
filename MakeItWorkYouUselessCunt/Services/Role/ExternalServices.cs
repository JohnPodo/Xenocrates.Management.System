using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.Role
{
    public class ExternalServices:IDisposable
    {
        private UserStore<ApplicationUser> _store;
        private UserManager<ApplicationUser> _manager;

        public ExternalServices()
        {
            _store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            _manager = new UserManager<ApplicationUser>(_store);
        }

        public void AddRoleToUser(string user,string role)
        {
            _manager.AddToRole(user, role);
        }

        public void Dispose()
        {
            _store.Dispose();
            _manager.Dispose();
        }
    }
}