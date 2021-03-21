using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.ViewModels;

namespace ManagementSystemVersionTwo.Services.ProjectServices
{
    public class ExternalProjectService : IDisposable
    {
        private ApplicationDbContext _db;

        public ExternalProjectService()
        {
            _db = new ApplicationDbContext();
        }
        public void Dispose()
        {

            _db.Dispose();
        }

        public void CreateProject(CreateProjectViewModel f2,string SupervisorID)
        {
            Project tosave = new Project();
            tosave = f2.Project;

            tosave.WorkersInMe.Add(new ProjectsAssignedToEmployee() {
                ProjectID=tosave.ID,
                WorkerID=_db.Users.Find(SupervisorID).Worker.ID
            });
            foreach(var emp in f2.Users)
            {
                if (emp.IsSelected)
                {
                    tosave.WorkersInMe.Add(new ProjectsAssignedToEmployee()
                    {
                        ProjectID = tosave.ID,
                        WorkerID = _db.Users.Find(emp.ID).Worker.ID
                    });
                }
            }
            _db.SaveChanges();
        }

        public void DeleteProject(int id)
        {
            var pro = _db.Projects.Find(id);
            _db.Projects.Remove(pro);
            _db.SaveChanges();
        }

        public void EditProject(CreateProjectViewModel f2)
        {
            var pro = _db.Projects.Find(f2.Project.ID);
            foreach(var workers in f2.Users)
            {
                if (pro.WorkersInMe.SingleOrDefault(w => w.WorkerID == _db.Users.Find(workers.ID).Worker.ID) != null && workers.IsSelected == false)
                {
                    pro.WorkersInMe.Remove(_db.ProjectsToEmployees.Find(workers.ID));
                }
                if (pro.WorkersInMe.SingleOrDefault(w => w.WorkerID == _db.Users.Find(workers.ID).Worker.ID) == null && workers.IsSelected == true)
                {
                    pro.WorkersInMe.Add(new ProjectsAssignedToEmployee() {
                        Project=pro,
                        Worker = _db.Users.Find(workers.ID).Worker
                    });
                }
                pro.Description = f2.Project.Description;
                pro.Title = f2.Project.Title;
                pro.StartDate = f2.Project.StartDate;
                pro.EndDate = f2.Project.EndDate;
                _db.Entry(pro).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }
    }
}