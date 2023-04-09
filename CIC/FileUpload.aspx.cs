using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ExcelUpload.HasFile)
            {
                // 取得檔案名稱和路徑
                string fileName = ExcelUpload.FileName;
                string filePath = Server.MapPath("~/uploads/" + fileName);

                // 上傳檔案到伺服器
                ExcelUpload.SaveAs(filePath);

                // 讀取 Excel 檔案
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    //HSSFWorkbook workbook = new HSSFWorkbook(stream);
                    //ISheet sheet = workbook.GetSheetAt(0);

                    // 取得最後一列的索引
                    //int lastRow = sheet.LastRowNum;

                    // 逐行讀取資料
                    //    for (int i = 1; i < lastRow; i++)
                    //    {
                    //        //IRow row = sheet.GetRow(i);

                    //        // 取得每個儲存格的值
                    //        string col1 = row.GetCell(0).StringCellValue;
                    //        string col2 = row.GetCell(1).StringCellValue;
                    //        int col3 = (int)row.GetCell(2).NumericCellValue;

                    //        // 將資料寫入資料庫
                    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[""].ConnectionString))
                    //        {
                    //            SqlCommand command = new SqlCommand("Inser Into Values()", con);

                    //            command.Parameters.AddWithValue("@", col1);
                    //            con.Open();
                    //            command.ExecuteNonQuery();
                    //        }
                    //    }
                    //}
                    // 刪除上傳的檔案
                    File.Delete(filePath);

                    // 顯示匯入成功的訊息
                    Response.Write("<script>alert('匯入成功！');</script>");
                }
            }
            else
            {
                // 如果沒有選擇檔案，顯示錯誤訊息
                Response.Write("<script>alert('請選擇要匯入的 Excel 檔案！');</script>");
            }
        }
    }
}