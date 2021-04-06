using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class Department 
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Only Letters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Necessary")]
        [RegularExpression("^[A-Za-z]+ [0-9]+$", ErrorMessage = "Only Letters")]
        public string Adress { get; set; }

        public virtual ICollection<Worker> WorkersInThisDepartment { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}