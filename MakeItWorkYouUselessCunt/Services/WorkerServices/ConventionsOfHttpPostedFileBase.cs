using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Services.WorkerServices
{
    public static class ConventionsOfHttpPostedFileBase
    {
            /// <summary>
            /// Give me a File type HttpPostedFileBase and I will convert it in Byte[] and return it to you
            /// </summary>
            //public static byte[] FromHttpPostedFileBaseToByteArray(HttpPostedFileBase imageIn)
            //{
            //    using (Stream inputStream = imageIn.InputStream)
            //    {
            //        MemoryStream memoryStream = inputStream as MemoryStream;
            //        if (memoryStream == null)
            //        {
            //            memoryStream = new MemoryStream();
            //            inputStream.CopyTo(memoryStream);
            //        }
            //        byte[] data = memoryStream.ToArray();
            //        return data;
            //    }
            //}

            ///<summary>
            ///Save Picture to File
            ///</summary>
            public static string ForPostedPicture(HttpPostedFileBase Pic)
            {
                //Check If File Name Description Images Exists and If Not He make It
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ProfPics/");
                if (Pic != null)
                {
                    string fileName = Path.GetFileName(Pic.FileName);
                    Pic.SaveAs(path + fileName);
                    string saveMe = fileName;
                    return saveMe;
                }
                return null;

            }

            /// <summary>
            ///  Give me the PDF HttpPostedFileBase I will save it in the folder you want and return you the string name to save in db
            /// </summary>
            public static string ForCV(HttpPostedFileBase CV)
            {
                //Check If File Name Description Images Exists and If Not He make It
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/CVs/");
                if (CV != null)
                {
                    string fileName = Path.GetFileName(CV.FileName);
                    CV.SaveAs(path + fileName);
                    string saveMe = fileName;
                    return saveMe;
                }
                return null;

            }

            /// <summary>
            ///  Give me the ContractOfEmployment HttpPostedFileBase I will save it in the folder you want and return you the string name to save in db
            /// </summary>
            public static string ForContractOfEmployments(HttpPostedFileBase ContractOfEmployment)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ContractOfEmployments/");
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