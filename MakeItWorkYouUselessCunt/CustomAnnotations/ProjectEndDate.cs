using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.CustomAnnotations
{
    [AttributeUsage(AttributeTargets.Property |
     AttributeTargets.Field, AllowMultiple = false)]
    sealed public class ProjectEndDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var pro = (Project)context.ObjectInstance;
            var endProject = DateTime.Compare(pro.EndDate.Date, pro.StartDate.Date);
            var today = DateTime.Compare(pro.EndDate.Date, DateTime.Now.Date);
            return endProject>0&& today>0 ? ValidationResult.Success : new ValidationResult("Wrong End Date");
        }

    }


}