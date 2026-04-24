using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ConvertidorImagenes
{
   
    public partial class Form1 : Form
    {
        bool noproces = false, cambio = false;
        string files = "",rtdestino=@"C:\Users\"+Environment.UserName+@"\Desktop";
        int contador = 0;
        String formato = "",nombarch="";
        public Form1()
        {
            InitializeComponent();
           
            btnomb.Enabled = false;
            txtnombre.Enabled = false;
            btremove.Enabled = false;
            
            String[] formatos = {"JPG","PNG","WEBP","GIF","BITMAP(BMP)","ICO"};
            lbrutadest.Text = rtdestino;
           
            foreach (String ft in formatos)
            {
                cbformatos.Items.Add(ft);
            }
            cbformatos.SelectedItem = "JPG";
      
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
            lbsms2.Dispose();
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "Todos los archivos (*.*)|*.*";
            abrir.Title = "Selecciona una iamgen a convertir...";
            DialogResult resultado = abrir.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                contador = 20;
                cbformatos.Enabled = false;
                lbprogre.Text = "Convertiendo...";
                String ruta = abrir.FileName;
                if (cambio)
                {
                    nombarch = txtnombre.Text;
                }
                else
                {
                    nombarch = abrir.SafeFileName;
                }
               
                string[] files = { ruta };

                if (ruta.Length > 0)
                {
                    contador = 40;
                    foreach (String file in files)
                    {
                        lbft.Text = file.Substring(file.LastIndexOf(".") + 1);
                        if (IsImageFile(ruta) && noproces == false)
                        {
                            pictureBox1.Image = Image.FromFile(ruta);
                            lbruta.Text = ruta;
                            contador = 80;
                            convertir(ruta,formato);
                          
                            break;
                        }
                        if (noproces)
                        {
                            convertir(ruta,formato);
                            pictureBox1.Image = null;
                        }
                    }

                    pictureBox1.Enabled = false;


                }


            }
        }
       
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
       
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            
        }
        private bool IsImageFile(string fileName)
        {
            if (fileName.EndsWith(".webp"))
            {
                noproces = true;
            }
            else
            {
               
                noproces = false;
            }
            
            return fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ||
                   fileName.EndsWith(".png") || fileName.EndsWith(".gif") ||
                   fileName.EndsWith(".webp")|| fileName.EndsWith(".bmp")|| fileName.EndsWith(".ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.AllowDrop = true;
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            lbprogre.Text = "Convertiendo...";
            cbformatos.Enabled = false;
            var im =e.Data.GetData(DataFormats.FileDrop);
           
            contador = 20;
            
            if (im != null) {
                var nombre = im as string[];
                if (nombre.Length > 0)
                {
                    contador = 40;
                    foreach (String file in nombre)
                    {
                        if (cambio)
                        {
                            nombarch = txtnombre.Text;
                        }
                        else
                        {
                            nombarch = Path.GetFileName(nombre[0]);
                        }
                       
                        contador = 70;
                        if (IsImageFile(file)&&noproces==false)
                        {
                            lbft.Text = file.Substring(file.LastIndexOf(".")+1);
                            pictureBox1.Image = Image.FromFile(nombre[0]);
                            lbruta.Text = nombre[0];
                            contador = 80;
                            convertir(nombre[0],formato);
                         
                            break;
                        }
                        if(noproces)
                        {
                            convertir(nombre[0],formato);
                            pictureBox1.Image = null;
                        }
                    }
                   
                }


                pictureBox1.Enabled = false;
            }
            
        }

        public void convertir(String files, String formato)
        {
           
            switch (formato)
            {
                case "JPG": {
                        using (var image = new MagickImage(files))
                        {

                            image.Format = MagickFormat.Jpg;
                            image.Write(rtdestino + @"\"+nombarch+"-Convertido"+".jpg");
                            contador = 100;
                            progreso.Value = contador;
                           
                            System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                        }
                        lbprogre.Text = "Finalizado!";
                        
                        btremove.Enabled = true;
                    } break;
                case "PNG":
                    {
                        using (var image = new MagickImage(files))
                        {

                            image.Format = MagickFormat.Png;
                            image.Write(rtdestino + @"\" + nombarch +" -Convertido"+ ".png");
                            progreso.Value = 100;
                          
                            System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                        }
                        lbprogre.Text = "Finalizado!";
                        
                        btremove.Enabled = true;
                    }
                    break;
                case "GIF":
                    {
                        using (var image = new MagickImage(files))
                        {

                            image.Format = MagickFormat.Gif;
                            image.Write(rtdestino + @"\" + nombarch + " -Convertido" + ".gif");
                            progreso.Value = 100;
                         
                            System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                        }
                        lbprogre.Text = "Finalizado!";
                       
                        btremove.Enabled = true;
                    }
                    break;
                   
                case "WEBP":
                    {
                        using (var image = new MagickImage(files))
                        {

                            image.Format = MagickFormat.WebP;
                            image.Write(rtdestino + @"\" + nombarch + " -Convertido" + ".webp");
                            progreso.Value = 100;

                            System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                        }
                        lbprogre.Text = "Finalizado!";
                        
                        btremove.Enabled = true;
                    }
                    break;
                case "BITMAP(BMP)":
                    {
                        using (var image = new MagickImage(files))
                        {

                            image.Format = MagickFormat.Bmp;
                            image.Write(rtdestino + @"\" + nombarch + " -Convertido" + ".bmp");
                            progreso.Value = 100;

                            System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                        }
                        lbprogre.Text = "Finalizado!";
                       
                        btremove.Enabled = true;
                    }
                    break;
                case "ICO":
                    {
                        try
                        {
                            using (var image = new MagickImage(files))
                            {

                                image.Format = MagickFormat.Ico;
                                image.Write(rtdestino + @"\" + nombarch + " -Convertido" + ".ico");
                                progreso.Value = 100;

                                System.Diagnostics.Process.Start("explorer.exe", rtdestino);
                            }
                            lbprogre.Text = "Finalizado!";
                           
                            btremove.Enabled = true;
                        }
                        catch (Exception e)
                        {
                            btremove.Enabled = true;
                            lbprogre.Text = "Fallo!";
                            MessageBox.Show("Resolucion muy grande o imagen no apta: Resolucion de formato ico(16x16, 32x32, 64x64 y 128x128 píxeles)");
                        }
                    }
                    break;
            }
            
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
         
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    
                    if (Path.GetExtension(file).Equals(".jpg") || Path.GetExtension(file).Equals(".jpeg") ||
                   Path.GetExtension(file).Equals(".png") || Path.GetExtension(file).Equals(".gif") ||
                  Path.GetExtension(file).Equals(".webp") || Path.GetExtension(file).Equals(".bmp") ||  Path.GetExtension(file).Equals(".ico"))
                    {
                        e.Effect = DragDropEffects.Copy;
                        pictureBox1.Cursor = Cursors.Hand; 
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
            pictureBox1.Cursor = Cursors.No;
        }


  

        private void btconver_Click(object sender, EventArgs e)
        {
            cambio = true;
           
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btremove_Click(object sender, EventArgs e)
        {
            lbprogre.Text = "n/a";
            lbruta.Text = "n/a";
            lbft.Text = "n/a";
            pictureBox1.Image = null;
            pictureBox1.Enabled = true;
            btremove.Enabled = false;
            cbformatos.Enabled = true;
            progreso.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           this.Dispose();
            Principal p = new Principal();
            p.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                txtnombre.Enabled = true;
                btnomb.Enabled = true;
               
            }
            else
            {
                txtnombre.Enabled = false;
                btnomb.Enabled = false;
                cambio = false;
            }
        }

        private void cbformatos_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            lbformat.Text = cbformatos.SelectedItem.ToString();
            formato=cbformatos.SelectedItem.ToString();
        }

        private void btruta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog rutaguarda = new FolderBrowserDialog();

            rutaguarda.Description = "Seleccione carpeta de destino";
            rutaguarda.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult resultado = rutaguarda.ShowDialog();
            if (resultado== DialogResult.OK)
            {
                lbsms.Dispose();
                // Obtiene la ruta de guardado seleccionada
                lbrutadest.Text = rutaguarda.SelectedPath;
                rtdestino = rutaguarda.SelectedPath;
                // Guarda los datos en el archivo correspondiente
                // ...
            }
        }
    }
}
