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

        public Project FindProjectById(int id) => _context.Projects.Include(s => s.WorkersInMe).SingleOrDefault(p => p.ID == id);

        public List<Project> AllProjects() => _context.Projects.Include(s => s.WorkersInMe).ToList();

        public List<Project> FindProjectsPerWorker(int id) => _context.Projects.Include(s => s.WorkersInMe).Where(p => p.WorkersInMe.FirstOrDefault(w => w.WorkerID == id) != null).ToList();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}