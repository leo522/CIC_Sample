using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class NorthWind : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductData();
            }
        }

        private void BindProductData()
        {
            string con = WebConfigurationManager.ConnectionStrings["NorthwindEntities"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(con))
            {
                var query = "Select * From Orders";

                using (SqlCommand comm = new SqlCommand(query,conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            OrderRepeater.DataSource = reader;
                            OrderRepeater.DataBind();
                        }
                    }
                }
            }
        }
    }
}