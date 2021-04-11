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
            DeleteMessagesOfDepartment(dep.Messages.ToList());
            _context.Departments.Remove(dep);
            _context.SaveChanges();
        }
        /// <summary>
        /// Delete the chat messages of the department
        /// </summary>
        private void DeleteMessagesOfDepartment(List<Message> messages)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                var message = _context.Messages.Find(messages[i].ID);
                _context.Messages.Remove(message);
            }
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}