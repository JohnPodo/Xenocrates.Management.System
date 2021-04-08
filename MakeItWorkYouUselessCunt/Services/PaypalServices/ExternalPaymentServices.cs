using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ManagementSystemVersionTwo.Models;
using Newtonsoft.Json;

namespace ManagementSystemVersionTwo.Services.PaypalServices
{
    public class ExternalPaymentServices: IDisposable
    {
        private ApplicationDbContext _db;

        public ExternalPaymentServices()
        {
            _db = new ApplicationDbContext();
        }

        public void SavePayment(string task,int workerID)
        {
            var worker= _db.Workers.Find(workerID);
            PaymentDetails payment = JsonConvert.DeserializeObject<PaymentDetails>(task);
            payment.Worker = worker;
            _db.Payments.Add(payment);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}