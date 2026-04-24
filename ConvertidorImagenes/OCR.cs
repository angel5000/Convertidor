using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Patagames.Pdf.Net.Controls.WinForms;
using Patagames.Pdf.Net;

namespace ConvertidorImagenes
{
    public partial class OCR : Form
    {
        String nombreft="", rtdestino = @"C:\Users\" + Environment.UserName + @"\Desktop",ext="";
        string ruta2="";
        private static OCR Adm = null;
        public  bool img = true, pdf = false, paginas = false, ingresado = false, visor = false,completado=false;
        int pagini = 1;
        
        PictureBox pictureBox ;
        
        public OCR()
        {
            InitializeComponent();
            btconver.Enabled = false;
            txtresult.ReadOnly = true;
            CheckForIllegalCrossThreadCalls = false;
            pictureBox = new PictureBox();
            btlect.Enabled = false;
            btcan.Enabled = false;
            btpegar.Enabled = false;
            this.KeyPreview = true;
          
        }
        public static OCR getadm()
        {
            if (Adm == null)
            {
                Adm = new OCR();
            }
            return Adm;
        }
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {

                    if (System.IO.Path.GetExtension(file).Equals(".jpg")&&img || System.IO.Path.GetExtension(file).Equals(".jpeg") && img ||
                   System.IO.Path.GetExtension(file).Equals(".png") && img || System.IO.Path.GetExtension(file).Equals(".gif") && img ||
                    System.IO.Path.GetExtension(file).Equals(".bmp") && img || System.IO.Path.GetExtension(file).Equals(".PNG") && img)
                    {
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                    if (System.IO.Path.GetExtension(file).Equals(".pdf")&&pdf){
                        e.Effect = DragDropEffects.Copy;
                        panel2.Cursor = Cursors.Hand;
                        return;
                    }
                }
             
            }
           
        }


        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
           
            
            var im = e.Data.GetData(DataFormats.FileDrop);
           
            if (im != null)
            {
               

                var nombre = im as string[];
                
                   
                    if (nombre.Length > 0&&img)
                    {
                    Task.Factory.StartNew(() =>
                    {
                        lbbar.Text = "Analizando...";
                        label2.Text = "Archivo Cargado...";
                        panel2.Enabled = false;
                        bar3.Value = 50;
                        using (var image = Image.FromFile(nombre[0]))
                        {
                           
                            pictureBox = new PictureBox();
                            int width = 320;
                            float aspectRatio = (float)image.Height / (float)image.Width;


                            int height = (int)(width * aspectRatio);
                            Image resizedImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                            pictureBox.Image = resizedImage;
                           
                                
                                pictureBox.Size = new Size(width, height);
                            int x = (panel2.Width - pictureBox.Width) / 2;
                            int y = (panel2.Height - pictureBox.Height) / 2;

                            pictureBox.Location = new Point(x, y);
                            panel1.Invoke(new Action(() => {
                                panel2.Controls.Add(pictureBox);
                            }));
                        }
                });
            }
               
                
                    if (nombre.Length > 0)
                    {

                    Task.Factory.StartNew(() =>
                    {
                        
                        foreach (String file in nombre)
                        {
                           
                         
                                ext = file.Substring(file.LastIndexOf(".") + 1);
                                bar3.Value = 70;
                                lbbar.Text = "Leyendo...";
                                lbruta.Text = nombre[0];
                                lbsms2.Dispose();
                            ingresado = true;
                                convertir(img, pdf, nombre[0]);
                            
                        }


                    });


                }
               


            }
        }

        public void convertir(bool img, bool pdf, string ruta)
        {

            var tempFolder = System.IO.Path.GetTempPath();
            if (img && pdf == false)
            {

                using (var image = Image.FromFile(ruta))
                {
                    if (image.Height > 500)
                    {
                        var newWidth = (int)((double)image.Width / image.Height * 400);
                        var resizedImage = ResizeImage(image, newWidth, 400);
                        var tempFilename = System.IO.Path.Combine(tempFolder, $"{Guid.NewGuid()}" + "." + ext);

                        if (ext.Equals("jpg"))
                        {
                            resizedImage.Save(tempFilename, ImageFormat.Jpeg);
                        }
                        if (ext.Equals("png"))
                        {
                            resizedImage.Save(tempFilename, ImageFormat.Png);
                        }
                        try
                        {
                            using (var api = OcrApi.Create())
                            {
                                bar3.Value = 90;

                                api.Init(Languages.Spanish);

                                string plainText = api.GetTextFromImage(tempFilename);
                                bar3.Value = 100;
                                txtresult.Text = plainText;

                                btconver.Enabled = true;
                                File.Delete(tempFilename);

                            }

                            lbbar.Text = "Finalizo!";
                           
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error! Verifique que la imagen contenga texto.");
                            lbbar.Text = "Error de Lectura!";
                            label2.Text = "Error de Lectura!";
                            bar3.Value = 0;
                            btconver.Enabled = true;
                        }
                        }
                    else
                    {
                        try
                        {
                            using (var api = OcrApi.Create())
                            {
                                bar3.Value = 90;
                                api.Init(Languages.Spanish);

                                string plainText = api.GetTextFromImage(ruta);

                                bar3.Value = 100;
                                txtresult.Text = plainText;

                                btconver.Enabled = true;
                                lbbar.Text = "Finalizo!";
                               

                            }
                        }catch(Exception e){
                            MessageBox.Show("Error! Verifique que la imagen contenga texto.");
                            lbbar.Text = "Error de Lectura!";
                            label2.Text = "Error de Lectura!";
                            bar3.Value = 0;
                            btconver.Enabled = true;
                        }

                       
                    }


                }


            }
            if (img==false && pdf)
            {
                if (ingresado&&paginas)
                {
                    
                    ruta2 = ruta;
                    btlect.Enabled = true;
                    label2.Text = "Seleccione la cantidad de Paginas a Escanear";
                    lbbar.Text = "A espera de Paginas...";
                    btcan.Enabled = true;
                    }
                


                if (ingresado&&paginas==false)
                {

                    var text = new System.Text.StringBuilder();

                    // Crear un objeto PdfReader para leer el archivo PDF
                    using (var pdfReader = new PdfReader(ruta))
                    {
                        // Recorrer cada página del PDF y extraer el texto
                        for (pagini = 1; pagini <= pdfReader.NumberOfPages; pagini++)
                        {
                            // Utilizar el objeto PdfTextExtractor para extraer el texto de la página actual
                            var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, pagini, new LocationTextExtractionStrategy());

                            // Agregar el texto extraído al objeto StringBuilder
                            text.Append(currentText);
                            bar3.Value = 100;
                            lbbar.Text = "Finalizo!";
                            label2.Text = "Finalizo!";
                        }

                    }
                    btconver.Enabled = true;
                    txtresult.Text = text.ToString();
                }
            }
           
            

        }

        public static Image ResizeImage(Image image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
            }
            return resizedImage;
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (img && pdf == false)
            {
                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Filter = "Archivos PNG (*.png)|*.png|Archivos JPG (*.jpg)|*.jpg|Archivos GIF (*.gif)|*.gif" +
              "|Archivos BMP (*.bmp)|*.bmp|Archivos JPEG (*.jpeg)|*.jpeg";
                abrir.Title = "Selecciona una iamgen a convertir...";
                DialogResult resultado = abrir.ShowDialog();
                if (resultado == DialogResult.OK)
                {
                    label2.Text = "Archivo Cargado...";
                   
                    String ruta = abrir.FileName;
                    bar3.Value = 50;

                    string[] files = { ruta };

                    Task.Factory.StartNew(() =>
                    {
                        btconver.Enabled = false;
                        lbbar.Text = "Analizando...";
                       
                        panel2.Enabled = false;
                        bar3.Value = 50;
                        using (var image = Image.FromFile(ruta))
                        {

                            pictureBox = new PictureBox();
                            int width = 320;
                            float aspectRatio = (float)image.Height / (float)image.Width;


                            int height = (int)(width * aspectRatio);
                            Image resizedImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                            pictureBox.Image = resizedImage;


                            pictureBox.Size = new Size(width, height);
                            int x = (panel2.Width - pictureBox.Width) / 2;
                            int y = (panel2.Height - pictureBox.Height) / 2;

                            pictureBox.Location = new Point(x, y);
                            panel1.Invoke(new Action(() =>
                            {
                                panel2.Controls.Add(pictureBox);
                            }));
                        }
                    });

                    Task.Factory.StartNew(() =>
                    {
                        if (ruta.Length > 0)
                    {
                        bar3.Value = 70;
                        btconver.Enabled = true;
                       
                        lbruta.Text = ruta;
                        convertir(img, pdf, ruta);

                    }
                    });
                }
            }
        
            if (img==false&&pdf)
            {
                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Filter = "Archivos PDF (*.pdf)|*.pdf";
                abrir.Title = "Selecciona un PDF a convertir...";
                DialogResult resultado = abrir.ShowDialog();
                if (resultado == DialogResult.OK)
                {
                    
                    // contador = 20;

                    
                    String ruta = abrir.FileName;
                    bar3.Value = 50;

                    string[] files = { ruta };

                    if (ruta.Length > 0)
                    {
                        bar3.Value = 70;
                        btconver.Enabled = true;
                        // contador = 40;

                        lbruta.Text = ruta;
                        convertir(img, pdf, ruta);

                    }



                }
                }

            }

        private void OCR_Load(object sender, EventArgs e)
        {
            panel2.AllowDrop = true;
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib))
            {
                btpegar.Enabled = true;
            }
            else
            {
                btpegar.Enabled = false;
            }
        }

        private void rbima_CheckedChanged(object sender, EventArgs e)
        {
            if (rbima.Checked)
            {
                img = true;
                btlect.Enabled = false;
                btpegar.Enabled = true;
                pdf = false;
            }
           
        }

        private void rbpdf_CheckedChanged(object sender, EventArgs e)
        {
            if (rbpdf.Checked)
            {
                img = false;
                pdf = true;
                btpegar.Enabled = false;

            }
            else
            {
                btlect.Enabled = false;
            }
        }

        private void btcan_Click(object sender, EventArgs e)
        {
            bar3.Value = 0;
            chpag.Checked = false;
            btlect.Enabled = false;
            lbbar.Text = "n/a";
            ruta2 = "";
            ingresado = false;
            paginas=false;
            btcan.Enabled = false;
            label2.Text = "Arrastre o busque el archivo";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtresult.Text);
        }

        private void OCR_Activated(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib)&&img&&pdf==false)
            {
                btpegar.Enabled = true;
            }
            else
            {
                btpegar.Enabled = false;
            }
        }

        private void btpegar_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
               
                Image image = Clipboard.GetImage();

                string imagePath =  System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(),".jpg");
                image.Save(imagePath, ImageFormat.Jpeg);
               
                img = true;
             

                    pictureBox = new PictureBox();
                    int width = 320;
                    float aspectRatio = (float)image.Height / (float)image.Width;


                    int height = (int)(width * aspectRatio);
                    Image resizedImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    pictureBox.Image = resizedImage;


                    pictureBox.Size = new Size(width, height);
                    int x = (panel2.Width - pictureBox.Width) / 2;
                    int y = (panel2.Height - pictureBox.Height) / 2;

                    pictureBox.Location = new Point(x, y);
                    panel1.Invoke(new Action(() => {
                        panel2.Controls.Add(pictureBox);
                    }));
                



                convertir(img,pdf,imagePath);

                
            }
        }

        private void OCR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control| Keys.V)&&pdf==false)
            {
                
              
                if (Clipboard.ContainsImage())
                {
                    Image image = Clipboard.GetImage();

                   
                    string imagePath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".jpg");
                    image.Save(imagePath, ImageFormat.Jpeg);
                    img = true;
                    

                    pictureBox = new PictureBox();
                    int width = 320;
                    float aspectRatio = (float)image.Height / (float)image.Width;


                    int height = (int)(width * aspectRatio);
                    Image resizedImage = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    pictureBox.Image = resizedImage;


                    pictureBox.Size = new Size(width, height);
                    int x = (panel2.Width - pictureBox.Width) / 2;
                    int y = (panel2.Height - pictureBox.Height) / 2;

                    pictureBox.Location = new Point(x, y);
                    panel1.Invoke(new Action(() => {
                        panel2.Controls.Add(pictureBox);
                    }));
                    convertir(img, pdf, imagePath);
                  
                    e.Handled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Principal p = new Principal();
            p.Show();
        }

        private void chpag_CheckedChanged(object sender, EventArgs e)
        {
            if (chpag.Checked)
            {
                paginas = true;
               // btlect.Enabled = true;
            }
            else
            {
                paginas = false;
                btlect.Enabled = false;
            }
        }

        private void btlect_Click(object sender, EventArgs e)
        {

            
                Lector lt = new Lector(ruta2,txtresult,label2,lbbar,bar3,btconver);
                lt.Show();
          
        }
        public void mostrar()
        {
           
        }
       
     
        private void btconver_Click(object sender, EventArgs e)
        {
            panel2.Enabled = true;
            txtresult.Text = "";
            bar3.Value = 0;
            chpag.Checked = false;
            btlect.Enabled = false;
            panel2.Controls.Remove(pictureBox);
            label2.Text = "Arrastre o busque el archivo";
            btcan.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btruta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog rutaguarda = new FolderBrowserDialog();

            rutaguarda.Description = "Seleccione carpeta de destino";
            rutaguarda.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult resultado = rutaguarda.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                
                lbsms.Dispose();
                lbrutadest.Text = rutaguarda.SelectedPath;
                rtdestino = rutaguarda.SelectedPath;

            }
        }
    }
}
