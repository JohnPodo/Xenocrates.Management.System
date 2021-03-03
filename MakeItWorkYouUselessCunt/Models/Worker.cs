using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class Worker
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public decimal Salary { get; set; }
        public string Pic { get; set; }
        public string CV { get; set; }
        public string ContractOfEmployment { get; set; }

        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<ProjectsAssignedToEmployee> MyProjects { get; set; }

        
       // public string ApplicationUserID { get; set; }
       [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}