﻿using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.StatisticsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace ManagementSystemVersionTwo.Services.StatisticsServices
{
    public class StatisticsForDashboard : IDisposable
    {
        private ApplicationDbContext _context;

        public StatisticsForDashboard()
        {
            _context = new ApplicationDbContext();
        }

        #region Statistics For Administrator


        /// <summary>
        /// Provides data for chart to display the percentage of Departments Per City
        /// </summary>
        /// <returns></returns>
        public Ratio DepartmentsPerCityChart()
        {
            int count;
            var departments = _context.Departments.Select(d => d.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departments)
            {

                count = departments.Count(x => x == item);
                obj.Count.Add(count);
            }
            foreach (var item in departments)
            {
                obj.Names.Add(item);
            }

            return obj;
        }

        /// <summary>
        /// Provides data for chart to display the Employees Per Department
        /// </summary>
        /// <returns></returns>
        public Ratio EmployeesPerDepartmentChart()
        {
            var departmentNames = _context.Departments.Select(x => x.City + " " + x.Adress).ToList();
            var employeesCount = _context.Departments.Select(x => x.WorkersInThisDepartment.Count()).ToList();

            Ratio obj = new Ratio();
            obj.Names = departmentNames;
            obj.Count = employeesCount;
            return obj;
        }

        /// <summary>
        /// Provides data for chart to display the Average Salary of Employees Per Department
        /// </summary>
        /// <returns></returns>
        public Ratio AverageSalaryChart()
        {
            decimal averageSalary;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                var averageSalariesList = _context.Workers.Where(x => x.Department.City == item).Select(x => x.Salary).ToList();
                averageSalary = averageSalariesList.Average();
                obj.Salaries.Add(averageSalary);
            }
            obj.Names = departmentNames;
            return obj;
        }

        /// <summary>
        /// Provides data for chart to display the Average Age of Employees Per Department
        /// </summary>
        /// <returns></returns>
        public Ratio AverageAgeChart()
        {
            double ages;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                var agesList = _context.Workers.Where(x => x.Department.City == item).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).ToList();
                ages = agesList.Average();
                obj.AverageAge.Add(ages);
            }
            obj.Names = departmentNames;
            return obj;

        }


        /// <summary>
        /// Provides data for chart to display the Gender Ratio Per Department
        /// </summary>
        /// <returns></returns>
        public Ratio GenderPerDepartmentChart()
        {
            Ratio obj = new Ratio();

            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            var departmentIds = _context.Departments.Select(x => x.ID).ToList();
            var dep = _context.Workers.ToList();
            foreach (var id in departmentIds)
            {
                var males = dep.Where(x => x.DepartmentID == id).Count(x => x.Gender == "Male");
                obj.Count.Add(males);
                var females = dep.Where(x => x.DepartmentID == id).Count(x => x.Gender == "Female");
                obj.Ages.Add(females);
            }

            obj.Names = departmentNames;
            return obj;

        }

        /// <summary>
        /// Provides data for chart to display the Total Amount of Salaries Paid Per Month
        /// </summary>
        /// <returns></returns>
        public Ratio TotalSalariesPerMonthChart()
        {
            decimal salary;
            var months = _context.Payments.Select(x => x.Date.Month).Distinct().ToList();
            Ratio obj = new Ratio();
            foreach (var item in months)
            {
                var salariesList = _context.Payments.Where(x => x.Date.Month == item).Select(x => x.Amount).ToList();
                salary = salariesList.Sum();
                obj.Salaries.Add(salary);
            }
            obj.Names = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            return obj;
        }


        /// <summary>
        /// Provides data for chart to display the Total Amount of Salaries Paid Per Department
        /// </summary>
        /// <returns></returns>
        public Ratio TotalSalaryPerDepartmentChart()
        {

            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            var departmentIds = _context.Departments.Select(x => x.ID).ToList();
            Ratio obj = new Ratio();
            obj.Names = departmentNames;

            foreach (var item in departmentIds)
            {
                obj.PaymentsPerMonth = new List<decimal>();
                for (int i = 1; i <= 12; i++)
                {

                    var salariesAmounts = _context.Payments.Where(x => x.Worker.DepartmentID == item && x.Date.Month == i).Select(x=>x.Amount).ToList();
                    var payments = salariesAmounts.Sum();
                    obj.PaymentsPerMonth.Add(payments);
                }
                obj.DepartmentsPaymentsPerMonth.Add(obj.PaymentsPerMonth);
            }

            return obj;
        }





        #endregion

        #region Statistics For Supervisor

        /// <summary>
        /// Provides data for chart to display the Salary of Each Employee that belongs to the department of the supervisor that is logged in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ratio SalaryPerEmployeeChart(string id)
        {
            var supervisor = _context.Users.Find(id);
            var salaries = _context.Workers.Where(x => x.DepartmentID == supervisor.Worker.DepartmentID).Select(x => x.Salary).ToList();
            var names = _context.Workers.Where(x => x.DepartmentID == supervisor.Worker.DepartmentID).Select(w => (w.FirstName + " " + w.LastName)).ToList();
            Ratio obj = new Ratio();
            obj.Salaries = salaries;
            obj.Names = names;
            return obj;
        }

        /// <summary>
        /// Provides data for chart to display the Age of Each Employee that belongs to the department of the supervisor that is logged in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ratio AgePerEmployeeChart(string id)
        {
            var supervisor = _context.Users.Find(id);
            var ages = _context.Workers.Where(x => x.DepartmentID == supervisor.Worker.DepartmentID).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).ToList();
            var names = _context.Workers.Where(x => x.DepartmentID == supervisor.Worker.DepartmentID).Select(w => (w.FirstName + " " + w.LastName)).ToList();
            Ratio obj = new Ratio();
            obj.Ages = ages;
            obj.Names = names;
            return obj;
        }

        /// <summary>
        /// Provides data for chart to display the number of Projects for the department of the supervisor that is logged in 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ratio ProjectsPerMonthChart(string id)
        {
            int projects;
            var supervisor = _context.Users.Find(id);
            var months = _context.Projects.Select(x => x.StartDate.Month).Distinct().ToList();
            Ratio obj = new Ratio();
            foreach (var item in months)
            {
                projects = _context.Projects.Where(x => x.StartDate.Month == item && x.WorkersInMe.Any(d=>d.WorkerID == supervisor.Worker.ID)).Select(x => x.StartDate.Month).Count();
                obj.Count.Add(projects);
            }
            obj.Names = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            return obj;
        }


        #endregion

        #region Statistics For Employee


        /// <summary>
        /// Provides data for chart to display the percentage of Active and Finished Projects for the employee that is logged in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ratio ProjectsProgressChart(string id)
        {
            var employee = _context.Users.Find(id);
            var projectsClosed = _context.Projects.Where(x => x.Finished == true && x.WorkersInMe.Any(w=>w.WorkerID == employee.Worker.ID)).Count();
            var projectsOpen = _context.Projects.Where(x => x.Finished == false).Count();

            Ratio obj = new Ratio();
            obj.MaleCount = projectsClosed;
            obj.FemaleCount = projectsOpen;
            return obj;
        }

        


        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }




    }
}