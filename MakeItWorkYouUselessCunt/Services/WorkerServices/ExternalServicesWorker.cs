using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.WorkerServices
{
    public class ExternalServicesWorker : IDisposable
    {
        private ApplicationDbContext _db;
        private UserStore<ApplicationUser> _store;
        private UserManager<ApplicationUser> _manager;

        public ExternalServicesWorker()
        {
            _db = new ApplicationDbContext();
            _store = new UserStore<ApplicationUser>(_db);
            _manager = new UserManager<ApplicationUser>(_store);
        }

        public void CreateWorker(CreateWorker f2, Department f2dep, string role)
        {
            _manager.AddToRole(f2.userID, role);
            var worker = new Worker()
            {
                Address = f2.Address,
                Pic = ConventionsOfHttpPostedFileBase.ForPostedPicture(f2.ProfilePicture),
                CV = ConventionsOfHttpPostedFileBase.ForCV(f2.CV),
                ContractOfEmployment = ConventionsOfHttpPostedFileBase.ForContractOfEmployments(f2.ContractOfEmployment),
                FirstName = f2.FirstName,
                LastName = f2.LastName,
                DateOfBirth = f2.DateOfBirth,
                Gender = f2.Gender,
                BankAccount = f2.BankAccount,
                Salary = f2.Salary,
                Department = _db.Departments.Single(s => s.ID == f2dep.ID),
                DepartmentID = _db.Departments.Single(s => s.ID == f2dep.ID).ID,
                ApplicationUser = _db.Users.Find(f2.userID)

            };
            _db.Workers.Add(worker);
            _db.SaveChanges();
        }

        public void DeleteProfPicOfWorker(string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProfPics/");
            // Check if file exists with its full path    
            if (File.Exists(Path.Combine(path, fileName)))
            {
                // If file found, delete it    
                File.Delete(Path.Combine(path, fileName));
            }
        }

        public void DeleteCVOfWorker(string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/CVs/");
            // Check if file exists with its full path    
            if (File.Exists(Path.Combine(path, fileName)))
            {
                // If file found, delete it    
                File.Delete(Path.Combine(path, fileName));
            }
        }

        public void DeleteContractOfEmployementOfWorker(string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ContractOfEmployments/");
            // Check if file exists with its full path    
            if (File.Exists(Path.Combine(path, fileName)))
            {
                // If file found, delete it    
                File.Delete(Path.Combine(path, fileName));
            }
        }

        public void DeleteWorkersApplicationUser(string id)
        {
            var user = _db.Users.Find(id);
            DeleteProfPicOfWorker(user.Worker.Pic);
            DeleteCVOfWorker(user.Worker.CV);
            DeleteContractOfEmployementOfWorker(user.Worker.ContractOfEmployment);
            _db.Workers.Remove(user.Worker);
            _manager.Delete(user);
            _db.SaveChanges();
        }
        public void EditWorkersApplicationUser(string id)
        {
            var user = _db.Users.Find(id);
            _manager.Update(user);
        }

        public EditWorker FillEditWorkerViewModel(ApplicationUser user)
        {
            EditWorker f2 = new EditWorker()
            {
                UserID = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.Worker.FirstName,
                LastName = user.Worker.LastName,
                DateOfBirth = user.Worker.DateOfBirth,
                Gender = user.Worker.Gender,
                Address = user.Worker.Address,
                BankAccount = user.Worker.BankAccount,
                Salary = user.Worker.Salary,
                IdOfDepartment = user.Worker.DepartmentID,
                SelectedRole = _db.Roles.Find(user.Roles.First().RoleId).Name,
                AllDepartments = _db.Departments.ToList(),
                Roles = _db.Roles.ToList()
            };
            return f2;
        }

        public void EditWorker(EditWorker editworker)
        {
            var workerInDb = _db.Users.Find(editworker.UserID);
            if (workerInDb.Roles.FirstOrDefault(r => r.RoleId == editworker.SelectedRole) != null)
            {
                workerInDb.Roles.First().RoleId = editworker.SelectedRole;
            }
            if (workerInDb.Worker.Department.ID != editworker.IdOfDepartment)
            {
                workerInDb.Worker.Department = _db.Departments.Find(editworker.IdOfDepartment);
                workerInDb.Worker.DepartmentID = editworker.IdOfDepartment;
            }
            if (editworker.ProfilePicture != null)
            {
                DeleteProfPicOfWorker(workerInDb.Worker.Pic);
                workerInDb.Worker.Pic = ConventionsOfHttpPostedFileBase.ForPostedPicture(editworker.ProfilePicture);
            }
            if (editworker.CV != null)
            {
                DeleteCVOfWorker(workerInDb.Worker.CV);
                workerInDb.Worker.Pic = ConventionsOfHttpPostedFileBase.ForCV(editworker.CV);
            }
            if (editworker.ContractOfEmployment != null)
            {
                DeleteContractOfEmployementOfWorker(workerInDb.Worker.ContractOfEmployment);
                workerInDb.Worker.Pic = ConventionsOfHttpPostedFileBase.ForContractOfEmployments(editworker.ContractOfEmployment);
            }
            workerInDb.UserName = editworker.Username;
            workerInDb.Email = editworker.Email;
            workerInDb.Worker.FirstName = editworker.FirstName;
            workerInDb.Worker.LastName = editworker.LastName;
            workerInDb.Worker.DateOfBirth = editworker.DateOfBirth;
            workerInDb.Worker.Gender = editworker.Gender;
            workerInDb.Worker.Address = editworker.Address;
            workerInDb.Worker.BankAccount = editworker.BankAccount;
            workerInDb.Worker.Salary = editworker.Salary;
            EditWorkersApplicationUser(workerInDb.Id);
            _db.Entry(workerInDb.Worker).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void FinalizeProject(int id) => _db.Projects.SingleOrDefault(p => p.ID == id).Finished = true;

        public void Dispose()
        {
            _db.Dispose();
            _store.Dispose();
            _manager.Dispose();
        }
    }
}
