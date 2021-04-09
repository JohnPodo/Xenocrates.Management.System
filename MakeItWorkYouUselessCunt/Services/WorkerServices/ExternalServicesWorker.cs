using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        private RoleManager<IdentityRole> _roleManager;

        public ExternalServicesWorker()
        {
            _db = new ApplicationDbContext();
            _store = new UserStore<ApplicationUser>(_db);
            _manager = new UserManager<ApplicationUser>(_store);
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
        }
        #region Create Worker

        /// <summary>
        /// Give me the ViewModel From the View the Chosen Department and the role and I will connect Everything and save it in DB
        /// </summary>
        /// <param name="f2"></param>
        /// <param name="f2dep"></param>
        /// <param name="role"></param>
        public void CreateWorker(CreateWorker f2, Department f2dep, string role)
        {
            _manager.AddToRole(f2.userID, role);

            var newWorker = NewWorker(f2, f2dep);

            _db.Workers.Add(newWorker);

            _db.SaveChanges();
        }

        /// <summary>
        /// Make a new Worker
        /// </summary>
        /// <param name="newWorker"></param>
        /// <param name="departmentOfNewWorker"></param>
        /// <returns></returns>
        private Worker NewWorker(CreateWorker newWorker, Department departmentOfNewWorker)
        {
            Worker newWorkerToAdd = new Worker()
            {
                Address = newWorker.Address,
                Pic = ConventionsOfHttpPostedFileBase.ForPostedPicture(newWorker.ProfilePicture),
                CV = ConventionsOfHttpPostedFileBase.ForCV(newWorker.CV),
                ContractOfEmployment = ConventionsOfHttpPostedFileBase.ForContractOfEmployments(newWorker.ContractOfEmployment),
                FirstName = newWorker.FirstName,
                LastName = newWorker.LastName,
                DateOfBirth = newWorker.DateOfBirth,
                Gender = newWorker.Gender,
                BankAccount = newWorker.BankAccount,
                Salary = newWorker.Salary,
                Department = _db.Departments.Single(s => s.ID == departmentOfNewWorker.ID),
                DepartmentID = _db.Departments.Single(s => s.ID == departmentOfNewWorker.ID).ID,
                ApplicationUser = _db.Users.Find(newWorker.userID)
            };
            return newWorkerToAdd;
        }

        #endregion

        #region Delete Worker
        public void DeleteWorkersApplicationUser(string id)
        {
            var user = _db.Users.Find(id);

            var worker = _db.Workers.Find(user.Worker.ID);

            var payments = worker.Payments.ToList();

            var days = worker.Days.ToList();

            DeletePayments(payments);

            DeleteDays(days);
          
            DeleteProfPicOfWorker(worker.Pic);

            DeleteCVOfWorker(worker.CV);

            DeleteContractOfEmployementOfWorker(worker.ContractOfEmployment);

            _db.Workers.Remove(worker);

            _db.Users.Remove(user);

            _db.SaveChanges();
        }

        /// <summary>
        /// Delete all payments details of user 
        /// </summary>
        /// <param name="payments"></param>
        private void DeletePayments(List<PaymentDetails> payments)
        {
            for (int i = 0; i < payments.Count; i++)
            {
                var pay = _db.Payments.Find(payments[i].ID);
                _db.Payments.Remove(pay);
            }

        }

        /// <summary>
        /// Delete all working day details of user 
        /// </summary>
        /// <param name="payments"></param>
        private void DeleteDays(List<WorkingDays> days)
        {
            for (int i = 0; i < days.Count; i++)
            {
                var da = _db.CaldendarDays.Find(days[i].ID);
                _db.CaldendarDays.Remove(da);
            }


        }
        #endregion

        #region Edit Worker
        public void EditWorker(EditWorker editworker)
        {
            var workerInDb = _db.Users.Find(editworker.UserID);

            var role = _db.Roles.Find(editworker.SelectedRole);

            CheckAndUpdateEverything(workerInDb, editworker, role);

            UpdatePropertiesOfEditedWorker(workerInDb, editworker);

            EditWorkersApplicationUser(workerInDb.Id);

            _db.Entry(workerInDb.Worker).State = EntityState.Modified;

            _db.SaveChanges();
        }

        /// <summary>
        /// Update ApplicationUser
        /// </summary>
        /// <param name="id"></param>
        private void EditWorkersApplicationUser(string id)
        {
            var user = _db.Users.Find(id);
            _manager.Update(user);
        }

        /// <summary>
        /// Update the Properties that needs to be updated
        /// </summary>
        /// <param name="workerInDb"></param>
        /// <param name="editworker"></param>
        private void UpdatePropertiesOfEditedWorker(ApplicationUser workerInDb, EditWorker editworker)
        {
            workerInDb.UserName = editworker.Username;
            workerInDb.Email = editworker.Email;
            workerInDb.Worker.FirstName = editworker.FirstName;
            workerInDb.Worker.LastName = editworker.LastName;
            workerInDb.Worker.DateOfBirth = editworker.DateOfBirth;
            workerInDb.Worker.Gender = editworker.Gender;
            workerInDb.Worker.Address = editworker.Address;
            workerInDb.Worker.BankAccount = editworker.BankAccount;
            workerInDb.Worker.Salary = editworker.Salary;
        }

        /// <summary>
        /// I will check if attachments and role changed and update them 
        /// </summary>
        /// <param name="workerInDb"></param>
        /// <param name="editworker"></param>
        /// <param name="role"></param>
        private void CheckAndUpdateEverything(ApplicationUser workerInDb, EditWorker editworker, IdentityRole role)
        {
            if (!workerInDb.Roles.Any(r => r.RoleId == role.Id))
            {
                workerInDb.Roles.Remove(workerInDb.Roles.First());
                workerInDb.Roles.Add(new IdentityUserRole()
                {
                    RoleId = role.Id,
                    UserId = workerInDb.Id
                });
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
        }
        #endregion

        #region Usefull Methods

        /// <summary>
        /// Give me the name of file and I will find and delete it
        /// </summary>
        /// <param name="fileName"></param>
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

        /// <summary>
        /// Give me the name of file and I will find and delete it
        /// </summary>
        /// <param name="fileName"></param>
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

        /// <summary>
        /// Give me the name of file and I will find and delete it
        /// </summary>
        /// <param name="fileName"></param>
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

        /// <summary>
        /// Fill ViewModel of EditWorker of Edit
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
                Roles = _db.Roles.Where(s => s.Name != "Admin").ToList()
            };
            return f2;
        }

        /// <summary>
        /// Give me an Array of WorkingDays and the Id of the Worker That you want to save the days in
        /// </summary>
        /// <param name="tosave"></param>
        /// <param name="id"></param>
        public void SaveWorkingDays(WorkingDays[] tosave, int id)
        {
            if (tosave.Length > 0 && tosave != null)
            {
                foreach (var day in tosave)
                {
                    WorkingDays saveme = new WorkingDays()
                    {
                        Start = day.Start,
                        Title = day.Title,
                        BackgroundColor = day.BackgroundColor,
                        Display = day.Display
                    };
                    var worker = _db.Workers.Find(id);
                    saveme.Worker = worker;
                    if (worker.Days.SingleOrDefault(s => s.Start == saveme.Start) is null)
                    {
                        _db.CaldendarDays.Add(saveme);
                        _db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Give me an array of Working days and the id Of the Worker and I will find the days and delete them
        /// </summary>
        /// <param name="tosave"></param>
        /// <param name="id"></param>
        public void DeleteWorkingDays(WorkingDays[] tosave, int id)
        {
            foreach (var item in tosave)
            {
                var day = _db.CaldendarDays.SingleOrDefault(d => d.Start == item.Start && d.Worker.ID == id);
                if (!(day is null))
                {
                    _db.CaldendarDays.Remove(day);
                    _db.SaveChanges();
                }

            }
        }

        /// <summary>
        /// I return List of SelectListItem with Gender Options For DropDown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GenderOptionsInSelectListItem()
        {
            var genders = new List<SelectListItem>() {
                                                new SelectListItem(){
            Text="Male",
            Value="Male"
            },
                                                new SelectListItem(){
            Text="Female",
            Value="Female"
            }
                };
            return genders;
        }
        #endregion

        public void Dispose()
        {
            _db.Dispose();
            _store.Dispose();
            _manager.Dispose();
            _roleManager.Dispose();
        }
    }
}
