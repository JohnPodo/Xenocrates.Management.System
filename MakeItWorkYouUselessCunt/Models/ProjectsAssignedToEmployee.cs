using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class ProjectsAssignedToEmployee
    {
        public int ID { get; set; }

        public int WorkerID { get; set; }

        public virtual Worker Worker { get; set; }

        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }
    }
}