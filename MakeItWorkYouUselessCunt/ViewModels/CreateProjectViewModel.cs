using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.ViewModels
{
    public class CreateProjectViewModel
    {
        public Project Project { get; set; }

        public List<DummyForProject> Users { get; set; }
    }
}