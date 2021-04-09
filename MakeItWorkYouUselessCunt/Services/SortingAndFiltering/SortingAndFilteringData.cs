using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Filtering;
using ManagementSystemVersionTwo.Services.Sorting;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.Services.SortingAndFiltering
{
    public static class SortingAndFilteringData
    {
        /// <summary>
        /// Give me the List of Departments to sort and the parameters to filter and sort and I will do the work and return
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <param name="data"></param>
        public static List<Department> SortAndFilterDepartments(string search,string sort,List<Department> data)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                data = SortingServices.SortDepartments(sort, data);
            }
            if (!string.IsNullOrEmpty(search))
            {
                data = FilteringServices.FilterDepartmentsByCity(search, data);
            }
            return data;
        }

        /// <summary>
        /// Give me the List of Workers to sort and the parameters to filter and sort and I will do the work and return
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="orderBy"></param>
        /// <param name="roleSpec"></param>
        /// <param name="depID"></param>
        /// <param name="data"></param>
        public static List<Worker> SortAndFilterWorkers(string searchName, string orderBy, string roleSpec, string depID, List<Worker> data)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                data = FilteringServices.FilterWorkerByName(searchName, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = SortingServices.SortWorker(orderBy, data);
            }
            if (!string.IsNullOrEmpty(roleSpec))
            {
                data = FilteringServices.FilterWorkersInRole(roleSpec, data);
            }
            if (!string.IsNullOrEmpty(depID))
            {
                data = FilteringServices.FilterWorkersPerDepartment(int.Parse(depID), data);
            }
            return data;
        }

        /// <summary>
        /// Give me the List of Roles to sort and the parameters to filter and sort and I will do the work and return
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="sort"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<IdentityRole> SortAndFilterRoles(string searchString, string sort, List<IdentityRole> data)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                data = FilteringServices.FilterRoleByName(searchString, data);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                data = SortingServices.SortRoles(sort, data);
            }
            return data;
        }

        /// <summary>
        /// Give me the List of Projects to sort and the parameters to filter and sort and I will do the work and return
        /// </summary>
        /// <param name="title"></param>
        /// <param name="orderBy"></param>
        /// <param name="depID"></param>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Project> SortAndFilterProjects(string title, string orderBy, string depID, string status, List<Project> data)
        {
            if (!string.IsNullOrEmpty(title))
            {
                data = FilteringServices.FilterProjectByTitle(title, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = SortingServices.SortProject(orderBy, data);
            }
            if (!string.IsNullOrEmpty(depID))
            {
                data = FilteringServices.FilterProjectsPerDepartment(int.Parse(depID), data);
            }
            if (!string.IsNullOrEmpty(status))
            {
                data = FilteringServices.StatusProject(status, data);
            }
            return data;
        }

        public static List<Worker> SortAndFilteringPayments(string searchName, string orderBy, List<Worker> data)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                data = FilteringServices.FilterWorkerByName(searchName, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = SortingServices.SortSalary(orderBy, data);
            }
            return data;
        }
    }
}