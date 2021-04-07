using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.StatisticsModels
{
    public class Ratio
    {  
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