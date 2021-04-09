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

        /// <summary>
        /// Give me ViewModel that was Filled in View And I will Create And Save the Project in Database
        /// </summary>
        /// <param name="f2"></param>
        /// <param name="supervisorID"></param>
        public void CreateProject(CreateProjectViewModel f2,string supervisorID)
        {
            
            var newProject = NewProject(f2.Project,f2.Attach);

            AddSupervisorToNewProject(supervisorID, newProject);

            AddEmployeesToNewProject(newProject, f2.Users);

            _db.Projects.Add(newProject);

            _db.SaveChanges();
        }

        /// <summary>
        /// Creates a new Project and fill the properties
        /// </summary>
        /// <param name="newProject"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add to new Project the Supervisor
        /// </summary>
        /// <param name="supervisorId"></param>
        /// <param name="newProject"></param>
        private void AddSupervisorToNewProject(string supervisorId,Project newProject)
        {
            var supervisorOfProject = _db.Users.Find(supervisorId).Worker;
            newProject.WorkersInMe.Add(new ProjectsAssignedToEmployee()
            {
                Project = newProject,
                Worker = supervisorOfProject
            });
        }

        /// <summary>
        /// Add Employees to new Project
        /// </summary>
        /// <param name="newProject"></param>
        /// <param name="employees"></param>
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

        /// <summary>
        /// Give me the ID of Project you want to Delete and I Will Delete It
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProject(int id)
        {
            var pro = _db.Projects.Find(id);

            DeleteProjectFiles(pro.Attachments);

            _db.Projects.Remove(pro);

            _db.SaveChanges();
        }
        #endregion

        #region Edit Project
        /// <summary>
        /// Give the View Model that was Edited in View And I will do the necessary things to Update The Project
        /// </summary>
        /// <param name="f2"></param>
        public void EditProject(EditProjectViewModel f2)
        {
            var projectToEdit = _db.Projects.Find(f2.Project.ID);
            CheckIfAttachmentChanged(f2.Attach, projectToEdit);
            CheckChangesInEmployeesOfProject(projectToEdit, f2.Users);
            UpdateProjectProperties(projectToEdit, f2.Project);
            _db.Entry(projectToEdit).State = EntityState.Modified;
            _db.SaveChanges();
        }

        /// <summary>
        /// Checks If it is a new Project Attachment and if it is a new attachment update the project attachment
        /// </summary>
        /// <param name="changedAttachment"></param>
        /// <param name="projectToEdit"></param>
        private void CheckIfAttachmentChanged(HttpPostedFileBase changedAttachment,Project projectToEdit)
        {
            if (!(changedAttachment is null))
            {
                DeleteProjectFiles(projectToEdit.Attachments);
                projectToEdit.Attachments = DeserializeAttach(changedAttachment);
            }
        }

        /// <summary>
        /// Check changes in employees in project and update
        /// </summary>
        /// <param name="projectToEdit"></param>
        /// <param name="employees"></param>
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

        /// <summary>
        /// Update project to edited properties
        /// </summary>
        /// <param name="projectToEdit"></param>
        /// <param name="editedOne"></param>
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

        /// <summary>
        /// Give me an uploaded File for Project Attachments I will save it to folder and return you the name of the file
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Give me a an Attachment name And I will search and delete it
        /// </summary>
        /// <param name="fileName"></param>
        private void DeleteProjectFiles(string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/");
            if (File.Exists(Path.Combine(path, fileName)))
            {
                File.Delete(Path.Combine(path, fileName));
            }
        }

        /// <summary>
        /// Send me a Project and I will change Finished property to True
        /// </summary>
        /// <param name="pro"></param>
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

        /// <summary>
        /// Give me a List of ApplicationUsers and I will filter it and return A list of DummyForProject full of employees
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="supervisorID"></param>
        /// <returns></returns>
        public List<DummyForProject> FillTheListOfDummies(List<ApplicationUser> employees,string supervisorID)
        {
            List<DummyForProject> f3 = new List<DummyForProject>();
            for (int i = 0; i < employees.Count; i++)
            {
                if (!(employees[i].Roles.SingleOrDefault(r => r.RoleId == supervisorID) == null))
                {
                    f3.Add(new DummyForProject()
                    {
                        ID = employees[i].Id,
                        Fullname = employees[i].Worker.FullName,
                        CV = employees[i].Worker.CV,
                        Pic = employees[i].Worker.Pic
                    });
                }
            }
            return f3;
        }

        /// <summary>
        /// Give me a List of ApplicationUsers and I will filter it and return A list of DummyForProject full of employees to send in EDIT MODE
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="supervisorID"></param>
        /// <returns></returns>
        public List<DummyForProject> FillTheListOfDummiesForEdit(List<ApplicationUser> employees, string roleId, Project projectToEdit)
        {
            List<DummyForProject> f3 = new List<DummyForProject>();
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Roles.SingleOrDefault(r => r.RoleId == roleId) != null)
                {
                    if (projectToEdit.WorkersInMe.FirstOrDefault(s => s.WorkerID == employees[i].Worker.ID) != null)
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = employees[i].Id,
                            Fullname = employees[i].Worker.FullName,
                            CV = employees[i].Worker.CV,
                            Pic = employees[i].Worker.Pic,
                            IsSelected = true
                        });
                    }
                    else
                    {
                        f3.Add(new DummyForProject()
                        {
                            ID = employees[i].Id,
                            Fullname = employees[i].Worker.FullName,
                            CV = employees[i].Worker.CV,
                            Pic = employees[i].Worker.Pic,
                            IsSelected = false
                        });
                    }
                }
            }
            return f3;
        }
        #endregion


    }
}