using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class Excel_Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            string fileName = fileUpload.FileName;
            string fileExt = Path.GetExtension(fileName);
            if (fileExt == ".xls" || fileExt == ".xlsx")
            {
                string filePath = Server.MapPath("~/Uploads/") + fileName;
                fileUpload.SaveAs(filePath);

                DataSet ds = new DataSet();
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;
                    if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    ISheet sheet = workbook.GetSheetAt(0);
                    IRow headerRow = sheet.GetRow(5);
                    int columnCount = headerRow.LastCellNum;
                    for (int i = 5; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null && !string.IsNullOrEmpty(row.GetCell(3).ToString()))
                        {
                            DataRow dataRow = ds.Tables[112].NewRow();
                            for (int j = 0; j < columnCount; j++)
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                            ds.Tables[0].Rows.Add(dataRow);
                        }
                        else
                        {
                            string errorMsg = string.Format("第 {0} 列的欄位 D 為空值，請檢查！", i + 1);
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + errorMsg + "');", true);
                            return;
                        }
                    }
                }
                string conn = ConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        bulkCopy.DestinationTableName = "TableName";
                        bulkCopy.BatchSize = ds.Tables[0].Rows.Count;
                        con.Open();
                        bulkCopy.WriteToServer(ds.Tables[0]);
                        con.Close();
                    }
                }
                // 顯示上傳成功訊息
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('上傳成功！');", true);
            }
            else
            {
                // 顯示不支援的檔案格式訊息
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('不支援的檔案格式！');", true);
            }
        }
    }
}