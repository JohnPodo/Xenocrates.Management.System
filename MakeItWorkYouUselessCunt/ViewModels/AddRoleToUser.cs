using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.ViewModels
{
    public class AddRoleToUser
    {
        public string UserID { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public string SelectedRole { get; set; }
    }
}