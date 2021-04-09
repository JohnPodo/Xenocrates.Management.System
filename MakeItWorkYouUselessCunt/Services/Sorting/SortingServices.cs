using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.Sorting
{
    public static class SortingServices
    {
        /// <summary>
        /// Give me a string to OrderBy and the list of Workers to be Ordered And I will return it Ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Worker> SortWorker(string sort, List<Worker> data)
        {

            switch (sort)
            {
                case "City Of Department":
                    return data.OrderBy(w => w.Department.City).ToList();
                case "Full Name":
                    return data.OrderBy(w => (w.FirstName + " " + w.LastName).ToUpper()).ToList();
                case "Age":
                    return data.OrderBy(w => w.DateOfBirth).ToList();
                case "Salary":
                    return data.OrderBy(w => w.Salary).ToList();
                case "Projects":
                    return data.OrderBy(w => w.MyProjects.Count).ToList();
                default:
                    return data;
            }
        }

        /// <summary>
        ///  Give me a string to OrderBy and the list of Departments to be Ordered And I will return it Ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="departments"></param>
        /// <returns></returns>
        public static List<Department> SortDepartments(string sort, List<Department> departments)
        {
            switch (sort)
            {
                case "City":
                    return departments.OrderBy(x => x.City).ToList();

                case "City_desc":
                    return departments.OrderByDescending(x => x.City).ToList();
                case "Low-High":
                    return departments.OrderBy(x => x.WorkersInThisDepartment.Count).ToList();

                case "High-Low":
                    return departments.OrderByDescending(x => x.WorkersInThisDepartment.Count).ToList();
                default:
                    return departments;

            }
        }

        /// <summary>
        ///  Give me a string to OrderBy and the list of Roles to be Ordered And I will return it Ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static List<IdentityRole> SortRoles(string sort, List<IdentityRole> roles)
        {
            switch (sort)
            {
                case "Role":
                    return roles.OrderBy(x => x.Name).ToList();

                case "Role_desc":
                    return roles.OrderByDescending(x => x.Name).ToList();
                case "Low-High":
                    return roles.OrderBy(x => x.Users.Count).ToList();

                case "High-Low":
                    return roles.OrderByDescending(x => x.Users.Count).ToList();
                default:
                    return roles;

            }
        }

        /// <summary>
        /// Give me a list of Workers and the orderby condition and I will return it ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static List<Worker> SortSalary(string sort, List<Worker> payments)
        {
            switch (sort)
            {
                case "Salary Desc":
                    return payments.OrderByDescending(x => x.Salary).ToList();
                case "Salary Asc":
                    return payments.OrderBy(x => x.Salary).ToList();
                default:
                    return payments;
            }
        }

        /// <summary>
        /// Give me a list of Payments and the orderby condition and I will return it ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<PaymentDetails> SortDate(string sort, List<PaymentDetails> date)
        {
            switch (sort)
            {
                case "Date Asc":
                    return date.OrderBy(x => x.Date).ToList();
                case "Date Desc":
                    return date.OrderByDescending(x => x.Date).ToList();
                default:
                    return date;
            }
        }

        /// <summary>
        /// Give me a list of Projects and the orderby condition and I will return it ordered
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Project> SortProject(string sort, List<Project> data)
        {
            switch (sort)
            {
                case "Employees":
                    return data.OrderBy(w => w.WorkersInMe.Count).ToList();
                case "Title":
                    return data.OrderBy(w => (w.Title).ToUpper()).ToList();
                case "Start Date":
                    return data.OrderBy(w => w.StartDate).ToList();
                case "End Date":
                    return data.OrderBy(w => w.EndDate).ToList();
                default:
                    return data;
            }
        }
    }
}