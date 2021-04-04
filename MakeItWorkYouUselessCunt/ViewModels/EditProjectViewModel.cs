using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.Models;

namespace ManagementSystemVersionTwo.ViewModels
{
    public class EditProjectViewModel {

        public Project Project { get; set; }

        public HttpPostedFileBase Attach { get; set; }

        public List<DummyForProject> Users { get; set; }
    }
}