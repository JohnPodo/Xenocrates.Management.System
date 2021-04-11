using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.Filtering
{
    public static class FilteringServices
    {
        /// <summary>
        /// Give a list of Workers and a substring to search and I will Filter the list and return it
        /// </summary>
        /// <param name="search"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Worker> FilterWorkerByName(string search, List<Worker> data) => data.Where(w => (w.FirstName + " " + w.LastName).Contains(search)).ToList();

        /// <summary>
        /// Give a list of Workers and a role to search and I will Filter the list and return it
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Worker> FilterWorkersInRole(string roleID, List<Worker> data) => data.Where(r => r.ApplicationUser.Roles.SingleOrDefault(w => w.RoleId == roleID) != null).ToList();

        /// <summary>
        /// Give a list of Workers and a DepartmentID to search and I will Filter the list and return it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Worker> FilterWorkersPerDepartment(int id, List<Worker> data) => data.Where(u => u.DepartmentID == id).ToList();

        /// <summary>
        /// Give a list of Departments and a City to search and I will Filter the list and return it
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="departments"></param>
        /// <returns></returns>
        public static List<Department> FilterDepartmentsByCity(string searchString, List<Department> departments) => departments.Where(x => x.City.Contains(searchString)).ToList();

        /// <summary>
        /// Give a list of Projects and a substring to search and I will Filter the list and return it
        /// </summary>
        /// <param name="search"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Project> FilterProjectByTitle(string search, List<Project> data) => data.Where(w => (w.Title).Contains(search)).ToList();

        /// <summary>
        /// Give a list of Projects and a status to check and I will Filter the list and return it
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Project> StatusProject(string status, List<Project> data)
        {
            switch (status)
            {
                case "Finished":
                    return data.Where(w => (w.Finished) == true).ToList();
                case "Not Finished":
                    return data.Where(w => (w.Finished) == false).ToList();
                default:
                    return data;
            }
        }


        /// <summary>
        /// Give a list of Paid or Not Paid Workers
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Worker> PaymentStatus(string status, List<Worker> data)
        {
            List<Worker> worker = new List<Worker>();
            if (status == "Paid")
            {
                foreach (var item in data)
                {
                    var count =  item.Payments.Where(d => d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).Count();
                    if(count != 0)
                    {
                        worker.Add(item);
                    }
                }
            }
            else if (status == "Not Paid")
            {
                foreach (var item in data)
                {
                    var count = item.Payments.Where(d => d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).Count();
                    if (count == 0)
                    {
                        worker.Add(item);
                    }
                }
            }
            else
            {
                worker = data;
            }
            return worker;
            
        }

        /// <summary>
        /// Give a list of Projects and a Department to check and I will Filter the list and return it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Project> FilterProjectsPerDepartment(int id, List<Project> data) => data.Where(u => u.WorkersInMe.FirstOrDefault().Worker.DepartmentID == id).ToList();

        /// <summary>
        /// Give a list of Roles and a Name to check and I will Filter the list and return it
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static List<IdentityRole> FilterRoleByName(string searchString, List<IdentityRole> roles) => roles.Where(x => x.Name.Contains(searchString)).ToList();

    }

}