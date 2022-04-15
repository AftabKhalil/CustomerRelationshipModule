using ArrayToExcel;
using Data.ORMHelper;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerRelationshipModule
{
    public partial class DataLabeling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Visible = true;
            string filePath = FileUpload1.PostedFile.FileName; // getting the file path of uploaded file  
            string filename1 = Path.GetFileName(filePath); // getting the file name of uploaded file  
            string ext = Path.GetExtension(filename1); // getting the file extension of uploaded file  
            string type = String.Empty;
            IExcelDataReader reader = null;

            if (!FileUpload1.HasFile)
            {
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Text = "Please Select File"; //if file uploader has no file selected  
            }
            else
            if (FileUpload1.HasFile)
            {
                try
                {
                    switch (ext) // this switch code validate the files which allow to upload only excel file you can change it for any file  
                    {
                        case ".xls":
                            type = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            type = "application/vnd.ms-excel";
                            break;
                    }

                    if (type != String.Empty)
                    {
                        Stream fs = FileUpload1.PostedFile.InputStream;
                        if (ext == ".xls")
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(fs);
                        }
                        else if (ext == ".xlsx")
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                        }


                        int fieldcount = reader.FieldCount;
                        int rowcount = reader.RowCount;
                        DataTable dt = new DataTable();
                        DataRow row;
                        DataTable dt_ = new DataTable();
                        dt_ = reader.AsDataSet().Tables[0];
                        for (int i = 0; i < dt_.Columns.Count; i++)
                        {
                            dt.Columns.Add(dt_.Rows[0][i].ToString());
                        }
                        int rowcounter = 0;
                        for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                        {
                            row = dt.NewRow();

                            for (int col = 0; col < dt_.Columns.Count; col++)
                            {
                                row[col] = dt_.Rows[row_][col].ToString();
                                rowcounter++;
                            }
                            dt.Rows.Add(row);
                        }

                        foreach (DataRow r in dt.Rows)
                        {
                            var id = (string)r.ItemArray[0];
                            var message = (string)r.ItemArray[1];
                            var sentiment = (string)r.ItemArray[2];

                            if (int.TryParse(id, out int ID) && int.TryParse(sentiment, out int SENTIMENT))
                            {
                                new TaskAssignmentHelper().EditRating(ID, SENTIMENT);
                            }
                        }

                        Label1.ForeColor = System.Drawing.Color.Green;
                        Label1.Text = "File Uploaded Successfully";
                    }
                    else
                    {
                        Label1.ForeColor = System.Drawing.Color.Red;
                        Label1.Text = "Select Only Excel File having extension .xlsx or .xls "; // if file is other than speified extension   
                    }
                }
                catch (Exception ex)
                {
                    Label1.Text = "Error: " + ex.Message.ToString();
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = ".xlsx";
            Response.AddHeader("content-disposition", "attachment;filename=Data.xlsx");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            var dataSet = new DataSet();

            var table = new DataTable($"Data");
            dataSet.Tables.Add(table);

            table.Columns.Add($"id", typeof(int));
            table.Columns.Add($"message", typeof(string));
            table.Columns.Add($"sentiment", typeof(int));

            var messages = new TaskAssignmentHelper().GetTaskAssignmetForSentimentExcel();

            messages.ForEach(m =>
            {
                table.Rows.Add(m.id, m.message, m.sentiment);
            });

            var excel = dataSet.ToExcel();

            Response.BinaryWrite(excel);
            Response.End();
        }
    }
}