using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.Services.DepartmentServices
{
    public class CRUDDepartment
    {
        private ApplicationDbContext _context;

        public CRUDDepartment()
        {
            _context = new ApplicationDbContext();
        }

        public void AddDepartment(Department x)
        {
            _context.Departments.Add(x);
            _context.SaveChanges();
        }

        public void EditDepartment(Department x)
        {
            _context.Entry(x).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteDepartment(Department x)
        {
            var dep = _context.Departments.Find(x.ID); //Mprabo re file , prepei na ths to broume :)
            _context.Departments.Remove(dep);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}