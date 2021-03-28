using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.CustomAnnotations
{
    [AttributeUsage(AttributeTargets.Property |
    AttributeTargets.Field, AllowMultiple = false)]
    sealed public class CheckFileIfItIsPnG : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = (HttpPostedFileBase)value;
            var check = true;
            if (!(file is null)){ 
            check= file.FileName.EndsWith(".png");
            }
            return check;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}