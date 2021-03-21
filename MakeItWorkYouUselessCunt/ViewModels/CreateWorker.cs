using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.ViewModels
{
    public class CreateWorker
    {
        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only Letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only Letters")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Necessary")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Necessary")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+ [0-9]+$", ErrorMessage = "Doesn't Look Like Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [StringLength(maximumLength: 16, MinimumLength = 16, ErrorMessage = "Must Be 16 Digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only Digits")]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Positive Numbers Only")]
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "Necessary")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required(ErrorMessage = "Necessary")]
        public HttpPostedFileBase CV { get; set; }

        [Required(ErrorMessage = "Necessary")]
        public HttpPostedFileBase ContractOfEmployment { get; set; }

        public List<Department> AllDepartments { get; set; }
        [Required(ErrorMessage = "Necessary")]
        public int IdOfDepartment { get; set; }
        public List<IdentityRole> Roles { get; set; }
        [Required(ErrorMessage = "Necessary")]
        public string SelectedRole { get; set; }
        public string userID { get; set; }
    }
}