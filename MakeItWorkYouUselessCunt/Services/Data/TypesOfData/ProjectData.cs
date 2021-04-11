using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.Services.Data.TypesOfData
{
    public class ProjectData:IDisposable
    {
        private ApplicationDbContext _context;

        public ProjectData()
        {
            _context = new ApplicationDbContext();
        }


        /// <summary>
        /// Give an id and I will return the project that has this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project FindProjectById(int id) => _context.Projects.Include(s => s.WorkersInMe).SingleOrDefault(p => p.ID == id);



        /// <summary>
        /// Returns a list of all the projects in the database
        /// </summary>
        /// <returns></returns>
        public List<Project> AllProjects() => _context.Projects.Include(s => s.WorkersInMe).ToList();


        /// <summary>
        /// Give me the id of a worker and I will return a list of the projects assigned to this worker
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Project> FindProjectsPerWorker(int id) => _context.Projects.Include(s => s.WorkersInMe).Where(p => p.WorkersInMe.FirstOrDefault(w => w.WorkerID == id) != null).ToList();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}