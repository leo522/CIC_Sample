using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIC.ViewModel
{
    public class ViewModel_PointBook
    {
        public string DEPT { get; set; }
        public string EmpName { get; set; }
        public string EID { get; set; }
        public double BasePoint { get; set; }
        public double WeightPoint { get; set; }
        public double TeacherPoint { get; set; }
        public double SubstituteTraining { get; set; }
        public string Remark { get; set; }
    }
}