using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class PaymentDetails
    {
        public int ID { get; set; }

        public string PaymentID { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public virtual Worker Worker { get; set; }
    }
}