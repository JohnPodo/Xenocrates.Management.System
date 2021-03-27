using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paypal
{
    internal class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
        public string encType { get; set; }
    }

    internal class Detail
    {
        public string field { get; set; }
        public string location { get; set; }
        public string issue { get; set; }
        public List<Link> link { get; set; }
    }

    internal class PaymentInfo
    {
        public string name { get; set; }
        public string message { get; set; }
        public string debug_id { get; set; }
        public string information_link { get; set; }
        public List<Detail> details { get; set; }
        public List<Link> links { get; set; }
    }
}
