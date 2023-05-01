using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIC.Models
{
    public class PonitBooks
    {
        string Dept { get; set; }
        string EmpName { get; set; }
        string EID { get; set; }
        decimal BasePoint { get; set; }
        decimal WeightPoint { get; set; }
        decimal TeacherPoint { get; set; }
        decimal SubstituteTraining { get; set; }
        decimal TotalScore { get; set; }
        string Remark { get;set; }
    }
}