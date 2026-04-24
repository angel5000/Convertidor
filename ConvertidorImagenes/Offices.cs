
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;
using System;
using org.apache.pdfbox.pdmodel;
using NPOI.XWPF.UserModel;
using org.apache.pdfbox.util;
using SautinSoft;
using SautinSoft.Document;
namespace ConvertidorImagenes
{
    public partial class Offices : Form
    {
        String nombreft = "", nombreft2 = "";
        int contador = 0;
        bool apdf = true;
        String rtdestino = @"C:\Users\"+Environment.UserName+@"\Desktop",nombarch="";
        public Offices()
        {
            InitializeComponent();
           
            String[] ftof = { "WORD", "EXCEL", "POWERPOINT" };
            String[] ftof2 = { "WORD", "EXCEL", "POWERPOINT" };
            foreach (String dt in ftof)
            {
                cbof.Items.Add(dt);
            }
            foreach (String dt2 in ftof2)
            {
                cbpdf.Items.Add(dt2);
            }
            cbof.SelectedItem = "WORD";
            cbpdf.SelectedItem = "WORD";
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ofapdf_CheckedChanged(object sender, EventArgs e)
        {
            if (ofapdf.Checked)
            {
                panel4.Enabled = true;
                panel5.Enabled = false;
                apdf = true;
            }
           // MessageBox.Show("" + apdf);
        }

        private void pdfaof_CheckedChanged(object sender, EventArgs e)
        {
            if (pdfaof.Checked)
            {
                panel5.Enabled = true;
                panel4.Enabled = false;
                apdf = false;
            }
           // MessageBox.Show("" + apdf);
        }

        private void Offices_Load(object sender, EventArgs e)
        {
            panel2.AllowDrop = true;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            panel2.Enabled = false;
            cbof.Enabled = false;

            bar2.Value = 20;
            var im = e.Data.GetData(DataFormats.FileDrop);

            

            if (im != null)
            {
                var nombre = im as string[];
                if (nombre.Length > 0 && apdf)
                {
                    lbro.Text = nombre[0];
                    nombarch = System.IO.Path.GetFileName(nombre[0]);
                    contador = 40;
                    bar2.Value = contador;
                    convertirpdf(nombreft, nombre[0]);
                   
                }
                if (nombre.Length > 0 && apdf==false)
                {
                    lbro.Text = nombre[0];
                    nombarch = System.IO.Path.GetFileName(nombre[0]);
                    contador = 40;
                    bar2.Value = contador;
                    convertirAdoc(nombre[0],nombreft2);

                }

            }
        }

        public void convertirpdf(String formato, string ruta)
        {
            switch (formato)
            {
                case "WORD":
                    {
                        contador = 60;
                        bar2.Value = contador;
                        string pdfFilePath = @"C:\Users\angeldvp\Desktop\convertido.pdf";
                        var wordApp = new Microsoft.Office.Interop.Word.Application();
                        var wordDocument = wordApp.Documents.Open(ruta);
                        contador = 80;
                        bar2.Value = contador;
                       wordDocument.ExportAsFixedFormat(rtdestino+@"\"+nombarch+ ".pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

                        wordDocument.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                                           Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                                           false);
                        bar2.Value = 100;
                        wordApp.Quit();

                    }
                    break;
                case "EXCEL":
                    {
                        contador = 60;
                        bar2.Value = contador;
                       // string pdfFilePath = @"C:\Users\angeldvp\Desktop\convertido.pdf";
                        var excelApp = new Microsoft.Office.Interop.Excel.Application();

                        var excelDocument = excelApp.Workbooks.Open(ruta);

                        excelDocument.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF,
                                                          rtdestino + @"\" + nombarch+".pdf");

                        excelDocument.Close(false, "", false); //Close document
                        excelApp.Quit(); //Important: When you forget this Excel keeps running in the background
                        bar2.Value = 100;
                    }
                    break;
                case "POWERPOINT":
                    {
                        contador = 60;
                        bar2.Value = contador;
                       // string pdfFilePath = @"C:\Users\angeldvp\Desktop\convertido.pdf";
                        var powerpointApp = new Microsoft.Office.Interop.PowerPoint.Application();

                        var powerpointDocument = powerpointApp.Presentations.Open(ruta,
                                        Microsoft.Office.Core.MsoTriState.msoTrue, //ReadOnly
                                        Microsoft.Office.Core.MsoTriState.msoFalse, //Untitled
                                        Microsoft.Office.Core.MsoTriState.msoFalse); //Window not visible during converting

                        powerpointDocument.ExportAsFixedFormat(rtdestino + @"\" + nombarch+".pdf",
                                        Microsoft.Office.Interop.PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF);

                        powerpointDocument.Close(); //Close document
                        powerpointApp.Quit(); //Important: When you forget this PowerPoint keeps running in the background


                        bar2.Value = 100;
                    }
                    break;
            }


        }

        public void convertirAdoc(string ruta, String formato)
        {
            switch (formato)
            {
                case "WORD":
                    {
                        PdfFocus pdfFocus = new PdfFocus();
                        pdfFocus.OpenPdf(ruta);

                        // Convertir el documento a formato DOCX
                        pdfFocus.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Docx;
                       int resultado = pdfFocus.ToWord(rtdestino+@"doc.docx");
                        if (resultado == 0)
                        {
                            System.Diagnostics.Process.Start(rtdestino + @"doc.docx");
                        }
                 
                        // Liberar recursos
                        pdfFocus.ClosePdf();
                        bar2.Value = 100;


                    }
                    break;
                case "EXCEL":
                    {
                        //string pathToPdf = @"..\..\Table.pdf";
            string pathToExcel = Path.ChangeExtension(ruta, ".xls");

            // Convert PDF file to Excel file
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            
	    	
            f.ExcelOptions.ConvertNonTabularDataToSpreadsheet = true;
          
            f.ExcelOptions.PreservePageLayout = true;

          
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            ci.NumberFormat.NumberDecimalSeparator = ",";
            ci.NumberFormat.NumberGroupSeparator = ".";
            f.ExcelOptions.CultureInfo = ci;

            f.OpenPdf(ruta);

            if (f.PageCount > 0)
            {
                int result = f.ToExcel(rtdestino+@"\doc.xls");
                
                //Open a produced Excel workbook
                if (result==0)
                {
                    System.Diagnostics.Process.Start(rtdestino + @"\doc.xls");
                }
            }
                        bar2.Value = 100;


                    }
                    break;
            }
            
        }

        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).Equals(".docx")&&nombreft.Equals("WORD")&& apdf)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    if (Path.GetExtension(file).Equals(".xlsx")&& nombreft.Equals("EXCEL")&&apdf )
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    if (Path.GetExtension(file).Equals(".pptx")&&nombreft.Equals("POWERPOINT")&&apdf)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    if (Path.GetExtension(file).Equals(".pdf")&&nombreft2.Equals("WORD")&&apdf==false)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    
                    if (Path.GetExtension(file).Equals(".pdf") && nombreft2.Equals("EXCEL") && apdf == false)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    if (Path.GetExtension(file).Equals(".pdf") && nombreft2.Equals("POWERPOINT") && apdf == false)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    
                   
                }
                e.Effect = DragDropEffects.None;
                panel2.Cursor = Cursors.No;
            }
           
        }

        private void cbof_SelectedIndexChanged(object sender, EventArgs e)
        {
            nombreft= cbof.SelectedItem.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void cbpdf_SelectedIndexChanged(object sender, EventArgs e)
        {
            nombreft2 = cbpdf.SelectedItem.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Enabled = true;
            cbof.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Enabled = true;
            cbof.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog rutaguarda = new FolderBrowserDialog();

            rutaguarda.Description = "Seleccione carpeta de destino";
            rutaguarda.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult resultado = rutaguarda.ShowDialog();
            if (resultado == DialogResult.OK)
            {
              //  lbsms.Dispose();
                // Obtiene la ruta de guardado seleccionada
                lbdt.Text = rutaguarda.SelectedPath;
                rtdestino = rutaguarda.SelectedPath;
               
            }
        }
    }
}
