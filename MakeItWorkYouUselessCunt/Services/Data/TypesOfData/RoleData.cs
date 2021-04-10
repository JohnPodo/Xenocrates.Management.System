using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.Data.TypesOfData
{
    public class RoleData:IDisposable
    {
        private ApplicationDbContext _context;

        public RoleData()
        {
            _context = new ApplicationDbContext();
        }


        /// <summary>
        /// Give me the name of the worker and I will return their role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IdentityRole FindRoleByName(string name) => _context.Roles.Include(r => r.Users).Single(s => s.Name == name);


        /// <summary>
        /// Returns a list of all the roles in the database
        /// </summary>
        /// <returns></returns>
        public List<IdentityRole> AllRoles() => _context.Roles.Include(r => r.Users).Where(s => s.Name != "Admin").ToList();


        /// <summary>
        /// Give me the id of the worker and I will return their role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IdentityRole FindRoleByID(string id) => _context.Roles.Include(r => r.Users).Single(s => s.Id == id);


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}