using System;
using System.Collections.Generic;
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



        public void Dispose()
        {
            _db.Dispose();
            _store.Dispose();
            _manager.Dispose();
        }
    }
}