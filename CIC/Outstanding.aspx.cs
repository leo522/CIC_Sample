﻿using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIC
{
    public partial class Outstanding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayImage();
            }
        }

        #region 直接顯示圖片

        private void DisplayImage()
        {
            try
            {
                int imageId = 1; // 設置要顯示的圖片ID
                string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
                string LoadPic = "Select FileName, ContentType, ImageData FROM Images WHERE Id = @ImageId";

                using (SqlConnection conn = new SqlConnection(con))
                {
                    SqlCommand command = new SqlCommand(LoadPic, conn);
                    command.Parameters.AddWithValue("@ImageId", imageId);

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string fileName = reader["FileName"].ToString();
                        string contentType = reader["ContentType"].ToString();
                        byte[] imageData = (byte[])reader["ImageData"];

                        // 設置響應頭信息
                        Response.Clear();
                        Response.ContentType = contentType;
                        Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);

                        // 輸出圖片數據
                        Response.BinaryWrite(imageData);
                        Response.End();
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 上傳圖片

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploads.HasFiles)
                {
                    foreach (HttpPostedFile file in FileUploads.PostedFiles)
                    {
                        string fileName = Path.GetFileName(file.FileName); //獲取文件訊息
                        string contentType = file.ContentType;

                        if (IsAllowedFileType(contentType)) //檢查文件類型
                        {
                            byte[] fileData = new byte[file.ContentLength];
                            file.InputStream.Read(fileData, 0, file.ContentLength);
                            string fileDataAsString = Encoding.Default.GetString(fileData);

                            SaveFileToDataBase(fileData, contentType, fileDataAsString, contentType);
                            StatusLabel.Text = "文件上傳成功";
                        }
                        else
                        {
                            StatusLabel.Text = "文件格式錯誤";
                        }
                    }
                }
                else
                {
                    StatusLabel.Text = "請選擇要上傳的文件";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //if (FileUploads.HasFiles)
        //{
        //    try
        //    {
        //        string fileName = FileUploads.FileName;
        //        string fileExtension = Path.GetExtension(fileName).ToLower();

        //        if (IsValidImageExtension(fileExtension)) //驗證上傳圖片格式
        //        {
        //            string contentType = FileUploads.PostedFile.ContentType;
        //            byte[] pics = FileUploads.FileBytes;

        //            //檢查圖片是否已存在相同
        //            if (!IsImageExists(pics))
        //            {
        //                string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
        //                using (SqlConnection conn = new SqlConnection(con))
        //                {
        //                    string InsertPic = "Insert Into Images(FileName, ContentType, ImageData) VALUES (@FileName, @ContentType, @ImageData)";

        //                    SqlCommand command = new SqlCommand(InsertPic, conn);
        //                    command.Parameters.AddWithValue("@FileName", fileName);
        //                    command.Parameters.AddWithValue("@ContentType", contentType);
        //                    command.Parameters.AddWithValue("@ImageData", pics);

        //                    conn.Open();
        //                    command.ExecuteNonQuery();
        //                    conn.Close();
        //                }
        //                StatusLabel.Text = "圖片上傳成功";
        //            }
        //            else
        //            {
        //                StatusLabel.Text = "已存在相同圖片";
        //            }
        //        }
        //        else
        //        {
        //            StatusLabel.Text = "只允許上傳 PNG、JPG、JPEG 圖片格式！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusLabel.Text = "檔案上傳失敗：" + ex.Message;
        //    }
        //}
        //else
        //{
        //    StatusLabel.Text = "請選擇要上傳的圖片！";
        //}
        private bool IsAllowedFileType(string contentType) //檢查文件類型是否允許上傳
        {
            string[] allowedTypes = { "image/png", "image/jpeg", "image/jpg", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/pdf" }; // 可根據需求添加或修改允許的文件類型
            return allowedTypes.Contains(contentType);
        }

        private void SaveFileToDataBase(byte[] imageFile, string imageContentType, string FilesData, string fileContentType)
        {
            string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
            string InsertPic = "INSERT INTO OutstandindData (ImagesData, ContentType, FilesData, FilesContentType) VALUES (@ImagesData, @ContentType, CONVERT(varbinary(max), @FilesData), @FilesContentType)"; 

            using (SqlConnection conn = new SqlConnection(con))
            {
                SqlCommand command = new SqlCommand(InsertPic, conn);
                command.Parameters.AddWithValue("@ImagesData", imageFile);
                command.Parameters.AddWithValue("@ContentType", imageContentType);
                command.Parameters.AddWithValue("@FilesData", FilesData);
                command.Parameters.AddWithValue("@FilesContentType", fileContentType);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        private bool IsImageExists(byte[] imageData) //檢查圖片是否重覆
        { 
            string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "Select Count(*) From Images Where ImageData = @ImageData";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@ImageData", imageData);

                conn.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                conn.Close() ;

                return count > 0;
            }
        }

        private bool IsValidImageExtension(string fileExtension) //限制圖片上傳格式
        {
            string[] validExtensions = { ".png", ".jpg", ".jpeg" };
            return validExtensions.Contains(fileExtension);
        }
        #endregion

        #region 讀取圖片
        protected void btn_Pic_Click(object sender, EventArgs e)
        {
            try
            {
                int imageID;

                if (int.TryParse(ImageIdTextBox.Text, out imageID))
                {
                    DisplayImages(imageID);
                }
                else
                {
                    StatusLabel.Text = "請輸入有效的圖片ID";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisplayImages(int imageID) //顯示圖片
        {
            string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
            string LoadPic = "Select FileName, ContentType, ImageData FROM Images WHERE Id = @ImageId";

            using (SqlConnection conn = new SqlConnection(con))
            {
                SqlCommand command = new SqlCommand(LoadPic, conn);
                command.Parameters.AddWithValue("@ImageId", imageID);

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string fileName = reader["FileName"].ToString();
                    string contentType = reader["ContentType"].ToString();
                    byte[] imageData = (byte[])reader["ImageData"];

                    // 設置響應頭信息
                    Response.Clear();
                    Response.ContentType = contentType;
                   // Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);

                    // 輸出圖片數據
                    Response.BinaryWrite(imageData);
                    Response.End();
                }
                else
                {
                    StatusLabelPic.Text = "無此圖片";
                }
                reader.Close();
                conn.Close();
            }
        }
        #endregion

        #region 存檔功能

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string source = RBSource.SelectedValue;
            //    string itemName = Request.Form["Item"];
            //    string support = Request.Form["SupportItem"];
            //    string dateDayValue = Request.Form["DateDay"];
            //    DateTime activeDate;
            //    string type = RBType.SelectedValue;

            //    string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
            //    using (SqlConnection conn = new SqlConnection(con))
            //    {
            //        string query = "Insert Into OutstandindData (RBsource, Awards, Organization, ActiveDates, RBtype) Values (@RBsource, @Awards, @Organization, @ActiveDates, @RBtype)";

            //        using (SqlCommand comm = new SqlCommand(query, conn))
            //        {
            //            comm.Parameters.AddWithValue("@RBsource", source);
            //            comm.Parameters.AddWithValue("@Awards", itemName);
            //            comm.Parameters.AddWithValue("@Organization", support);
            //            comm.Parameters.AddWithValue("@ActiveDates", dateDayValue);
            //            comm.Parameters.AddWithValue("@RBtype", type);

            //            conn.Open();
            //            comm.ExecuteNonQuery();
            //        }
            //    }
            //    StatusMessage.Text = "存檔成功";
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            try
            {
                string source = RBSource.SelectedValue;
                string itemName = Request.Form["Item"];
                string support = Request.Form["SupportItem"];
                string dateDayValue = Request.Form["DateDay"];
                DateTime activeDate;
                string type = RBType.SelectedValue;

                string fileName = FileUploads.FileName;
                string fileExtension = Path.GetExtension(fileName).ToLower();
                if (FileUploads.HasFiles)
                {
                    if (IsAllowedFileType(fileExtension)) //检查文件类型
                    {
                        string contentType = FileUploads.PostedFile.ContentType;
                        byte[] imageFile = FileUploads.FileBytes;
                        string fileData = Encoding.Default.GetString(imageFile);

                        string con = WebConfigurationManager.ConnectionStrings["CIC_ReportEntities"].ConnectionString;
                        using (SqlConnection conn = new SqlConnection(con))
                        {
                            string query = "INSERT INTO OutstandindData (RBsource, Awards, Organization, ActiveDates, RBtype, ImagesData, ContentType, FilesData, FilesContentType) VALUES (@RBsource, @Awards, @Organization, @ActiveDates, @RBtype, @ImagesData, @ContentType, CONVERT(varbinary(max), @FilesData), @FilesContentType)";

                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@RBsource", source);
                                comm.Parameters.AddWithValue("@Awards", itemName);
                                comm.Parameters.AddWithValue("@Organization", support);
                                comm.Parameters.AddWithValue("@ActiveDates", dateDayValue);
                                comm.Parameters.AddWithValue("@RBtype", type);
                                comm.Parameters.AddWithValue("@ImagesData", imageFile);
                                comm.Parameters.AddWithValue("@ContentType", contentType);
                                comm.Parameters.AddWithValue("@FilesData", fileData);
                                comm.Parameters.AddWithValue("@FilesContentType", fileExtension);

                                conn.Open();
                                comm.ExecuteNonQuery();
                            }
                        }
                    }
                }

                StatusMessage.Text = "存檔成功";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}