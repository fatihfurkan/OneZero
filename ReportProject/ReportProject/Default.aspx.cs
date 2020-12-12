using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportProject
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportList = new List<Report>();
                FormulList = new List<Formul>();

               readXLS(Server.MapPath("~/files/FORMUL-GIRDI.xlsx"));
            }
        }

        #region ViewState

        private List<Report> ReportList
        {
            get
            {
                List<Report> item = (List<Report>)ViewState["ReportList"];
                if (item != null)
                    return item;
                else
                    return null;
            }
            set
            {
                ViewState["ReportList"] = value;
            }
        }
        private List<Formul> FormulList
        {
            get
            {
                List<Formul> item = (List<Formul>)ViewState["FormulList"];
                if (item != null)
                    return item;
                else
                    return null;
            }
            set
            {
                ViewState["FormulList"] = value;
            }
        }
        private int MaxDesi
        {
            get
            {
                int item = (int)ViewState["MaxDesi"];
                if (item != null)
                    return item;
                else
                    return 0;
            }
            set
            {
                ViewState["MaxDesi"] = value;
            }
        }
        #endregion
        #region Function
        public void readXLS(string FilePath)
        {
            FileInfo existingFile = new FileInfo(FilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    string[] words = worksheet.Cells[row, 1].Value.ToString().Trim().Split('-');
                    FormulList.Add(new Formul
                    {
                        DesiMin = words.Count() == 2 ? int.Parse(words[0]) : -1,//30 dan büyük ise DesiMin ve DesiMaxı -1 yap
                        DesiMax = words.Count() == 2 ? int.Parse(words[1]) : -1,
                        KSY = decimal.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                        UO = decimal.Parse(worksheet.Cells[row, 3].Value.ToString().Trim())

                    });
                }
                MaxDesi = FormulList.Max(x => x.DesiMax);
            
            }
        }
        public decimal calculatePrice(int adet, int kgdesi, int mesafe)
        {
            decimal price = 0;
            if (kgdesi > MaxDesi)//30 dan büyük ise
            {
                price = (FormulList.Where(x => x.DesiMax == MaxDesi).Select(a => mesafe == 0 ? a.KSY : a.UO).First() +
                         FormulList.Where(x => x.DesiMax == -1).Select(a => mesafe == 0 ? a.KSY : a.UO).First() * (kgdesi - MaxDesi)) * adet;
            }
            else
            {
                price = FormulList.Where(x => x.DesiMin <= kgdesi && x.DesiMax >= kgdesi).Select(a => mesafe == 0 ? a.KSY : a.UO).First() * adet;

            }
            return price;
        }

        #endregion
        #region Button

        protected void btnReport_Click(object sender, EventArgs e)
        {
            FileInfo existingFile = new FileInfo(Server.MapPath("~/files/EKSTRE-GIRDI.xlsx"));
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                package.Workbook.Worksheets[1].Name = "İşlem Sonucu";
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.End.Row;
                int sıraNo, adet, kgDesi, mesafeType;
                string mesafe;
                worksheet.Cells[1, 5].Value = "ÜCRET";
                for (int row = 2; row <= rowCount; row++)
                {
                    sıraNo = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                    adet = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim());
                    kgDesi = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim());
                    mesafe = worksheet.Cells[row, 4].Value.ToString().Trim();
                    mesafeType = mesafe == "ORTA" || mesafe == "UZAK" ? 1 : 0;
                    worksheet.Cells[row, 5].Value = calculatePrice(adet, kgDesi, mesafeType);

                    ReportList.Add(new Report
                    {
                        SıraNo = sıraNo,
                        Adet = adet,
                        KgDesi = kgDesi,
                        Mesafe = mesafe,
                        Ucret = decimal.Parse(worksheet.Cells[row, 5].Value.ToString().Trim())
                    });
                }
                
                ExcelWorksheet worksheetPivot = package.Workbook.Worksheets.Add("Pivot Rapor");

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#d9e1f2");
                worksheetPivot.Cells[2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPivot.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);
                worksheetPivot.Cells[2, 2].Style.Font.Bold = true;
                worksheetPivot.Cells[2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPivot.Cells[2, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);
                worksheetPivot.Cells[2, 3].Style.Font.Bold = true;
                worksheetPivot.Cells[8, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPivot.Cells[8, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);
                worksheetPivot.Cells[8, 2].Style.Font.Bold = true;
                worksheetPivot.Cells[8, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPivot.Cells[8, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);
                worksheetPivot.Cells[8, 3].Style.Font.Bold = true;
                worksheetPivot.Cells[2, 2].Value = "Mesafe";
                worksheetPivot.Cells[2, 3].Value = "Kargo Adedi";
                worksheetPivot.Cells[8, 2].Value = "Grand Total";

                var mesafeList = ReportList.GroupBy(a => a.Mesafe)
                                .Select(r => new Report { Mesafe = r.Key, Adet = r.Sum(x => x.Adet) })
                                .ToList().OrderBy(x => x.Adet);
                int rowPivot = 3;
                int grandTotal = 0;
                foreach (var item in mesafeList)
                {
                    worksheetPivot.Cells[rowPivot, 2].Value = item.Mesafe;
                    worksheetPivot.Cells[rowPivot, 3].Value = item.Adet;
                    rowPivot++;
                    grandTotal += item.Adet;
                }
                worksheetPivot.Cells[8, 3].Value = grandTotal;

                worksheetPivot.Column(1).AutoFit();
                worksheetPivot.Column(2).AutoFit();
                worksheetPivot.Column(3).AutoFit();

                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();
                worksheet.Column(5).AutoFit();
                byte[] bin = package.GetAsByteArray();

                Response.ClearHeaders();
                Response.Clear();
                Response.Buffer = true;

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                Response.AddHeader("content-length", bin.Length.ToString());

                Response.AddHeader("content-disposition", "attachment; filename=\"RAPOR.xlsx\"");

                Response.OutputStream.Write(bin, 0, bin.Length);

                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {

                if (System.IO.Path.GetExtension(FileUploadId.FileName) != ".xlsx")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Lütfen .xlsx Frormatında Bir Dosyayı Seçiniz!" + "');", true);
                    return;
                }


                if (FileUploadId.HasFile)
                {
                    FileInfo file = new FileInfo(Server.MapPath("~/files/EKSTRE-GIRDI.xlsx"));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    FileUploadId.PostedFile.SaveAs(Server.MapPath("~/files/EKSTRE-GIRDI.xlsx"));
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Ekstre Dosyası Başarıyla Yüklendi!" + "');", true);

                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void btnUploadFormul_Click(object sender, EventArgs e)
        {
            try
            {

                if (System.IO.Path.GetExtension(FileUpload1.FileName) != ".xlsx")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Lütfen .xlsx Frormatında Bir Dosyayı Seçiniz!" + "');", true);
                    return;
                }


                if (FileUpload1.HasFile)
                {
                    FileInfo file = new FileInfo(Server.MapPath("~/files/FORMUL-GIRDI.xlsx"));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("~/files/FORMUL-GIRDI.xlsx"));
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Formül Dosyası Başarıyla Yüklendi!" + "');", true);

                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}