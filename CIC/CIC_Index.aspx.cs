using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
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
            if (!IsPostBack)
            {
                string CICname = Request.QueryString["NAME"];
                if (string.IsNullOrWhiteSpace(CICname))
                {
                    CIC_ImagePreview.ImageUrl = "~/Images/Pics.jpg"; //如果姓名欄位沒有輸入值，顯示預設圖片
                }
                else
                {
                    string result = GetNameFromDatabase(CICname); //如果姓名欄位有輸入值，顯示帶有姓名的圖片
                    SetTextOnImage(result);
                    CIC_ImagePreview.ImageUrl = "~/Images/CertificateWithName.jpg";
                }
            }  
        }

        #region 資料庫讀取
        private string GetNameFromDatabase(string Name)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select * From CIC_REPORT Where NAME_ZH = @name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", Name);
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

                        result = string.Format("{0}\n{1}\n{2}\n{3}\n", name, receiptList, NoList, effectiveDate, dept);
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
        private void SetTextOnImage(string objs)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("~/Images/Pics.jpg"));
            Graphics pics = Graphics.FromImage(img);

            // 設定文字大小、字型、位置和顏色
            Font fonts = new Font("微軟正黑體", 8);
            Brush brush = Brushes.Black;

            //設定文字繪製位置
            PointF point = new PointF(250, 252);
            float lineHeight = fonts.GetHeight() * 0.2f;

            //設定文字渲染模式和畫質
            pics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            pics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            //設定行間距
            string[] lines = objs.Split('\n');
            float totalHeight = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                SizeF size = pics.MeasureString(line, fonts);
                float x = point.X;
                float y = point.Y + totalHeight;
                pics.DrawString(line, fonts, brush, x, y);
                totalHeight += size.Height + lineHeight;
            }

            // 儲存繪製好的圖片
            img.Save(Server.MapPath("~/Images/CertificateWithName.jpg"));
        }
        #endregion

        #region 下載圖片
        protected void btn_DownLoad_Click(object sender, EventArgs e)
        {
            string Names= Request.Form["NameSerach"];

            if (string.IsNullOrWhiteSpace(Names))
            {
                return; // 如果姓名欄位沒有輸入值，不執行下載動作
            }
            string result = GetNameFromDatabase(Names);
            SetTextOnImage(result);

            string targetFilePath = Server.MapPath("~/Images/臨床教師證.jpg"); //儲存的目標路徑

            //傳輸檔案之前，先將檔案複製到目標路徑
            string sourceFilePath = Server.MapPath("~/Images/CertificateWithName.jpg");
            File.Copy(sourceFilePath, targetFilePath, true); //複製檔案到目標路徑
            Response.Clear();
            Response.ContentType = "image/jpeg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=臨床教師證.jpg");
            Response.TransmitFile(targetFilePath);
            Response.End();
        }
        #endregion

        #region 預覽圖片
        protected void btn_PreView_Click(object sender, EventArgs e)
        {
            string CIC_Name = Request.Form["NameSerach"];
            if (string.IsNullOrWhiteSpace(CIC_Name))
            {             
                CIC_ImagePreview.ImageUrl = "~/Images/Pics.jpg"; //如果姓名欄位沒有輸入值，顯示預設圖片
            }
            else
            {
                // 如果姓名欄位有輸入值，顯示帶有姓名的圖片
                string result = GetNameFromDatabase(CIC_Name);
                SetTextOnImage(result);
                CIC_ImagePreview.ImageUrl = "~/Images/臨床教師證.jpg";
            }
        }
        #endregion

        #region 檔案上傳
        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            // 檢查是否選擇了檔案
            if (ExcleUpload.HasFile)
            {
                // 取得檔案名稱和路徑
                string fileName = ExcleUpload.FileName;
                string filePath = Server.MapPath("~/uploads/" + fileName);

                // 上傳檔案到伺服器
                ExcleUpload.SaveAs(filePath);

                // 讀取 Excel 檔案
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // 取得最後一列的索引
                    int lastRow = sheet.LastRowNum;

                    // 逐行讀取資料
                    for (int i = 1; i <= lastRow; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        // 儲存資料的 List
                        List<string> rowData = new List<string>();

                        foreach (var cell in row)
                        {
                            string columnStr = string.Empty;

                            switch (cell.CellType)
                            {
                                case CellType.Numeric:  // 數值格式
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {   // 日期格式
                                        columnStr = cell.DateCellValue.ToString();
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

                        //取得每個儲存格的值
                        //string col1;
                        //if (row.GetCell(0).CellType == CellType.Numeric)
                        //{
                        //    col1 = row.GetCell(0).NumericCellValue.ToString();
                        //}
                        //else
                        //{
                        //    col1 = row.GetCell(0).StringCellValue;
                        //}
                        //string col2 = row.GetCell(1).StringCellValue;
                        //string col3 = row.GetCell(2).StringCellValue;
                        //string col4 = row.GetCell(3).StringCellValue;
                        //DateTime col5;
                        //if (row.GetCell(4).CellType == CellType.Numeric)
                        //{
                        //    double excelDate = row.GetCell(4).NumericCellValue;
                        //    col5 = DateTime.FromOADate(excelDate);
                        //    col5 = col5.AddYears(-1911); // 民國轉西元
                        //}
                        //else
                        //{
                        //    col5 = DateTime.ParseExact(row.GetCell(4).StringCellValue, "yyyMMdd", CultureInfo.InvariantCulture);
                        //}

                        //DateTime col6;
                        //if (row.GetCell(5).CellType == CellType.Numeric)
                        //{
                        //    double excelDate = row.GetCell(5).NumericCellValue;
                        //    col6 = DateTime.FromOADate(excelDate);
                        //    col6 = col6.AddYears(-1911); // 民國轉西元
                        //}
                        //else
                        //{
                        //    col6 = DateTime.ParseExact(row.GetCell(5).StringCellValue, "yyyMMdd", CultureInfo.InvariantCulture);
                        //}
                        //string col7 = row.GetCell(6).StringCellValue;

                        // 將資料寫入資料庫
                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString))
                        {
                            SqlCommand command = new SqlCommand("Insert Into CICBOOK (SID,EID,NAME_ZH,RECEIPT_LIST,NO_LIST,EFFECTIVE_STARTDATE,EFFECTIVE_ENDDATE,Dept) Values(@SID,@EID,@NAME_ZH,@RECEIPT_LIST,@NO_LIST,@EFFECTIVE_STARTDATE,@EFFECTIVE_ENDDATE,@Dept)", connection);


                            //// 產生流水序號，作為檔案名稱的一部分
                            string fileId = GenerateFileId();
                            //int sid = int.Parse(fileId);
                            int sid = Int32.Parse(fileId.Replace("F", ""));
                            command.Parameters.AddWithValue("@SID", sid);
                            command.Parameters.AddWithValue("@EID", rowData[0]);
                            command.Parameters.AddWithValue("@NAME_ZH", rowData[1]);
                            command.Parameters.AddWithValue("@RECEIPT_LIST", rowData[2]);
                            command.Parameters.AddWithValue("@NO_LIST", rowData[3]);
                            //DateTime effectiveStartDate;
                            //if (DateTime.TryParseExact(rowData[4], "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out effectiveStartDate))
                            //{
                            //    effectiveStartDate = effectiveStartDate.AddYears(1911); // 將年份加上 1911
                            //    command.Parameters.AddWithValue("@EFFECTIVE_STARTDATE", effectiveStartDate);
                            //}
                            //else
                            //{
                            //    command.Parameters.AddWithValue("@EFFECTIVE_STARTDATE", DBNull.Value);
                            //}

                            //DateTime effectiveEndDate;
                            //if (DateTime.TryParseExact(rowData[5], "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out effectiveEndDate))
                            //{
                            //    effectiveEndDate = effectiveEndDate.AddYears(1911); // 將年份加上 1911
                            //    command.Parameters.AddWithValue("@EFFECTIVE_ENDDATE", effectiveEndDate);
                            //}
                            //else
                            //{
                            //    command.Parameters.AddWithValue("@EFFECTIVE_ENDDATE", DBNull.Value);
                            //}
                            DateTime effectiveStartDate;
                            int effectiveStartDateInt;
                            if (int.TryParse(rowData[4], out effectiveStartDateInt))
                            {
                                if (effectiveStartDateInt < 1000000) // 民國年格式
                                {
                                    effectiveStartDateInt += 19110000;
                                }
                                effectiveStartDate = DateTime.ParseExact(effectiveStartDateInt.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                                command.Parameters.AddWithValue("@EFFECTIVE_STARTDATE", effectiveStartDate);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@EFFECTIVE_STARTDATE", DBNull.Value);
                            }

                            DateTime effectiveEndDate;
                            int effectiveEndDateInt;
                            if (int.TryParse(rowData[5], out effectiveEndDateInt))
                            {
                                if (effectiveEndDateInt < 1000000) // 民國年格式
                                {
                                    effectiveEndDateInt += 19110000;
                                }
                                effectiveEndDate = DateTime.ParseExact(effectiveEndDateInt.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                                command.Parameters.AddWithValue("@EFFECTIVE_ENDDATE", effectiveEndDate);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@EFFECTIVE_ENDDATE", DBNull.Value);
                            }
                            command.Parameters.AddWithValue("@Dept", rowData[6]);
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
        #endregion


        #region 產生流水序號
        private string GenerateFileId()
        {
            //int fileId = 0;
            string fileId = "";
            // 這裡假設流水序號的格式為 "F0001"、"F0002"、"F0003"......
            // 若要修改格式，可自行調整以下程式碼
            int nextId = 1;

            string lastId = (string)Application["LastFileId"];

            if (lastId != null)
            {
                int lastIdInt = 0;
                nextId = int.Parse(lastId.Substring(1)) +1;
            }
            fileId = "F" + nextId.ToString("0000");
            //fileId = nextId;
            // 更新 Application 變數中的 LastFileId
            //Application["LastFileId"] = fileId;
            Application["LastFileId"] = fileId.ToString(); // 將 fileId 轉換成字串
            return fileId;
        }
        #endregion

        #region 上傳

        protected void btn_UP_Click(object sender, EventArgs e)
        {
            string msg = ""; // 宣告msg，確保一定有初始化值
            // 取得檔案名稱和路徑
            string fileName = ExcleUpload.FileName;
            string filePath = Server.MapPath("~/uploads/" + fileName);
            IWorkbook workbook;
            IFormulaEvaluator formulaEvaluator;
            
            using (FileStream filex = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string extension = Path.GetExtension(filePath);
                if (extension == ".xls")
                {   // xls
                    workbook = new HSSFWorkbook(filex);
                    formulaEvaluator = new HSSFFormulaEvaluator(workbook); // Important!! 取公式值的時候會用到
                }
                else if (extension == ".xlsx")
                {   // xlsx
                    workbook = new XSSFWorkbook(filex);
                    formulaEvaluator = new XSSFFormulaEvaluator(workbook); // Important!! 取公式值的時候會用到
                }
                else
                {
                    msg = "檔案格式錯誤。";
                    return; // 若檔案格式錯誤，直接回傳即可
                }
            }

            var st = workbook.GetSheetAt(0);

            for (int i = 0; i <= st.LastRowNum; i++)
            {
                var row = st.GetRow(i);

                foreach (var cell in row.Cells)
                {
                    string columnStr = string.Empty;

                    switch (cell.CellType)
                    {
                        case CellType.Numeric:  // 數值格式
                            if (DateUtil.IsCellDateFormatted(cell))
                            {   // 日期格式
                                columnStr = cell.DateCellValue.ToString();
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
                            var formulaValue = formulaEvaluator.Evaluate(cell);
                            if (formulaValue.CellType == CellType.String) columnStr = formulaValue.StringValue.ToString();          // 執行公式後的值為字串型態
                            else if (formulaValue.CellType == CellType.Numeric) columnStr = formulaValue.NumberValue.ToString();    // 執行公式後的值為數字型態
                            break;
                        default:
                            break;
                    }
                }
            }
    }
        #endregion
    }
}