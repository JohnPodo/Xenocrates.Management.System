using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.StatisticsModels
{
    public class Ratio
    {
        public int Athens { get; set; }
        public int Thessaloniki { get; set; }
        public int Larissa { get; set; }

        public decimal SalaryAthens { get; set; }
        public decimal SalaryThessaloniki { get; set; }

        public double AgeAthens { get; set; }
        public double AgeThessaloniki { get; set; }

        public List<decimal> Salaries { get; set; }

        public List<string> Names { get; set; }

        public List<int> Ages { get; set; }

        public int MaleAthens { get; set; }
        public int MaleThessaloniki { get; set; }
        public int FemaleAthens { get; set; }
        public int FemaleThessaloniki { get; set; }
        public List<decimal> Months { get; set; }
        public List<decimal> SalariesPerMonthAthens { get; set; }
        public List<decimal> SalariesPerMonthThessaloniki { get; set; }

    }
}