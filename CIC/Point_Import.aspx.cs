using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class Point_Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region 檔案上傳      
        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            // 檢查是否選擇了檔案
            if (FileUpload.HasFile)
            {
                // 取得檔案名稱和路徑
                string fileName = FileUpload.FileName;
                string fileExtension = Path.GetExtension(FileUpload.FileName);

                //檢查副檔名
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string filePath = Server.MapPath("~/uploads/" + fileName);

                    // 上傳檔案到伺服器
                    FileUpload.SaveAs(filePath);

                    // 讀取 Excel 檔案
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

                        // 取得最後一列的索引
                        int lastRow = sheet.LastRowNum;

                        // 逐行讀取資料
                        for (int i = 6; i <= lastRow - 6; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            // 儲存資料的 List
                            List<string> rowData = new List<string>();

                            for (int j = 1; j < row.LastCellNum; j++)
                            {
                                ICell cell = row.GetCell(j);
                                string columnStr = string.Empty;

                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.Numeric:  // 數值格式
                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {   // 日期格式
                                                columnStr = DateTime.FromOADate(cell.NumericCellValue).ToString();
                                            }
                                            else
                                            {   // 數值格式
                                                columnStr = cell.NumericCellValue.ToString();
                                            }
                                            break;
                                        case CellType.String:   // 字串格式
                                            columnStr = cell.StringCellValue;
                                            break;
                                        case CellType.Formula:  // 公式格式
                                            var formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();
                                            var formulaValue = formulaEvaluator.Evaluate(cell);
                                            if (formulaValue.CellType == CellType.String) columnStr = formulaValue.StringValue.ToString();          // 執行公式後的值為字串型態
                                            else if (formulaValue.CellType == CellType.Numeric) columnStr = formulaValue.NumberValue.ToString();    // 執行公式後的值為數字型態
                                            break;
                                        default:
                                            break;
                                    }
                                    rowData.Add(columnStr);
                                }
                            }

                            // 將資料寫入資料庫
                            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString))
                            {
                                SqlCommand command = new SqlCommand("Insert Into PointBook (Dept,EmpName,EID,BasePoint,WeightPoint,TeacherPoint,SubstituteTraining,TotalScore,Remark) Values(@DEPT,@EMPNAME,@EID,@BASE_POINT,@WEIGHT_POINT,@TEACHER_POINT,@SUBSTITUTE_TRAINING,@TOTAL_SCORE,@REMARK)", connection);

                                command.Parameters.AddWithValue("@DEPT", rowData[0]);
                                command.Parameters.AddWithValue("@EMPNAME", rowData[1]);
                                command.Parameters.AddWithValue("@EID", rowData[2]);
                                if (string.IsNullOrEmpty(rowData[3]))
                                {
                                    command.Parameters.AddWithValue("@BASE_POINT", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@BASE_POINT", Convert.ToSingle(rowData[3]));
                                }
                                if (string.IsNullOrEmpty(rowData[4]))
                                {
                                    command.Parameters.AddWithValue("@WEIGHT_POINT", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@WEIGHT_POINT", Convert.ToSingle(rowData[4]));
                                }
                                if (string.IsNullOrEmpty(rowData[5]))
                                {
                                    command.Parameters.AddWithValue("@TEACHER_POINT", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@TEACHER_POINT", Convert.ToSingle(rowData[5]));
                                }
                                if (string.IsNullOrEmpty(rowData[6]))
                                {
                                    command.Parameters.AddWithValue("@SUBSTITUTE_TRAINING", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@SUBSTITUTE_TRAINING", Convert.ToSingle(rowData[6]));
                                }
                                if (string.IsNullOrEmpty(rowData[7]))
                                {
                                    command.Parameters.AddWithValue("@TOTAL_SCORE", DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@TOTAL_SCORE", Convert.ToSingle(rowData[7]));
                                }
                                command.Parameters.AddWithValue("@REMARK", rowData[8]);
                                connection.Open();
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    // 刪除上傳的檔案
                    File.Delete(filePath);

                    // 顯示匯入成功的訊息
                    Response.Write("<script>alert('匯入成功！');</script>");
                }
                else
                {
                    // 如果沒有選擇檔案，顯示錯誤訊息
                    Response.Write("<script>alert('請選擇要匯入的Excel檔案！');</script>");
                }
            }     
        }
        #endregion
    }
}