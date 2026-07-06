// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.Form1
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ConvertidorImagenes;
using ConvertidorImagenes.Controllers;
using ConvertidorImagenes.Properties;
using ConvertidorImagenes.Services;

namespace ConvertidorImagenes
{
public partial class Form1 : Form
{
	private readonly ImageConverterController imageController = new ImageConverterController(new ImageConversionService());

	private bool noproces = false;

	private bool cambio = false;

	private bool boton = false;

	private bool chkbt = true;

	private string rtdestino = "";

	private string formato = "";

	private string nombarch = "";

	private bool mostrarubi = true;

	private string Ruta = "";

	private string format = "";

	private IContainer components = null;

	private PictureBox pictureBox1;

	private Label label1;

	private Button btruta;

	private RoundedPanel panel1;

	private Label label2;

	private Label lbrutadest;

	private Label lbruta;

	private Label label4;

	private Label label3;

	private Button btnomb;

	private Button btremove;

	private Label lbft;

	private Label label6;

	private Label label5;

	private ComboBox cbformatos;

	private ProgressBar progreso;

	private Label label8;

	private Label lbprogre;

	private Label label9;

	private Label lbformat;

	private RoundedPanel panel2;

	private TextBox txtnombre;

	private CheckBox checkBox1;

	private Button button1;

	private RoundedPanel panel3;

	private Label label7;

	private CheckBox chkgd;

	private Button button4;

	private Button button6;

	private Label lbnombarch;

	private Label label10;

	private Button btconvertir;

	private CheckBox chkbtcv;

	public Form1()
	{
		InitializeComponent();
		this.DoubleBuffered = true;
		this.BackColor = Color.FromArgb(238, 240, 245);
		ApplyModernStyles();
		btnomb.Enabled = false;
		txtnombre.Enabled = false;
		btremove.Text = "Otra conversion";
		btremove.Enabled = false;
		rtdestino = imageController.DefaultDestination;
		string[] array = imageController.SupportedFormats;
		lbrutadest.Text = rtdestino;
		string[] array2 = array;
		foreach (string item in array2)
		{
			cbformatos.Items.Add(item);
		}
		cbformatos.SelectedItem = "JPG";
	}

	private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Todos los formatos de imagen (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp;*.ico)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp;*.ico|Archivos JPG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Archivos PNG (*.png)|*.png|Archivos GIF (*.gif)|*.gif|Archivos BMP (*.bmp)|*.bmp|Archivos WEBP (*.webp)|*.webp|Archivos ICO (*.ico)|*.ico";
		openFileDialog.Title = "Selecciona una iamgen a convertir...";
		DialogResult dialogResult = openFileDialog.ShowDialog();
		if (dialogResult != DialogResult.OK)
		{
			return;
		}
		progreso.Value = 40;
		btruta.Enabled = false;
		cbformatos.Enabled = false;
		lbprogre.Text = "Convertiendo...";
		string fileName = openFileDialog.FileName;
		btconvertir.Enabled = true;
		if (cambio)
		{
			nombarch = txtnombre.Text;
		}
		else
		{
			nombarch = openFileDialog.SafeFileName;
		}
		string[] array = new string[1] { fileName };
		if (fileName.Length <= 0)
		{
			return;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			lbft.Text = text.Substring(text.LastIndexOf(".") + 1);
			if (IsImageFile(fileName) && !noproces && chkbt)
			{
				progreso.Value = 60;
				pictureBox1.Image = Image.FromFile(fileName);
				lbruta.Text = fileName;
				Ruta = fileName;
				format = formato;
				convertir(fileName, formato);
				break;
			}
			if (!noproces && !chkbt)
			{
				btconvertir.Enabled = true;
				lbprogre.Text = "Esperando aceptacion...";
				btremove.Text = "Cancelar";
				btremove.Enabled = true;
				Ruta = fileName;
				format = formato;
				pictureBox1.Image = Image.FromFile(Ruta);
				lbruta.Text = Ruta;
			}
			if (noproces && chkbt)
			{
				progreso.Value = 60;
				convertir(fileName, formato);
				Ruta = fileName;
				format = formato;
				pictureBox1.Image = null;
			}
			if (noproces && !chkbt)
			{
				lbprogre.Text = "Esperando aceptacion...";
				btremove.Text = "Cancelar";
				btremove.Enabled = true;
				progreso.Value = 60;
				btconvertir.Enabled = true;
				Ruta = fileName;
				format = formato;
				lbruta.Text = Ruta;
				pictureBox1.Image = null;
			}
		}
		pictureBox1.Enabled = false;
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
		noproces = imageController.RequiresPreviewBypass(fileName);
		return imageController.IsSupportedImageFile(fileName);
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		pictureBox1.AllowDrop = true;
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	private void pictureBox1_DragDrop(object sender, DragEventArgs e)
	{
		btconvertir.Enabled = true;
		lbprogre.Text = "Convertiendo...";
		cbformatos.Enabled = false;
		object data = e.Data.GetData(DataFormats.FileDrop);
		btruta.Enabled = false;
		progreso.Value = 40;
		if (data == null)
		{
			return;
		}
		string[] array = data as string[];
		if (array.Length != 0)
		{
			if (cambio)
			{
				nombarch = txtnombre.Text;
			}
			else
			{
				nombarch = Path.GetFileName(array[0]);
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (IsImageFile(text) && !noproces && chkbt)
				{
					progreso.Value = 60;
					lbft.Text = text.Substring(text.LastIndexOf(".") + 1);
					pictureBox1.Image = Image.FromFile(array[0]);
					lbruta.Text = array[0];
					convertir(array[0], formato);
					break;
				}
				if (!noproces && !chkbt)
				{
					btconvertir.Enabled = true;
					lbprogre.Text = "Esperando aceptacion...";
				btremove.Text = "Cancelar";
				btremove.Enabled = true;
					Ruta = array[0];
					format = formato;
					pictureBox1.Image = Image.FromFile(Ruta);
					lbruta.Text = Ruta;
				}
				if (noproces && chkbt)
				{
					progreso.Value = 60;
					btconvertir.Enabled = true;
					convertir(array[0], formato);
					pictureBox1.Image = null;
				}
				if (noproces && !chkbt)
				{
					lbprogre.Text = "Esperando aceptacion...";
				btremove.Text = "Cancelar";
				btremove.Enabled = true;
					progreso.Value = 60;
					btconvertir.Enabled = true;
					Ruta = array[0];
					format = formato;
					lbruta.Text = Ruta;
					pictureBox1.Image = null;
				}
			}
		}
		pictureBox1.Enabled = false;
	}

	public void convertir(string files, string formato)
	{
		btnomb.Enabled = false;
		btnomb.Enabled = true;
		panel2.Enabled = false;
		btconvertir.Enabled = false;
		progreso.Value = 90;
		try
		{
			imageController.ConvertImage(files, rtdestino, nombarch, formato, mostrarubi);
			progreso.Value = 100;
			lbprogre.Text = "Finalizado!";
			btnomb.Enabled = true;
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			return;
		}
		catch (Exception ex)
		{
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			lbprogre.Text = "Fallo!";
			if (formato == "ICO")
			{
				MessageBox.Show("Resolucion muy grande o imagen no apta: Resolucion de formato ico(16x16, 32x32, 64x64 y 128x128 pixeles)");
			}
			else
			{
				MessageBox.Show(ex.Message);
			}
			return;
		}
		/*
		string text = "";
		switch (formato)
		{
		case "JPG":
			try
			{
				using (MagickImage magickImage3 = new MagickImage(files))
				{
					magickImage3.Format = MagickFormat.Jpg;
					if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.jpg"))
					{
						int num3 = 1;
						while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.jpg") + "(" + num3 + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.jpg")))
						{
							num3++;
						}
						text = rtdestino + "\\" + nombarch + "-Convertido(" + num3 + ").jpg";
						magickImage3.Write(text);
					}
					else
					{
						magickImage3.Write(rtdestino + "\\" + nombarch + "-Convertido.jpg");
					}
					if (mostrarubi)
					{
						Process.Start("explorer.exe", rtdestino);
					}
				}
				progreso.Value = 100;
				lbprogre.Text = "Finalizado!";
				btnomb.Enabled = true;
				btremove.Text = "Otra conversion";
			btremove.Enabled = true;
				break;
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
				break;
			}
		case "PNG":
		{
			using (MagickImage magickImage5 = new MagickImage(files))
			{
				magickImage5.Format = MagickFormat.Png;
				if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.png"))
				{
					int num5 = 1;
					while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.png") + "(" + num5 + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.png")))
					{
						num5++;
					}
					text = rtdestino + "\\" + nombarch + "-Convertido(" + num5 + ").png";
					magickImage5.Write(text);
				}
				else
				{
					magickImage5.Write(rtdestino + "\\" + nombarch + "-Convertido.png");
				}
				if (mostrarubi)
				{
					Process.Start("explorer.exe", rtdestino);
				}
			}
			progreso.Value = 100;
			lbprogre.Text = "Finalizado!";
			btnomb.Enabled = true;
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			break;
		}
		case "GIF":
		{
			using (MagickImage magickImage6 = new MagickImage(files))
			{
				magickImage6.Format = MagickFormat.Gif;
				if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.gif"))
				{
					int num6 = 1;
					while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.gif") + "(" + num6 + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.gif")))
					{
						num6++;
					}
					text = rtdestino + "\\" + nombarch + "-Convertido(" + num6 + ").gif";
					magickImage6.Write(text);
				}
				else
				{
					magickImage6.Write(rtdestino + "\\" + nombarch + "-Convertido.gif");
				}
				if (mostrarubi)
				{
					Process.Start("explorer.exe", rtdestino);
				}
			}
			progreso.Value = 100;
			lbprogre.Text = "Finalizado!";
			btnomb.Enabled = true;
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			break;
		}
		case "WEBP":
		{
			using (MagickImage magickImage4 = new MagickImage(files))
			{
				magickImage4.Format = MagickFormat.WebP;
				if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.webp"))
				{
					int num4 = 1;
					while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.webp") + "(" + num4 + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.webp")))
					{
						num4++;
					}
					text = rtdestino + "\\" + nombarch + "-Convertido(" + num4 + ").webp";
					magickImage4.Write(text);
				}
				else
				{
					magickImage4.Write(rtdestino + "\\" + nombarch + "-Convertido.webp");
				}
				if (mostrarubi)
				{
					Process.Start("explorer.exe", rtdestino);
				}
			}
			progreso.Value = 100;
			lbprogre.Text = "Finalizado!";
			btnomb.Enabled = true;
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			break;
		}
		case "BITMAP(BMP)":
		{
			using (MagickImage magickImage2 = new MagickImage(files))
			{
				magickImage2.Format = MagickFormat.Bmp;
				if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.bmp"))
				{
					int num2 = 1;
					while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.bmp") + "(" + num2 + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.bmp")))
					{
						num2++;
					}
					text = rtdestino + "\\" + nombarch + "-Convertido(" + num2 + ").bmp";
					magickImage2.Write(text);
				}
				else
				{
					magickImage2.Write(rtdestino + "\\" + nombarch + "-Convertido.bmp");
				}
				if (mostrarubi)
				{
					Process.Start("explorer.exe", rtdestino);
				}
			}
			progreso.Value = 100;
			lbprogre.Text = "Finalizado!";
			btnomb.Enabled = true;
			btremove.Text = "Otra conversion";
			btremove.Enabled = true;
			break;
		}
		case "ICO":
			try
			{
				using (MagickImage magickImage = new MagickImage(files))
				{
					magickImage.Format = MagickFormat.Ico;
					if (File.Exists(rtdestino + "\\" + nombarch + "-Convertido.ico"))
					{
						int num = 1;
						while (File.Exists(rtdestino + Path.GetFileNameWithoutExtension("\\" + nombarch + "-Convertido.bmp") + "(" + num + ")" + Path.GetExtension(rtdestino + "\\" + nombarch + "-Convertido.ico")))
						{
							num++;
						}
						text = rtdestino + "\\" + nombarch + "-Convertido(" + num + ").ico";
						magickImage.Write(text);
					}
					else
					{
						magickImage.Write(rtdestino + "\\" + nombarch + "-Convertido.ico");
					}
					if (mostrarubi)
					{
						Process.Start("explorer.exe", rtdestino);
					}
				}
				progreso.Value = 100;
				lbprogre.Text = "Finalizado!";
				btnomb.Enabled = true;
				btremove.Text = "Otra conversion";
			btremove.Enabled = true;
				break;
			}
			catch (Exception)
			{
				btremove.Text = "Otra conversion";
			btremove.Enabled = true;
				lbprogre.Text = "Fallo!";
				MessageBox.Show("Resolucion muy grande o imagen no apta: Resolucion de formato ico(16x16, 32x32, 64x64 y 128x128 pÃ­xeles)");
				break;
			}
		}
	}

		*/
	}

	private void pictureBox1_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] array2 = array;
			foreach (string path in array2)
			{
				if (Path.GetExtension(path).Equals(".jpg") || Path.GetExtension(path).Equals(".jpeg") || Path.GetExtension(path).Equals(".png") || Path.GetExtension(path).Equals(".gif") || Path.GetExtension(path).Equals(".webp") || Path.GetExtension(path).Equals(".bmp") || Path.GetExtension(path).Equals(".ico"))
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
		lbnombarch.Text = txtnombre.Text;
		lbprogre.Text = "Cargue una Imagen...";
		lbprogre.Location = new Point(569, 275);
	}

	private void btremove_Click(object sender, EventArgs e)
	{
		lbprogre.Text = "";
		lbruta.Text = "";
		lbft.Text = "";
		pictureBox1.Image = null;
		pictureBox1.Enabled = true;
		btremove.Text = "Otra conversion";
		btremove.Enabled = false;
		cbformatos.Enabled = true;
		btconvertir.Enabled = false;
		progreso.Value = 0;
		btruta.Enabled = true;
		panel2.Enabled = true;
		txtnombre.Text = "";
		if (cambio)
		{
			txtnombre.Enabled = true;
			btnomb.Enabled = true;
		}
		else
		{
			txtnombre.Enabled = false;
			btnomb.Enabled = false;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Dispose();
		Principal principal = new Principal();
		principal.Show();
	}

	private void chkgd_CheckedChanged(object sender, EventArgs e)
	{
		if (!chkgd.Checked)
		{
			mostrarubi = false;
		}
		else
		{
			mostrarubi = true;
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
	}

	private void button6_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBox1.Checked)
		{
			txtnombre.Enabled = true;
			btnomb.Enabled = true;
			cambio = true;
		}
		else
		{
			txtnombre.Enabled = false;
			btnomb.Enabled = false;
			cambio = false;
		}
	}

	private void chkbtcv_CheckedChanged(object sender, EventArgs e)
	{
		if (chkbtcv.Checked)
		{
			boton = true;
			btconvertir.Hide();
			chkbt = true;
		}
		else
		{
			btconvertir.Show();
			btconvertir.Enabled = false;
			chkbt = false;
			boton = false;
		}
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		Environment.Exit(0);
	}

	private void btconvertir_Click(object sender, EventArgs e)
	{
		boton = true;
		convermanual();
	}

	public void convermanual()
	{
		if (boton)
		{
			convertir(Ruta, format);
		}
	}

	private void cbformatos_SelectedIndexChanged(object sender, EventArgs e)
	{
		lbformat.Text = cbformatos.SelectedItem.ToString();
		formato = cbformatos.SelectedItem.ToString();
	}

	private void btruta_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.Description = "Seleccione carpeta de destino";
		folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
		DialogResult dialogResult = folderBrowserDialog.ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
			lbrutadest.Text = folderBrowserDialog.SelectedPath;
			rtdestino = folderBrowserDialog.SelectedPath;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertidorImagenes.Form1));
		this.label1 = new System.Windows.Forms.Label();
		this.btruta = new System.Windows.Forms.Button();
		this.panel1 = new ConvertidorImagenes.RoundedPanel();
		this.btremove = new System.Windows.Forms.Button();
		this.lbformat = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lbprogre = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.progreso = new System.Windows.Forms.ProgressBar();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lbft = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cbformatos = new System.Windows.Forms.ComboBox();
		this.lbrutadest = new System.Windows.Forms.Label();
		this.lbruta = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnomb = new System.Windows.Forms.Button();
		this.panel2 = new ConvertidorImagenes.RoundedPanel();
		this.lbnombarch = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.txtnombre = new System.Windows.Forms.TextBox();
		this.panel3 = new ConvertidorImagenes.RoundedPanel();
		this.label7 = new System.Windows.Forms.Label();
		this.chkgd = new System.Windows.Forms.CheckBox();
		this.button4 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.btconvertir = new System.Windows.Forms.Button();
		this.chkbtcv = new System.Windows.Forms.CheckBox();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label1.Location = new System.Drawing.Point(3, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(187, 17);
		this.label1.TabIndex = 1;
		this.label1.Text = "Agrega o Arrastra una imagen";
		this.btruta.Location = new System.Drawing.Point(6, 327);
		this.btruta.Name = "btruta";
		this.btruta.Size = new System.Drawing.Size(108, 23);
		this.btruta.TabIndex = 2;
		this.btruta.Text = "Seleccionar Ruta";
		this.btruta.UseVisualStyleBackColor = true;
		this.btruta.Click += new System.EventHandler(btruta_Click);
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.btconvertir);
		this.panel1.Controls.Add(this.btremove);
		this.panel1.Controls.Add(this.lbformat);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.lbprogre);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.progreso);
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Controls.Add(this.lbft);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.cbformatos);
		this.panel1.Controls.Add(this.lbrutadest);
		this.panel1.Controls.Add(this.lbruta);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.btruta);
		this.panel1.Location = new System.Drawing.Point(18, 61);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(770, 360);
		this.panel1.TabIndex = 4;
		this.btremove.Image = (System.Drawing.Image)resources.GetObject("btremove.Image");
		this.btremove.Location = new System.Drawing.Point(510, 166);
		this.btremove.Name = "btremove";
		this.btremove.Size = new System.Drawing.Size(108, 26);
		this.btremove.TabIndex = 6;
		this.btremove.Text = "Otra conversion";
		this.btremove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btremove.UseVisualStyleBackColor = true;
		this.btremove.Click += new System.EventHandler(btremove_Click);
		this.lbformat.AutoSize = true;
		this.lbformat.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbformat.Location = new System.Drawing.Point(630, 133);
		this.lbformat.Name = "lbformat";
		this.lbformat.Size = new System.Drawing.Size(24, 13);
		this.lbformat.TabIndex = 16;
		this.lbformat.Text = "";
		this.label9.AutoSize = true;
		this.label9.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label9.Location = new System.Drawing.Point(510, 133);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(114, 13);
		this.label9.TabIndex = 15;
		this.label9.Text = "Formato seleccionado:";
		this.lbprogre.AutoSize = true;
		this.lbprogre.BackColor = System.Drawing.Color.Transparent;
		this.lbprogre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lbprogre.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbprogre.Location = new System.Drawing.Point(595, 280);
		this.lbprogre.Name = "lbprogre";
		this.lbprogre.Size = new System.Drawing.Size(24, 13);
		this.lbprogre.TabIndex = 12;
		this.lbprogre.Text = "";
		this.lbprogre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.AutoSize = true;
		this.label8.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label8.Location = new System.Drawing.Point(524, 244);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(52, 13);
		this.label8.TabIndex = 11;
		this.label8.Text = "Progreso:";
		this.progreso.Location = new System.Drawing.Point(527, 275);
		this.progreso.Name = "progreso";
		this.progreso.Size = new System.Drawing.Size(175, 23);
		this.progreso.TabIndex = 10;
		this.pictureBox1.BackColor = System.Drawing.Color.White;
		this.pictureBox1.BackgroundImage = (System.Drawing.Image)resources.GetObject("pictureBox1.BackgroundImage");
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.pictureBox1.ErrorImage = null;
		this.pictureBox1.InitialImage = null;
		this.pictureBox1.Location = new System.Drawing.Point(101, 41);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(403, 263);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(pictureBox1_DragDrop);
		this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(pictureBox1_DragEnter);
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
		this.lbft.AutoSize = true;
		this.lbft.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbft.Location = new System.Drawing.Point(615, 41);
		this.lbft.Name = "lbft";
		this.lbft.Size = new System.Drawing.Size(24, 13);
		this.lbft.TabIndex = 9;
		this.lbft.Text = "";
		this.label6.AutoSize = true;
		this.label6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label6.Location = new System.Drawing.Point(510, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(99, 13);
		this.label6.TabIndex = 8;
		this.label6.Text = "Formato detectado:";
		this.label5.AutoSize = true;
		this.label5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label5.Location = new System.Drawing.Point(510, 71);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(151, 13);
		this.label5.TabIndex = 7;
		this.label5.Text = "Seleccione formato a convertir";
		this.cbformatos.FormattingEnabled = true;
		this.cbformatos.Location = new System.Drawing.Point(513, 98);
		this.cbformatos.Name = "cbformatos";
		this.cbformatos.Size = new System.Drawing.Size(153, 21);
		this.cbformatos.TabIndex = 6;
		this.cbformatos.Text = "Formatos...";
		this.cbformatos.SelectedIndexChanged += new System.EventHandler(cbformatos_SelectedIndexChanged);
		this.lbrutadest.AutoSize = true;
		this.lbrutadest.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbrutadest.Location = new System.Drawing.Point(198, 332);
		this.lbrutadest.Name = "lbrutadest";
		this.lbrutadest.Size = new System.Drawing.Size(24, 13);
		this.lbrutadest.TabIndex = 5;
		this.lbrutadest.Text = "";
		this.lbruta.AutoSize = true;
		this.lbruta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbruta.Location = new System.Drawing.Point(198, 306);
		this.lbruta.Name = "lbruta";
		this.lbruta.Size = new System.Drawing.Size(24, 13);
		this.lbruta.TabIndex = 4;
		this.lbruta.Text = "";
		this.label4.AutoSize = true;
		this.label4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label4.Location = new System.Drawing.Point(125, 332);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 13);
		this.label4.TabIndex = 3;
		this.label4.Text = "Ruta Destino:";
		this.label3.AutoSize = true;
		this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label3.Location = new System.Drawing.Point(125, 306);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 13);
		this.label3.TabIndex = 2;
		this.label3.Text = "Ruta Origen:";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Times New Roman", 20.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label2.Location = new System.Drawing.Point(12, 27);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(285, 31);
		this.label2.TabIndex = 2;
		this.label2.Text = "Convertidor de Imagenes";
		this.btnomb.Location = new System.Drawing.Point(326, 18);
		this.btnomb.Name = "btnomb";
		this.btnomb.Size = new System.Drawing.Size(85, 23);
		this.btnomb.TabIndex = 5;
		this.btnomb.Text = "Aceptar";
		this.btnomb.UseVisualStyleBackColor = true;
		this.btnomb.Click += new System.EventHandler(btconver_Click);
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.lbnombarch);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.checkBox1);
		this.panel2.Controls.Add(this.txtnombre);
		this.panel2.Controls.Add(this.btnomb);
		this.panel2.Location = new System.Drawing.Point(18, 444);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(433, 68);
		this.panel2.TabIndex = 7;
		this.lbnombarch.AutoSize = true;
		this.lbnombarch.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbnombarch.Location = new System.Drawing.Point(143, 42);
		this.lbnombarch.Name = "lbnombarch";
		this.lbnombarch.Size = new System.Drawing.Size(24, 13);
		this.lbnombarch.TabIndex = 18;
		this.lbnombarch.Text = "";
		this.label10.AutoSize = true;
		this.label10.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label10.Location = new System.Drawing.Point(3, 42);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(134, 13);
		this.label10.TabIndex = 17;
		this.label10.Text = "Nuevo nombre de Archivo:";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(3, 21);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(141, 17);
		this.checkBox1.TabIndex = 8;
		this.checkBox1.Text = "Cambiar nombre Archivo";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.txtnombre.Location = new System.Drawing.Point(150, 20);
		this.txtnombre.Name = "txtnombre";
		this.txtnombre.Size = new System.Drawing.Size(159, 20);
		this.txtnombre.TabIndex = 7;
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.chkbtcv);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.chkgd);
		this.panel3.Location = new System.Drawing.Point(457, 444);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(331, 68);
		this.panel3.TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label7.Location = new System.Drawing.Point(3, 10);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(139, 13);
		this.label7.TabIndex = 17;
		this.label7.Text = "Configuraciones Opcionales";
		this.chkgd.AutoSize = true;
		this.chkgd.Checked = true;
		this.chkgd.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkgd.Location = new System.Drawing.Point(3, 38);
		this.chkgd.Name = "chkgd";
		this.chkgd.Size = new System.Drawing.Size(159, 17);
		this.chkgd.TabIndex = 0;
		this.chkgd.Text = "Mostrar ubicacion al finalizar";
		this.chkgd.UseVisualStyleBackColor = true;
		this.chkgd.CheckedChanged += new System.EventHandler(chkgd_CheckedChanged);
      this.button4.Image = null;
		this.button4.Location = new System.Drawing.Point(678, 518);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(108, 44);
		this.button4.TabIndex = 10;
		this.button4.Text = "Salir";
		this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
        this.button1.Image = null;
		this.button1.Location = new System.Drawing.Point(678, 12);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(99, 43);
		this.button1.TabIndex = 8;
		this.button1.Text = "Principal";
		this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
        this.button6.Image = null;
		this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button6.Location = new System.Drawing.Point(558, 519);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(100, 43);
		this.button6.TabIndex = 30;
		this.button6.Text = "Minimizar";
		this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
		this.btconvertir.Location = new System.Drawing.Point(510, 209);
		this.btconvertir.Name = "btconvertir";
		this.btconvertir.Size = new System.Drawing.Size(108, 23);
		this.btconvertir.TabIndex = 19;
		this.btconvertir.Text = "Convertir";
		this.btconvertir.UseVisualStyleBackColor = true;
		this.btconvertir.Click += new System.EventHandler(btconvertir_Click);
		this.chkbtcv.AutoSize = true;
		this.chkbtcv.Checked = true;
		this.chkbtcv.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkbtcv.Location = new System.Drawing.Point(167, 38);
		this.chkbtcv.Name = "chkbtcv";
		this.chkbtcv.Size = new System.Drawing.Size(134, 17);
		this.chkbtcv.TabIndex = 18;
		this.chkbtcv.Text = "Conversion automatica";
		this.chkbtcv.UseVisualStyleBackColor = true;
		this.chkbtcv.CheckedChanged += new System.EventHandler(chkbtcv_CheckedChanged);
		base.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 565);
		base.ControlBox = false;
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Name = "Form1";
		this.Text = "Convertidor de Imagenes";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
    [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
    private extern static void ReleaseCapture();
    [DllImport("user32.DLL", EntryPoint = "SendMessage")]
    private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void DragForm_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(this.Handle, 0x112, 0xf012, 0);
    }

    private void ApplyModernStyles()
    {
        this.MouseDown += new MouseEventHandler(DragForm_MouseDown);
        if (panel1 != null) panel1.MouseDown += new MouseEventHandler(DragForm_MouseDown);
        if (panel2 != null) panel2.MouseDown += new MouseEventHandler(DragForm_MouseDown);
        if (panel3 != null) panel3.MouseDown += new MouseEventHandler(DragForm_MouseDown);
        
        // Fuentes
        Font titleFont = new Font("Segoe UI", 16f, FontStyle.Bold);
        Font subtitleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        Font regularFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);

        Color darkText = Color.FromArgb(30, 32, 40);
        Color lightText = Color.FromArgb(120, 124, 135);
        Color primaryBlue = Color.FromArgb(109, 91, 208);
        Color borderSutil = Color.FromArgb(235, 236, 242);
        Color whiteBg = Color.White;

        // Estilos para RoundedPanels
        var panels = new RoundedPanel[] { panel1, panel2, panel3 };
        foreach (var p in panels)
        {
            if (p != null)
            {
                p.BackColor = whiteBg;
                p.CornerRadius = 14;
                p.BorderColor = borderSutil;
                p.BorderThickness = 1;
                p.BorderStyle = BorderStyle.None; 
            }
        }

        // Estilo de los Labels
        

        Label[] subtitulos = { label1, label7 };
        foreach (var l in subtitulos) { if (l != null) { l.Font = subtitleFont; l.ForeColor = darkText; } }

        Label[] textos = { label2, label3, label4, label5, label6, label8, label9, lbft, lbformat, lbprogre, lbruta, lbrutadest, lbnombarch };
        foreach (var l in textos) { if (l != null) { l.Font = regularFont; l.ForeColor = darkText; } }

        // Estilo de los CheckBoxes
        CheckBox[] checks = { checkBox1, chkgd, chkbtcv };
        foreach (var c in checks) { if (c != null) { c.Font = regularFont; c.ForeColor = darkText; } }

        // Estilo de los Botones
        Button[] primaryButtons = { btruta, btnomb, btconvertir, button1 };
        foreach (var b in primaryButtons)
        {
            if (b != null)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = primaryBlue;
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
            }
        }

        Button[] secondaryButtons = { btremove, button4, button6 };
        foreach (var b in secondaryButtons)
        {
            if (b != null)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = borderSutil;
                b.BackColor = whiteBg;
                b.ForeColor = darkText;
                b.Font = regularFont;
                b.Cursor = Cursors.Hand;
            }
        }

                        // Layout and Positions
        this.ClientSize = new Size(1020, 700);

        if (panel1 != null) { 
            panel1.Size = new Size(920, 440); 
            panel1.Location = new Point(40, 60);
        }
        if (panel2 != null) {
            panel2.Size = new Size(500, 80);
            panel2.Location = new Point(40, 520);
        }
        if (panel3 != null) {
            panel3.Size = new Size(420, 80);
            panel3.Location = new Point(560, 520);
            
            if (label7 != null) { label7.Location = new Point(15, 10); label7.AutoSize = true; }
            if (chkgd != null) { chkgd.Location = new Point(15, 45); chkgd.AutoSize = true; }
            if (chkbtcv != null) { chkbtcv.Checked = false; chkbtcv.Location = new Point(225, 45); chkbtcv.AutoSize = true; }
        }
        
        // Panel 2 layout
        if (checkBox1 != null) { checkBox1.Location = new Point(15, 15); checkBox1.AutoSize = true; }
        if (label10 != null) { label10.Location = new Point(15, 50); label10.AutoSize = true; label10.Font = regularFont; }
        if (txtnombre != null) { txtnombre.Location = new Point(180, 48); txtnombre.Size = new Size(180, 25); }
        if (btnomb != null) { btnomb.Location = new Point(380, 44); btnomb.Size = new Size(100, 32); }

        if (button4 != null) { button4.Location = new Point(760, 620); }
        if (button6 != null) { button6.Location = new Point(870, 620); }
        
        if (label2 != null) { label2.Location = new Point(20, 20); label2.Font = new Font("Segoe UI", 12f, FontStyle.Bold); }
        if (button1 != null) { button1.Location = new Point(850, 15); button1.Size = new Size(110, 35); }

        if (btruta != null) { btruta.Location = new Point(20, 380); btruta.Size = new Size(120, 35); }

        // Alineación derecha de textos
        if (lbruta != null) { lbruta.AutoSize = true; lbruta.Location = new Point(160, 380); }
        if (lbrutadest != null) { lbrutadest.AutoSize = true; lbrutadest.Location = new Point(160, 400); }
        
                        // AutoSize fix para textos cortados
        if (label9 != null) { label9.AutoSize = true; }
        if (label6 != null) { label6.AutoSize = true; }
        if (label5 != null) { label5.AutoSize = true; }
        if (label8 != null) { label8.AutoSize = true; label8.Location = new Point(510, 215); }

        if (lbformat != null) { lbformat.Location = new Point(680, 133); }
        if (lbft != null) { lbft.Location = new Point(680, 41); }
        
        if (lbprogre != null) { lbprogre.AutoSize = true; lbprogre.Location = new Point(580, 215); lbprogre.BackColor = Color.Transparent; }
        
        if (progreso != null) { progreso.Location = new Point(510, 240); progreso.Size = new Size(300, 25); }
        
        if (btconvertir != null) { btconvertir.Location = new Point(510, 290); btconvertir.Size = new Size(130, 35); }
        if (btremove != null) { btremove.Location = new Point(660, 290); btremove.Size = new Size(130, 35); }

        // Alineación derecha de textos de Rutas
        if (label3 != null) { label3.AutoSize = true; label3.Location = new Point(160, 350); }
        if (lbruta != null) { lbruta.AutoSize = true; lbruta.Location = new Point(250, 350); }
        
        if (label4 != null) { label4.AutoSize = true; label4.Location = new Point(160, 388); }
        if (lbrutadest != null) { lbrutadest.AutoSize = true; lbrutadest.Location = new Point(250, 388); }

        // Otros controles
        if (txtnombre != null) { txtnombre.Font = regularFont; txtnombre.BorderStyle = BorderStyle.FixedSingle; }
        if (label5 != null) { label5.AutoSize = true; }
        if (label6 != null) { label6.AutoSize = true; }
        if (label3 != null) { label3.AutoSize = true; }
        if (label4 != null) { label4.AutoSize = true; }
        if (label8 != null) { label8.AutoSize = true; }
        if (lbft != null) { lbft.AutoSize = true; }
        if (lbformat != null) { lbformat.AutoSize = true; }
        if (lbprogre != null) { lbprogre.AutoSize = true; lbprogre.Location = new Point(620, 196); }
        if (cbformatos != null) { cbformatos.Font = regularFont; cbformatos.FlatStyle = FlatStyle.Flat; }
        if (pictureBox1 != null) { pictureBox1.BackColor = whiteBg; }
    }
}
}











