using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Services.WorkerServices
{
    public static class ConventionsOfHttpPostedFileBase
    {
        
        public static string ForPostedPicture(HttpPostedFileBase Pic)
        {
            //Check If File Name Description Images Exists and If Not He make It
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProfPics/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Pic != null)
            {
                string fileName = Path.GetFileName(Pic.FileName);
                Pic.SaveAs(path + fileName);
                string saveMe = fileName;
                return saveMe;
            }
            return null;

        }

        public static string ForCV(HttpPostedFileBase CV)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/CVs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (CV != null)
            {
                string fileName = Path.GetFileName(CV.FileName);
                CV.SaveAs(path + fileName);
                string saveMe = fileName;
                return saveMe;
            }
            return null;

        }

        public static string ForContractOfEmployments(HttpPostedFileBase ContractOfEmployment)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ContractOfEmployments/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (ContractOfEmployment != null)
            {
                string fileName = Path.GetFileName(ContractOfEmployment.FileName);
                ContractOfEmployment.SaveAs(path + fileName);
                string saveMe = fileName;
                return saveMe;

            }
            return null;
        }

    }
}