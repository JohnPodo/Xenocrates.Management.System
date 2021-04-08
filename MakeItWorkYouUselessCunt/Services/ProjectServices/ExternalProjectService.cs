using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        #region CreateProject

        public void CreateProject(CreateProjectViewModel f2,string supervisorID)
        {
            
            var newProject = NewProject(f2.Project,f2.Attach);
            AddSupervisorToNewProject(supervisorID, newProject);
            AddEmployeesToNewProject(newProject, f2.Users);
            _db.Projects.Add(newProject);
            _db.SaveChanges();
        }

        private Project NewProject(Project newProject,HttpPostedFileBase attachment)
        {
            Project projectToSave = new Project()
            {
                Title = newProject.Title,
                Description = newProject.Description,
                StartDate = newProject.StartDate,
                EndDate = newProject.EndDate,
                Finished = false,
                Attachments = DeserializeAttach(attachment),
                WorkersInMe = new List<ProjectsAssignedToEmployee>()
            };
            return projectToSave;
        }

        private void AddSupervisorToNewProject(string supervisorId,Project newProject)
        {
            var supervisorOfProject = _db.Users.Find(supervisorId).Worker;
            newProject.WorkersInMe.Add(new ProjectsAssignedToEmployee()
            {
                Project = newProject,
                Worker = supervisorOfProject
            });
        }

        private void AddEmployeesToNewProject(Project newProject,List<DummyForProject> employees)
        {
            var workerOfProject = new Worker();
            foreach (var emp in employees)
            {
                if (emp.IsSelected)
                {
                    workerOfProject = _db.Users.Find(emp.ID).Worker;
                    newProject.WorkersInMe.Add(new ProjectsAssignedToEmployee()
                    {
                        Project = newProject,
                        Worker = workerOfProject
                    });
                }
            }
        }

        #endregion

        #region DeleteProject
        public void DeleteProject(int id)
        {
            var pro = _db.Projects.Find(id);
            DeleteProjectFiles(pro.Attachments);
            _db.Projects.Remove(pro);
            _db.SaveChanges();
        }
        #endregion

        #region Edit Project
        public void EditProject(EditProjectViewModel f2)
        {
            var projectToEdit = _db.Projects.Find(f2.Project.ID);
            CheckIfAttachmentChanged(f2.Attach, projectToEdit);
            CheckChangesInEmployeesOfProject(projectToEdit, f2.Users);
            UpdateProjectProperties(projectToEdit, f2.Project);
            _db.Entry(projectToEdit).State = EntityState.Modified;
            _db.SaveChanges();
        }

        private void CheckIfAttachmentChanged(HttpPostedFileBase changedAttachment,Project projectToEdit)
        {
            if (!(changedAttachment is null))
            {
                DeleteProjectFiles(projectToEdit.Attachments);
                projectToEdit.Attachments = DeserializeAttach(changedAttachment);
            }
        }

        private void CheckChangesInEmployeesOfProject(Project projectToEdit,List<DummyForProject> employees)
        {
            foreach (var workers in employees)
            {
                var worker = _db.Workers.SingleOrDefault(s => s.ApplicationUser.Id == workers.ID);
                if (projectToEdit.WorkersInMe.SingleOrDefault(w => w.WorkerID == worker.ID) != null && workers.IsSelected == false)
                {
                    var projectToDelete = worker.MyProjects.SingleOrDefault(s => s.ProjectID == projectToEdit.ID);
                    _db.ProjectsToEmployees.Remove(projectToDelete);
                }
                if (projectToEdit.WorkersInMe.SingleOrDefault(w => w.WorkerID == _db.Users.Find(workers.ID).Worker.ID) == null && workers.IsSelected == true)
                {
                    projectToEdit.WorkersInMe.Add(new ProjectsAssignedToEmployee()
                    {
                        Project = projectToEdit,
                        Worker = worker
                    });
                }
            }
        }

        private void UpdateProjectProperties(Project projectToEdit,Project editedOne)
        {
            projectToEdit.Description = editedOne.Description;
            projectToEdit.Title = editedOne.Title;
            projectToEdit.StartDate = editedOne.StartDate;
            projectToEdit.EndDate = editedOne.EndDate;
            projectToEdit.Finished = editedOne.Finished;
        }
        #endregion

        #region Usefull Methods
        private string DeserializeAttach(HttpPostedFileBase File)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (File != null)
            {
                string fileName = Path.GetFileName(File.FileName);
                File.SaveAs(path+fileName);
                return fileName;
            }
            return null;
        }

        private void DeleteProjectFiles(string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/");
            if (File.Exists(Path.Combine(path, fileName)))
            {
                File.Delete(Path.Combine(path, fileName));
            }
        }

        public void FinalizeProject(Project pro)
        {
            var projectInDb = _db.Projects.Find(pro.ID);
            if (!projectInDb.Finished)
            {
                projectInDb.Finished = true;
                _db.Entry(projectInDb).State = EntityState.Modified;
                _db.SaveChanges();
            }

        }
        #endregion

    }
}