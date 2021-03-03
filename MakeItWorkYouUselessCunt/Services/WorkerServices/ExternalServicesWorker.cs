using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.ViewModels;

namespace ManagementSystemVersionTwo.Services.WorkerServices
{
    public class ExternalServicesWorker:IDisposable
    {
        private ApplicationDbContext _db;

        public ExternalServicesWorker()
        {
            _db = new ApplicationDbContext();
        }

        public void CreateWorker(CreateWorker f2,Department f2dep)
        {
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
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}