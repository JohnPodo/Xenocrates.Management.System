using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Models;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.Services.PaypalServices;
using Newtonsoft.Json;
using Paypal;

namespace ManagementSystemVersionTwo.Controllers
{
    public class PaymentController : Controller
    {
        private DataRepository _data;
        private ExternalPaymentServices _external;

        public PaymentController()
        {
            _data = new DataRepository();
            _external = new ExternalPaymentServices();
        }

        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
            _external.Dispose();

        }
        // GET: Payment
        public ActionResult Index(string searchName, string orderBy)
        {
            var data = _data.AllWorkersWithPayments();
            if (!string.IsNullOrEmpty(searchName))
            {
                data = _data.FindWorkerByName(searchName, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = _data.SortSalary(orderBy, data);
            }

            ViewBag.SortSalary = _data.SalarySortingOptionsViewBag();
            ViewBag.Names = _data.GetWorkerNamesForAutocomplete();
            return View(data);
        }

        public ActionResult ShowPartial(int id)
        {
            var worker = _data.FindWorkerByID(id);
            return PartialView("PartialViewForPayment",worker);
        }

        public async Task<ActionResult> MakePayment(int id)
        {
            var worker = _data.FindWorkerByID(id);
            var statusOfPayment = await Paypal.Paypal.MakePayment(worker.Salary, worker.ApplicationUser.Email);
            if (statusOfPayment is null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                _external.SavePayment(statusOfPayment, worker.ID);
                return RedirectToAction("Index");
            }

        }
    }
}