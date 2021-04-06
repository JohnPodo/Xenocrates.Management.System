using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.ViewModels;

namespace ManagementSystemVersionTwo.CustomAnnotations
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]
    sealed public class LegalAge : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            var year = DateTime.Now.Year - date.Year;
            return (year >= 18 && year <= 72);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}