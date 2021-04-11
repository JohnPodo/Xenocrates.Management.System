using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using System.Data.Entity;

namespace ManagementSystemVersionTwo.Services.Data.TypesOfData
{
    public class WorkerData :IDisposable
    {
        private ApplicationDbContext _context;

        public WorkerData()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Give me the id and I will return the worker with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Worker FindWorkerByID(int id) => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(w => w.MyProjects).Include(w => w.Payments).SingleOrDefault(w => w.ID == id);


        /// <summary>
        /// Returns a list of all the workers in the database
        /// </summary>
        /// <returns></returns>
        public List<Worker> AllWorkers() => _context.Workers.Include(w => w.ApplicationUser).Include(w => w.Department).Include(p => p.MyProjects).Include(u => u.Payments).ToList();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}