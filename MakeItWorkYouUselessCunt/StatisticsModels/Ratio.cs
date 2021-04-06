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

        
        

        
        
        public List<decimal> SalariesPerMonthAthens { get; set; }
        public List<decimal> SalariesPerMonthThessaloniki { get; set; }

        public List<int> Count { get; set; }

        public List<decimal> Salaries { get; set; }

        public List<string> Names { get; set; }
        public List<int> Ages { get; set; }
        public List<double> AverageAge { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
        public List<decimal> PaymentsPerMonth { get; set; }

        public List<List<decimal>> DepartmentsPaymentsPerMonth { get; set; }



        public Ratio()
        {
            Count = new List<int>();
            Names = new List<string>();
            Salaries = new List<decimal>();
            Ages = new List<int>();
            AverageAge = new List<double>();
            PaymentsPerMonth = new List<decimal>();
            DepartmentsPaymentsPerMonth = new List<List<decimal>>();
        }


    }
}