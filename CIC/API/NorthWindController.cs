using CIC.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CIC.API
{
    public class NorthWindController : ApiController
    {
        #region 北風_客戶資料API
        [HttpGet]
        [Route("api/North")]
        public IHttpActionResult GetNorth()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["NorthwindEntities"].ConnectionString;

                using (var conn = new SqlConnection(connectionString))
                {
                    var query = "SELECT * FROM Customers";
                    var cmd = new SqlCommand(query, conn);
                    var entities = new List<ViewModel_NorthWindCustomer>();
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var entity = new ViewModel_NorthWindCustomer
                        {
                            CustomerID = (string)reader["CustomerID"],
                            CompanyName = (string)reader["CompanyName"],
                            ContactName = (string)reader["ContactName"],
                        };
                        entities.Add(entity);
                    }
                    reader.Close();
                    return Json(entities);
                }             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}