using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Mvc;
using ManagementSystemVersionTwo.StatisticsModels;
using ManagementSystemVersionTwo.Services.Data.TypesOfData;

namespace ManagementSystemVersionTwo.Services.Data
{
    public class DataRepository : IDisposable
    {
        public ApplicationUserData ApplicationUser;
        public DepartmentData Department;
        public RoleData Role;
        public WorkerData Worker;
        public ProjectData Project;

        public DataRepository()
        {
            ApplicationUser = new ApplicationUserData();
            Department = new DepartmentData();
            Role = new RoleData();
            Worker = new WorkerData();
            Project = new ProjectData();
        }

        public void Dispose()
        {
            ApplicationUser.Dispose();
            Department.Dispose();
            Role.Dispose();
            Worker.Dispose();
            Project.Dispose();
        }
    }
}