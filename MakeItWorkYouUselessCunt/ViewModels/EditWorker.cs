using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.CustomAnnotations;
using ManagementSystemVersionTwo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystemVersionTwo.ViewModels
{
    public class EditWorker
    {
        public string UserID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only Letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only Letters")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Necessary")]
        [LegalAge(ErrorMessage = "Not Legal Age To Hire")]
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

        [CheckFileIfItIsPnG(ErrorMessage = "EIPA SYGKENTRWSOU VLAKA")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        [CheckFileIfItIsPDF(ErrorMessage = "Sygkentrwsou Vlaka")]
        public HttpPostedFileBase CV { get; set; }

        [CheckFileIfItIsPDF(ErrorMessage = "Sygkentrwsou Vlaka")]
        public HttpPostedFileBase ContractOfEmployment { get; set; }

        public List<Department> AllDepartments { get; set; }
        [Required(ErrorMessage = "Necessary")]
        public int IdOfDepartment { get; set; }
        public List<IdentityRole> Roles { get; set; }
        [Required(ErrorMessage = "Necessary")]
        public string SelectedRole { get; set; }
    }
}
