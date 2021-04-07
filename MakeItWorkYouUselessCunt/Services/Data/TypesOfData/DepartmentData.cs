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

        public List<Department> AllDepartments() => _context.Departments.Include(s => s.WorkersInThisDepartment).ToList();

        public Department FindDepartmentByID(int id) => _context.Departments.Include(s => s.WorkersInThisDepartment).SingleOrDefault(s => s.ID == id);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}