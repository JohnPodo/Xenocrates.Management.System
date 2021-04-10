using ManagementSystemVersionTwo.Models;
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


        //Gets the number of Departments Per City
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

        //Gets the number of Employees Per Department
        public Ratio EmployeesPerDepartmentChart()
        {
            var departmentNames = _context.Departments.Select(x => x.City + " " + x.Adress).ToList();
            var employeesCount = _context.Departments.Select(x => x.WorkersInThisDepartment.Count()).ToList();

            Ratio obj = new Ratio();
            obj.Names = departmentNames;
            obj.Count = employeesCount;
            return obj;
        }

        //Gets the Average Salary of Employees Per Department
        public Ratio AverageSalaryChart()
        {
            decimal averageSalary;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                averageSalary = _context.Workers.Where(x => x.Department.City == item).Select(x => x.Salary).Average();
                obj.Salaries.Add(averageSalary);
            }
            obj.Names = departmentNames;
            return obj;
        }

        //Gets the Average Age of Employees Per Department
        public Ratio AverageAgeChart()
        {
            double ages;
            var departmentNames = _context.Departments.Select(x => x.City).ToList();
            Ratio obj = new Ratio();
            foreach (var item in departmentNames)
            {
                ages = _context.Workers.Where(x => x.Department.City == item).Select(x => DateTime.Now.Year - x.DateOfBirth.Year).Average();
                obj.AverageAge.Add(ages);
            }
            obj.Names = departmentNames;
            return obj;

        }


        //Gets the Number of Male and Female Employees Per Department 
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

        //Total Amount of Payments Per Month
        public Ratio TotalSalariesPerMonthChart()
        {
            decimal salary;
            var months = _context.Payments.Select(x => x.Date.Month).Distinct().ToList();
            Ratio obj = new Ratio();
            foreach (var item in months)
            {
                salary = _context.Payments.Where(x => x.Date.Month == item).Select(x => x.Amount).Sum();
                obj.Salaries.Add(salary);
            }
            obj.Names = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            return obj;
        }

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

                    var paymentsList = _context.Payments.Where(x => x.Worker.DepartmentID == item && x.Date.Month == i).Select(x => x.Amount).ToList();
                    var payments = paymentsList.Sum();
                    obj.PaymentsPerMonth.Add(payments);
                }
                obj.DepartmentsPaymentsPerMonth.Add(obj.PaymentsPerMonth);
            }

            return obj;
        }





        #endregion


        #region Statistics For Supervisor
        //Gets the Salary of Each Employee
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

        //Gets The Age of each Employee
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

        //Get the number of projects per month
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