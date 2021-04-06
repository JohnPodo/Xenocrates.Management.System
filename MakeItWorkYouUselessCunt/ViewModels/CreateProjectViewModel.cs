using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.CustomAnnotations;
using ManagementSystemVersionTwo.Models;


namespace ManagementSystemVersionTwo.ViewModels
{
    public class CreateProjectViewModel
    {
        public Project Project { get; set; }
        
        [Required]
        public HttpPostedFileBase Attach { get; set; }

        public List<DummyForProject> Users { get; set; }
    }
}