
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Patagames.Pdf.Net.Controls.WinForms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ConvertidorImagenes
{
    public partial class Lector : Form
    {
        private static OCR ADM = OCR.getadm();
        string Ruta = "";
        RichTextBox txt1 = null;
        Label lbb1 = null; Label lbb2=null; ProgressBar pbb = null; Button btt1;
        public Lector(string ruta, RichTextBox txt2,Label lb1,Label lb2, ProgressBar pb, Button bt1)
        {
            InitializeComponent();
            print(ruta);
            Ruta = ruta;
            txt1=txt2;
            lbb1 = lb1;
            lbb2 = lb2;
            pbb = pb;
            btt1 = bt1;

        }

        private void Lector_Load(object sender, EventArgs e)
        {

        }
        public void print(string ruta)
        {
            
                pdfViewer1.LoadDocument(ruta);
            
            //,String inipag, String finpag
        }

        private void pdfToolStripPages1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public void render(RichTextBox txt)
        {
            int pagini = 1;
            var text = new System.Text.StringBuilder();
           
            using (var pdfReader = new PdfReader(Ruta))
            {
               
                for (pagini = 1; pagini <= pdfReader.NumberOfPages; pagini++)
                {
                    pdfReader.SelectPages(txtini.Text+"-"+textBox2.Text);
                   
                    var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, pagini, new LocationTextExtractionStrategy());

                  
                    text.Append(currentText);
                   
                    pbb.Value = 100;
                    lbb2.Text = "Finalizo!";
                    lbb1.Text = "Finalizado!";
                }
            }
            btt1.Enabled = true;
            txt.Text = text.ToString();
         

        }
        private void acepag_Click(object sender, EventArgs e)
        {
            if  (int.TryParse(txtini.Text, out int num) && int.TryParse(textBox2.Text, out int num2)&&num==num2)
                {
                    textBox2.Text = "";
                txtini.Text = "";

            }
            if (int.TryParse(txtini.Text, out int num3) && int.TryParse(textBox2.Text, out int num4) && num4 < num3)
            {
                textBox2.Text = "";
                txtini.Text = "";

            }
            if (int.TryParse(txtini.Text, out int num5) && int.TryParse(textBox2.Text, out int num6) && num5< num6)
            {
                render(txt1);

                this.Hide();
            }
            
            

        }

        private void txtini_KeyPress(object sender, KeyPressEventArgs e)
        {
            
           int.TryParse(txtini.Text, out int num2);
           


            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
         
            if (num2<1||num2>=pdfViewer1.Document.Pages.Count)
            {
                txtini.Text = "";
            }
            
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            int.TryParse(textBox2.Text, out int num);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (num < 1 || num >= pdfViewer1.Document.Pages.Count)
            {
                textBox2.Text = "";
            }

        }


        private void txtini_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btcancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
