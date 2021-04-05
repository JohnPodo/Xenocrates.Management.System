using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class WorkingDays
    {
        public int ID { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Title { get; set; }

        public string Display { get; set; }

        public string BackgroundColor { get; set; }

        public virtual Worker Worker { get; set; }
    }
}