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
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class Excel_DataTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack == false)
            {
                try
                {
                    if (FileUpload.HasFile)
                    {
                        string fileName = FileUpload.FileName;
                        string fileExtension = Path.GetExtension(fileName);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string filePath = Server.MapPath("~/uploads/" + fileName);
                            FileUpload.SaveAs(filePath);

                            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                IWorkbook workbook = null;
                                if (fileExtension == ".xls")
                                {
                                    workbook = new HSSFWorkbook(stream);
                                }
                                else if (fileExtension == ".xlsx'")
                                {
                                    workbook = new XSSFWorkbook(stream);
                                }

                                ISheet sheet = workbook.GetSheetAt(0);
                                int lastRow = sheet.LastRowNum;

                                DataTable dTs = new DataTable();
                                dTs.Columns.Add("Dept", typeof(string));
                                dTs.Columns.Add("EmpName", typeof(string));
                                dTs.Columns.Add("EID", typeof(string));
                                dTs.Columns.Add("BasePoint", typeof(float));
                                dTs.Columns.Add("WeightPoint", typeof(float));
                                dTs.Columns.Add("TeacherPoint", typeof(float));
                                dTs.Columns.Add("SubstituteTraining", typeof(float));
                                dTs.Columns.Add("TotalScore", typeof(float));
                                dTs.Columns.Add("Remark", typeof(string));


                                for (int i = 5; i <= lastRow - 6; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (sheet.GetRow(i).GetCell(0) != null &&
                                        sheet.GetRow(i).GetCell(1) != null &&
                                        sheet.GetRow(i).GetCell(2) != null &&
                                        sheet.GetRow(i).GetCell(3) != null &&
                                        sheet.GetRow(i).GetCell(4) != null &&
                                        sheet.GetRow(i).GetCell(5) != null &&
                                        sheet.GetRow(i).GetCell(6) != null &&
                                        sheet.GetRow(i).GetCell(7) != null &&
                                        sheet.GetRow(i).GetCell(8) != null &&
                                        sheet.GetRow(i).GetCell(9) != null)
                                    {
                                        DataRow dtRow = dTs.NewRow();
                                        dtRow["Dept"] = row.GetCell(1).ToString();
                                        dtRow["EmpName"] = row.GetCell(2).ToString();

                                        if (String.IsNullOrWhiteSpace(row.GetCell(3).ToString()))
                                        {
                                            Response.Write("<script>alert('識別證號碼不能為空!');</script>");
                                            return;
                                        }
                                        dtRow["EID"] = row.GetCell(3).ToString();
                                        if (row.GetCell(4).CellType == CellType.Numeric)
                                        {
                                            dtRow["BasePoint"] = (float)row.GetCell(4).NumericCellValue;
                                        }

                                        if (row.GetCell(5).CellType == CellType.Numeric)
                                        {
                                            dtRow["WeightPoint"] = (float)row.GetCell(5).NumericCellValue;
                                        }

                                        if (row.GetCell(6).CellType == CellType.Numeric)
                                        {
                                            dtRow["TeacherPoint"] = (float)row.GetCell(6).NumericCellValue;
                                        }

                                        if (row.GetCell(7).CellType == CellType.Numeric)
                                        {
                                            dtRow["SubstituteTraining"] = (float)row.GetCell(7).NumericCellValue;
                                        }

                                        if (row.GetCell(8).CellType == CellType.Numeric)
                                        {
                                            dtRow["TotalScore"] = (float)row.GetCell(8).NumericCellValue;
                                        }

                                        dtRow["Remark"] = row.GetCell(9).ToString();
                                        dTs.Rows.Add(dtRow);
                                    }
                                }

                                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString))
                                {
                                    connection.Open();

                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                                    {
                                        bulkCopy.DestinationTableName = "PointBook";

                                        foreach (DataColumn column in dTs.Columns)
                                        {
                                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                                        }
                                        bulkCopy.WriteToServer(dTs);
                                    }
                                }
                            }
                            File.Delete(filePath);

                            Response.Write("<script>alert('匯入成功！');</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('請選擇要匯入的Excel檔案！');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        private bool IsRowValid(IRow row)
        {
            for (int i = 0; i < row.LastCellNum; i++)
            {
                if (row.GetCell(i) == null || row.GetCell(i).CellType == CellType.Blank)
                {
                    return false;
                }
            }
            return true;
        }

        protected void btn_Up_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["IsButtonClicked"] == null || !(bool)Session["IsButtonClicked"])
                {
                    Session["IsButtonClicked"] = true;
                }
                else
                {
                    Session["IsButtonClicked"] = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }   

    }
}