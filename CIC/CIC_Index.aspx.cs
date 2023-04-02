using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class CIC_Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //// 從資料庫中讀取名字欄位
            //string bibs = GetNameFromDatabase();

            //// 設定圖片上的文字
            //SetTextOnImage(bibs);

            //string bibs = Request.QueryString["Text2"];

            string bibs = Request.QueryString["BIB"];
            string result = GetNameFromDatabase(bibs);
            SetTextOnImage(result);
        }

        #region 資料庫讀取
        private string GetNameFromDatabase(string bibs)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select * From CIC_REPORT Where BIB = @bib";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@bib", bibs);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader.GetString(1);
                        string receiptList = reader.GetString(2);
                        string NoList = reader.GetString(3);
                        string effectiveDate = reader.GetString(4);
                        string dept = reader.GetString(5);

                        result = string.Format("姓名：{0}\r\n收據清單：{1}\r\n號碼清單：{2}\r\n生效日期：{3}\r\n部門：{4}",
                name, receiptList, NoList, effectiveDate, dept);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
        #endregion


        #region 讀取圖片並建立圖像物件
        private void SetTextOnImage(string name)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("~/Images/Pics.jpg"));
            Graphics g = Graphics.FromImage(img);

            // 設定文字大小、字型、位置和顏色
            Font font = new Font("Arial", 8);
            Brush brush = Brushes.Black;
            PointF point = new PointF(400, 300);

            // 在圖片上繪製文字
            g.DrawString(name, font, brush, point);

            // 儲存繪製好的圖片
            img.Save(Server.MapPath("~/Images/CertificateWithName.jpg"));
        }
        #endregion


        #region 下載圖片
        protected void btn_DownLoad_Click(object sender, EventArgs e)
        {
            string bibs = Request.Form["Text2"];
            string result = GetNameFromDatabase(bibs);
            SetTextOnImage(result);

            Response.ContentType = "image/jpeg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CertificateWithName.jpg");
            Response.TransmitFile(Server.MapPath("~/Images/CertificateWithName.jpg"));
            Response.End();
        }
        #endregion


        //protected void ImportButton_Click(object sender, EventArgs e)
        //{
        //    // 建立連線字串
        //    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/ExcelFile/112合格造冊.xlsx") + ";Extended Properties=Excel 12.0;";

        //// 建立 OleDb 連線物件
        //OleDbConnection conn = new OleDbConnection(connectionString);

        //    try
        //    {
        //        // 開啟連線
        //        conn.Open();

        //        // 建立 OleDb 查詢物件
        //        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", conn);

        //        // 執行查詢，取得資料
        //        OleDbDataReader reader = cmd.ExecuteReader();

        //        // 建立連線字串
        //        string dbConnectionString = "Data Source=YourDatabase;Initial Catalog=YourDatabaseName;Integrated Security=True";

        //        // 建立 SqlConnection 連線物件
        //        SqlConnection dbConn = new SqlConnection(dbConnectionString);

        //        // 開啟連線
        //        dbConn.Open();

        //        // 建立 SqlTransaction 交易物件
        //        SqlTransaction transaction = dbConn.BeginTransaction();

        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                // 讀取 Excel 資料
        //                string column1 = reader["Column1"].ToString();
        //                string column2 = reader["Column2"].ToString();
        //                string column3 = reader["Column3"].ToString();

        //                // 建立 SqlCommand 物件
        //                SqlCommand insertCommand = new SqlCommand("INSERT INTO YourTable(Column1, Column2, Column3) VALUES(@Column1, @Column2, @Column3)", dbConn, transaction);

        //                // 設定參數值
        //                insertCommand.Parameters.AddWithValue("@Column1", column1);
        //                insertCommand.Parameters.AddWithValue("@Column2", column2);
        //                insertCommand.Parameters.AddWithValue("@Column3", column3);

        //                // 執行 SQL 命令
        //                insertCommand.ExecuteNonQuery();
        //            }
        //            // 提交交易
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            // 發生錯誤，回復交易
        //            transaction.Rollback();

        //            throw ex;
        //        }
        //        finally 
        //        {
        //            // 關閉資料庫連線
        //            dbConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        // 關閉 Excel 檔案連線
        //        conn.Close();
        //    }
        //}
    }
}