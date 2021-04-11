using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using System.Data.Entity;

namespace ManagementSystemVersionTwo.Services.Data.TypesOfData
{
    public class DepartmentData:IDisposable
    {
        private ApplicationDbContext _context;

        public DepartmentData()
        {
            _context = new ApplicationDbContext();
        }


        /// <summary>
        /// Returns a list of all the departments in database
        /// </summary>
        /// <returns></returns>
        public List<Department> AllDepartments() => _context.Departments.Include(s => s.WorkersInThisDepartment).ToList();


        /// <summary>
        /// Give me an id of a department and I will return the department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Department FindDepartmentByID(int id) => _context.Departments.Include(s => s.WorkersInThisDepartment).SingleOrDefault(s => s.ID == id);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}