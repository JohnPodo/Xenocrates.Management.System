using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual Department Department { get; set; }

    }
}