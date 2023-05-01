using CIC.Models;
using CIC.ViewModel;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Description;
using System.Xml;
using static CIC.Models.CIC_ReportModel;
using static CIC.Models.PointBookModel;
using static CIC.ViewModel.ViewModel_PointBook;
namespace CIC
{
    public class TestController : ApiController
    {
        private CIC_ReportModel db = new CIC_ReportModel();
        //[AcceptVerbs("Get")]
        //public string Hello()
        //{
        //    return "Hello World Leo";
        //}

        //[AcceptVerbs("Get")]
        //public List<string> GetCustomers()
        //{
        //    try
        //    {
        //        List<string> ret = new List<string>();

        //        using (SqlConnection con = new SqlConnection("CIC_ReportEntities"))
        //        {
        //            con.Open();

        //            using (SqlCommand com = new SqlCommand("Select * From PointBook", con))
        //            {
        //                using (SqlDataReader reader = com.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        ret.Add(reader["EmpName"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public class CICBookViewModel
        {
            public string Message { get; set; }
            public int EID { get; set; }
            public string DEPT { get; set; }
            public string NAME_ZH { get; set; }
        }
        //[HttpGet]
        //[Route("CIC/List")]
        //public IHttpActionResult GetBook()
        //{
        //    var books = db.CICBOOK.Select(d => new CICBookViewModel
        //    {
        //        Message = "點數申請表",
        //        EID = d.EID,
        //        DEPT = d.DEPT,
        //        NAME_ZH = d.NAME_ZH
        //    }).ToList();
        //    return Ok(books);
        //}

        #region XML格式的API        
        //[HttpGet]
        //[Route("Point_Book/List")]
        //public IHttpActionResult GetPointBook() 
        //{
        //    using (PointBookModel db = new PointBookModel())
        //    {
        //        var Pb = db.PointBook.Select(b => new ViewModel_PointBook
        //        {
        //            DEPT = b.Dept,
        //            EmpName = b.EmpName,
        //            EID = b.EID,
        //            //BasePoint = Convert.ToInt32(b.BasePoint),
        //            //WeightPoint = Convert.ToInt32(b.WeightPoint),
        //            //TeacherPoint = Convert.ToInt32(b.TeacherPoint),
        //            //SubstituteTraining = Convert.ToInt32(b.SubstituteTraining),
        //            Remark = b.Remark,
        //        }).ToList();
        //        return Ok(Pb);
        //    }
        //}
        #endregion

        #region Json格式的API
        //[HttpGet]
        //[Route("Point_Book/List")]
        //public JsonResult<List<ViewModel_PointBook>> GetPointBook()
        //{
        //    using (PointBookModel db = new PointBookModel())
        //    {
        //        var Pb = db.PointBook.Select(b => new ViewModel_PointBook
        //        {
        //            DEPT = b.Dept,
        //            EmpName = b.EmpName,
        //            EID = b.EID,
        //            //BasePoint = Convert.ToInt32(b.BasePoint),
        //            //WeightPoint = Convert.ToInt32(b.WeightPoint),
        //            //TeacherPoint = Convert.ToInt32(b.TeacherPoint),
        //            //SubstituteTraining = Convert.ToInt32(b.SubstituteTraining),
        //            Remark = b.Remark,
        //        }).ToList();
        //        return Json(Pb);
        //    }
        //}
        #endregion

        #region API
        [HttpGet]
        [Route("api/MyEntities")]
        public IHttpActionResult GetMyEntities()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = "SELECT * FROM PointBook";
                    var cmd = new SqlCommand(query, conn);
                    var entities = new List<ViewModel_PointBook>();
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var entity = new ViewModel_PointBook
                        {
                            DEPT = (string)reader["Dept"],
                            EmpName = (string)reader["EmpName"],
                            EID = (string)reader["EID"],
                            BasePoint = (double)reader["BasePoint"],
                            WeightPoint = (double)reader["WeightPoint"],
                            TeacherPoint = reader.IsDBNull(reader.GetOrdinal("TeacherPoint")) ? 0 : (double)reader["TeacherPoint"],
                            //SubstituteTraining = reader.IsDBNull(reader.GetOrdinal("SubstituteTraining")) ? 0 : (double)reader["SubstituteTraining"],

                            Remark = (string)reader["Remark"],
                    };
                        entities.Add(entity);
                    }
                    reader.Close();
                    return Json(entities); // 回傳 JSON 格式的物件
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //var connectionString = "Data Source=(local)\\SqlExpress;Initial Catalog=CIC_Report;Integrated Security=True";
            //using (var conn = new SqlConnection(connectionString))
            //{
            //    var query = "SELECT * FROM PointBook";
            //    var cmd = new SqlCommand(query, conn);
            //    var entities = new List<ViewModel_PointBook>();
            //    conn.Open();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var entity = new ViewModel_PointBook
            //        {
            //            DEPT = (string)reader["Dept"],
            //            EmpName = (string)reader["EmpName"],
            //            EID = (string)reader["EID"],
            //            //BasePoint = (double)reader["BasePoint"],
            //            //WeightPoint = (double)reader["WeightPoint"],
            //            //TeacherPoint = (double)reader["TeacherPoint"],
            //            //SubstituteTraining = (double)reader["SubstituteTraining"],
            //            Remark = (string)reader["Remark"],
            //        };
            //        entities.Add(entity);
            //    }
            //    reader.Close();
            //    var json = JsonConvert.SerializeObject(entities);
            //    return Ok(json);
            //}
        }
        #endregion

        
    }
}