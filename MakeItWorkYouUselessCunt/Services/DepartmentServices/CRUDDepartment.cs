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

        /// <summary>
        /// Give me a Department type object and I will save it to Database
        /// </summary>
        public void AddDepartment(Department departmentToSave)
        {
            _context.Departments.Add(departmentToSave);
            _context.SaveChanges();
        }

        /// <summary>
        /// Give me a Department type object and I will edit it and save changes to Database
        /// </summary>
        public void EditDepartment(Department departmentToEdit)
        {
            _context.Entry(departmentToEdit).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Give me a Department type object and I will delete it
        /// </summary>
        public void DeleteDepartment(Department departmentToDelete)
        {
            var dep = _context.Departments.Find(departmentToDelete.ID);
            _context.Departments.Remove(dep);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}