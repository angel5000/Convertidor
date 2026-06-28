// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.redimension
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ConvertidorImagenes;
using ConvertidorImagenes.Properties;

public class redimension : Form
{
	private Bitmap originalImage;

	private bool isSelecting;

	private Point selectionStart;

	private Point selectionEnd;

	private int contador = 1;

	private Rectangle cropRectangle;

	private bool recor = false;

	private bool redimen = true;

	private bool cargado = false;

	private bool mover = false;

	private bool apli = false;

	private string ruta = "";

	private string nombrearch = "";

	private int xori = 0;

	private int yori = 0;

	private int p1 = 0;

	private int p2 = 0;

	private int xori2 = 0;

	private int yori2 = 0;

	private int pp1 = 0;

	private int pp2 = 0;

	private IContainer components = null;

	private PictureBox pictureBox1;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private RadioButton rbrecorte;

	private RadioButton rbredi;

	private TextBox txtancho;

	private TextBox txtaltura;

	private Label lbruta;

	private Label label3;

	private Panel panel3;

	private Button btguardar;

	private PictureBox vistapre;

	private Button btcancel;

	private Button btapli;

	private Label label2;

	private Label lbaviso;

	private Button button1;

	private Button btvisua;

	private Button btzomin;

	private Panel panel4;

	private Button btzomout;

	private Label label7;

	private Label label6;

	private Label lbpx;

	private Label label9;

	private Label lbpix2;

	private Label lbpxrc;

	private Button button3;

	private Button button4;

	private Button button5;

	private Button button6;

	public redimension()
	{
		InitializeComponent();
		btzomout.Enabled = false;
		btzomin.Enabled = false;
		xori = pictureBox1.Size.Width;
		yori = pictureBox1.Size.Height;
		p1 = pictureBox1.Location.X;
		p2 = pictureBox1.Location.Y;
		xori2 = vistapre.Size.Width;
		yori2 = vistapre.Size.Height;
		pp1 = vistapre.Location.X;
		pp2 = vistapre.Location.Y;
		button1.Enabled = false;
		lbaviso.Show();
		btcancel.Enabled = false;
		btapli.Enabled = false;
		txtaltura.Enabled = false;
		txtancho.Enabled = false;
		button3.Enabled = false;
		btguardar.Enabled = false;
		btvisua.Enabled = false;
		btzomin.Enabled = false;
		btzomout.Enabled = false;
		Text = "Edicion de Imagenes";
	}

	private void CargarImagen(string imagePath)
	{
		originalImage = new Bitmap(imagePath);
		pictureBox1.Image = originalImage;
		mover = true;
		ruta = imagePath;
		if (pictureBox1.Image.Width < pictureBox1.Width && pictureBox1.Image.Height < pictureBox1.Height)
		{
			pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBox1.ClientSize = originalImage.Size;
			int num = (panel1.Width - pictureBox1.Width) / 2;
			int num2 = (panel1.Height - pictureBox1.Height) / 2;
			pictureBox1.Location = new Point(num, num2);
		}
		else
		{
			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
		}
		int num3 = pictureBox1.Image.Width;
		int num4 = pictureBox1.Image.Height;
		if (num3 >= pictureBox1.Width || num4 >= pictureBox1.Height)
		{
			float num5 = Math.Min((float)pictureBox1.Width / (float)num3, (float)pictureBox1.Height / (float)num4);
			num3 = (int)((float)num3 * num5);
			num4 = (int)((float)num4 * num5);
		}
		pictureBox1.ClientSize = new Size(num3, num4);
		int num6 = (panel1.Width - pictureBox1.Width) / 2;
		int num7 = (panel1.Height - pictureBox1.Height) / 2;
		pictureBox1.Location = new Point(num6, num7);
		cargado = true;
		pictureBox1.AllowDrop = false;
		lbpix2.Text = pictureBox1.Image.Width + "x" + pictureBox1.Image.Height;
		if (rbredi.Checked)
		{
			txtaltura.Enabled = true;
			txtancho.Enabled = true;
		}
	}

	private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
	{
		if (isSelecting)
		{
			selectionEnd = new Point(Math.Max(Math.Min(e.Location.X, pictureBox1.Width - 1), 0), Math.Max(Math.Min(e.Location.Y, pictureBox1.Height - 1), 0));
			pictureBox1.Invalidate();
		}
	}

	private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right && !cargado)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Imágenes PNG (*.png)|*.png|Imágenes JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Imágenes BITMAP (*.bmp)|*.bmp|Imágenes GIF (*.gif)|*.gif|Imágenes ICO (*.ico)|*.ico";
			openFileDialog.Title = "Selecciona una iamgen a convertir...";
			DialogResult dialogResult = openFileDialog.ShowDialog();
			nombrearch = openFileDialog.SafeFileName;
			if (dialogResult == DialogResult.OK)
			{
				lbaviso.Hide();
				string fileName = openFileDialog.FileName;
				string[] array = new string[1] { fileName };
				if (fileName.Length > 0)
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (IsImageFile(fileName))
						{
							pictureBox1.Image = Image.FromFile(fileName);
							lbruta.Text = fileName;
							CargarImagen(fileName);
							break;
						}
					}
				}
			}
		}
		if (recor && !redimen && cargado && mover && e.Button == MouseButtons.Left)
		{
			selectionStart = e.Location;
			isSelecting = true;
		}
	}

	private bool IsImageFile(string fileName)
	{
		return fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".png") || fileName.EndsWith(".gif") || fileName.EndsWith(".bmp") || fileName.EndsWith(".ico") || fileName.EndsWith(".PNG") || fileName.EndsWith(".JPG");
	}

	private void button2_Click(object sender, EventArgs e)
	{
		visualizador visualizador2 = new visualizador(vistapre.Image);
		visualizador2.ShowDialog();
	}

	public void redimensionar(Image imagen)
	{
		btguardar.Enabled = true;
		btvisua.Enabled = true;
		btzomin.Enabled = true;
		btzomout.Enabled = true;
		vistapre.Size = new Size(xori2, yori2);
		vistapre.Location = new Point(pp1, pp2);
		int num = int.Parse(txtancho.Text);
		int num2 = int.Parse(txtaltura.Text);
		if (num < pictureBox1.Image.Width && num2 < pictureBox1.Image.Height)
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num3 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num3);
			Image thumbnailImage = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage;
			int num4 = thumbnailImage.Width;
			int num5 = thumbnailImage.Height;
			vistapre.ClientSize = new Size(num4, num5);
			vistapre.ClientSize = new Size(num4, num5);
			int num6 = (panel4.Width - vistapre.Width) / 2;
			int num7 = (panel4.Height - vistapre.Height) / 2;
			vistapre.Location = new Point(num6, num7);
		}
		else
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num8 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num8);
			Image thumbnailImage2 = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage2;
			int num9 = thumbnailImage2.Width;
			int num10 = thumbnailImage2.Height;
			vistapre.ClientSize = new Size(num9, num10);
			int num11 = (panel4.Width - vistapre.Width) / 2;
			int num12 = (panel4.Height - vistapre.Height) / 2;
			vistapre.Location = new Point(num11, num12);
		}
		if (num > pictureBox1.Image.Width && num2 > pictureBox1.Image.Height)
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num13 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num13);
			Image thumbnailImage3 = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage3;
			int num14 = thumbnailImage3.Width;
			int num15 = thumbnailImage3.Height;
			vistapre.ClientSize = new Size(num14, num15);
		}
		int num16 = (panel4.Width - vistapre.Width) / 2;
		int num17 = (panel4.Height - vistapre.Height) / 2;
		vistapre.Location = new Point(num16, num17);
		lbpxrc.Text = vistapre.Image.Width + "x" + vistapre.Image.Height;
		txtaltura.Text = vistapre.Image.Height.ToString();
	}

	private void btcancel_Click(object sender, EventArgs e)
	{
		pictureBox1.AllowDrop = true;
		pictureBox1.Image = null;
		pictureBox1.Enabled = true;
		vistapre.Image = null;
		cargado = false;
		apli = false;
		txtaltura.Text = "";
		txtancho.Text = "";
		pictureBox1.Cursor = Cursors.Default;
		pictureBox1.Size = new Size(xori, yori);
		pictureBox1.Location = new Point(p1, p2);
		vistapre.Size = new Size(xori2, yori2);
		vistapre.Location = new Point(pp1, pp2);
		lbaviso.Show();
		button3.Enabled = false;
		btguardar.Enabled = false;
		btvisua.Enabled = false;
		btzomin.Enabled = false;
		btzomout.Enabled = false;
		lbpx.Text = "n/a";
		lbpix2.Text = "n/a";
		lbpxrc.Text = "n/a";
		lbruta.Text = "n/a";
	}

	private void button3_Click_1(object sender, EventArgs e)
	{
		txtaltura.Text = "";
		txtancho.Text = "";
	}

	private void button4_Click_1(object sender, EventArgs e)
	{
		Dispose();
		Principal principal = new Principal();
		principal.Show();
	}

	private void redimension_FormClosing(object sender, FormClosingEventArgs e)
	{
		Environment.Exit(0);
	}

	private void button6_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void button5_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void btapli_Click(object sender, EventArgs e)
	{
		if (txtaltura.Text.Equals("") || txtancho.Text.Equals(""))
		{
			MessageBox.Show("INGRESE LOS DATOS CORRESPONDIENTES");
			return;
		}
		redimensionar(pictureBox1.Image);
		lbpx.Text = pictureBox1.Image.Width + "x" + pictureBox1.Image.Height;
		apli = true;
	}

	private void txtaltura_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
		btcancel.Enabled = true;
		if (txtancho.Text.Equals("") && txtaltura.Text.Equals(""))
		{
			btcancel.Enabled = false;
		}
		else
		{
			btcancel.Enabled = true;
		}
		if (!txtancho.Text.Equals("") && !txtaltura.Text.Equals(""))
		{
			btapli.Enabled = true;
			button3.Enabled = true;
		}
		else
		{
			btapli.Enabled = false;
			button3.Enabled = false;
		}
		txtaltura.MaxLength = 4;
	}

	private void txtancho_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
		txtancho.MaxLength = 4;
		btcancel.Enabled = true;
		if (txtancho.Text.Equals("") && txtaltura.Text.Equals(""))
		{
			btcancel.Enabled = false;
		}
		else
		{
			btcancel.Enabled = true;
		}
		if (!txtancho.Text.Equals("") && !txtaltura.Text.Equals(""))
		{
			btapli.Enabled = true;
			button3.Enabled = true;
		}
		else
		{
			btapli.Enabled = false;
			button3.Enabled = false;
		}
	}

	private void btguardar_Click(object sender, EventArgs e)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		string text = ruta.Substring(ruta.LastIndexOf(".") + 1);
		saveFileDialog.Title = "Guardar imagen";
		saveFileDialog.DefaultExt = "." + text;
		saveFileDialog.Filter = "Imágenes PNG (*.png)|*.png|Imágenes JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Imágenes BITMAP (*.bmp)|*.bmp|Imágenes GIF (*.gif)|*.gif|Imágenes ICO (*.ico)|*.ico";
		saveFileDialog.FileName = nombrearch + " -recortado";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = saveFileDialog.FileName;
			ImageFormat imageFormat = null;
            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    imageFormat = ImageFormat.Png;
                    break;
                case 2:
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case 3:
                    imageFormat = ImageFormat.Bmp;
                    break;
                case 4:
                    imageFormat = ImageFormat.Gif;
                    break;
                case 5:
                    imageFormat = ImageFormat.Icon;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }
            vistapre.Image.Save(fileName, imageFormat);
		}
	}

	private void pictureBox1_Paint(object sender, PaintEventArgs e)
	{
		if (cropRectangle != Rectangle.Empty)
		{
			using (Brush brush = new SolidBrush(Color.FromArgb(128, Color.White)))
			{
				Region region = new Region(pictureBox1.ClientRectangle);
				region.Exclude(cropRectangle);
				e.Graphics.FillRegion(brush, region);
				return;
			}
		}
		if (isSelecting)
		{
			Rectangle rectangle = MakeRectangle(selectionStart, selectionEnd);
			using (Brush brush2 = new SolidBrush(Color.FromArgb(128, Color.White)))
			{
				rectangle = MakeRectangle(selectionStart, selectionEnd);
				e.Graphics.FillRectangle(brush2, rectangle);
			}
		}
	}
	private Rectangle MakeRectangle(Point start, Point end)
	{
		int num = Math.Max(Math.Min(start.X, end.X), 0);
		int num2 = Math.Max(Math.Min(start.Y, end.Y), 0);
		int num3 = Math.Min(Math.Abs(start.X - end.X), pictureBox1.Width - num);
		int num4 = Math.Min(Math.Abs(start.Y - end.Y), pictureBox1.Height - num2);
		return new Rectangle(num, num2, num3, num4);
	}

	private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			isSelecting = false;
			cropRectangle = MakeRectangle(selectionStart, selectionEnd);
			if (cropRectangle.Width > 0 && cropRectangle.Height > 0)
			{
				lbpx.Text = pictureBox1.Image.Width + "x" + pictureBox1.Image.Height;
				vistapre.Size = new Size(xori2, yori2);
				vistapre.Location = new Point(pp1, pp2);
				Bitmap bitmap = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
				vistapre.SizeMode = PictureBoxSizeMode.Zoom;
				vistapre.Image = bitmap.Clone(cropRectangle, bitmap.PixelFormat);
				lbpxrc.Text = vistapre.Image.Width + "x" + vistapre.Image.Height;
				int num = vistapre.Image.Width;
				int num2 = vistapre.Image.Height;
				float num3 = Math.Min((float)vistapre.Width / (float)num, (float)vistapre.Height / (float)num2);
				num = (int)((float)num * num3);
				num2 = (int)((float)num2 * num3);
				vistapre.ClientSize = new Size(num, num2);
				int num4 = (panel4.Width - vistapre.Width) / 2;
				int num5 = (panel4.Height - vistapre.Height) / 2;
				vistapre.Location = new Point(num4, num5);
				btzomin.Enabled = true;
				btguardar.Enabled = true;
				btvisua.Enabled = true;
				btzomin.Enabled = true;
				btzomout.Enabled = true;
			}
		}
		cropRectangle = Rectangle.Empty;
		pictureBox1.Invalidate();
	}

	private void rbredi_CheckedChanged(object sender, EventArgs e)
	{
		if (rbredi.Checked)
		{
			recor = false;
			redimen = true;
			pictureBox1.Cursor = Cursors.Default;
			mover = false;
			txtaltura.Enabled = true;
			txtancho.Enabled = true;
			button1.Enabled = false;
			Text = "Edicion de Imagenes - Redimenzionar";
		}
		if (cargado && !mover && !apli)
		{
			txtaltura.Enabled = true;
			txtancho.Enabled = true;
		}
	}

	private void redimension_Load(object sender, EventArgs e)
	{
		pictureBox1.AllowDrop = true;
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	private void button1_Click(object sender, EventArgs e)
	{
		pictureBox1.AllowDrop = true;
		pictureBox1.Image = null;
		pictureBox1.Enabled = true;
		vistapre.Image = null;
		cargado = false;
		mover = false;
		apli = false;
		pictureBox1.Cursor = Cursors.Default;
		pictureBox1.Size = new Size(xori, yori);
		pictureBox1.Location = new Point(p1, p2);
		vistapre.Size = new Size(xori2, yori2);
		vistapre.Location = new Point(pp1, pp2);
		txtaltura.Text = "";
		txtancho.Text = "";
		btguardar.Enabled = false;
		btvisua.Enabled = false;
		btzomin.Enabled = false;
		btzomout.Enabled = false;
		lbpx.Text = "n/a";
		lbpix2.Text = "n/a";
		lbpxrc.Text = "n/a";
		lbaviso.Show();
		lbruta.Text = "n/a";
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (contador <= 5)
		{
			vistapre.Top = (int)((double)vistapre.Top - (double)vistapre.Height * 0.075);
			vistapre.Left = (int)((double)vistapre.Left - (double)vistapre.Width * 0.075);
			vistapre.Height = (int)((double)vistapre.Height + (double)vistapre.Height * 0.2);
			vistapre.Width = (int)((double)vistapre.Width + (double)vistapre.Width * 0.2);
			contador++;
			btzomout.Enabled = true;
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		if (contador >= 2)
		{
			vistapre.Top = (int)((double)vistapre.Top + (double)vistapre.Height * 0.075);
			vistapre.Left = (int)((double)vistapre.Left + (double)vistapre.Width * 0.075);
			vistapre.Height = (int)((double)vistapre.Height - (double)vistapre.Height * 0.2);
			vistapre.Width = (int)((double)vistapre.Width - (double)vistapre.Width * 0.2);
			contador--;
		}
		if (contador <= 2)
		{
			contador = 1;
			btzomout.Enabled = false;
		}
	}

	private void rbrecorte_CheckedChanged(object sender, EventArgs e)
	{
		if (rbrecorte.Checked)
		{
			recor = true;
			redimen = false;
			pictureBox1.Enabled = true;
			button1.Enabled = true;
			txtaltura.Enabled = false;
			txtancho.Enabled = false;
			btapli.Enabled = false;
			btcancel.Enabled = false;
			Text = "Edicion de Imagenes - Recortar";
			if (cargado)
			{
				mover = true;
			}
			else
			{
				mover = false;
			}
			pictureBox1.Cursor = Cursors.Cross;
		}
		if (rbredi.Checked && apli)
		{
			btapli.Enabled = true;
		}
	}

	private void pictureBox1_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(DataFormats.FileDrop);
		if (data == null)
		{
			return;
		}
		string[] array = data as string[];
		if (array.Length == 0)
		{
			return;
		}
		string[] array2 = array;
		foreach (string fileName in array2)
		{
			if (IsImageFile(fileName))
			{
				lbaviso.Hide();
				pictureBox1.Image = Image.FromFile(array[0]);
				lbruta.Text = array[0];
				nombrearch = Path.GetFileName(array[0]);
				CargarImagen(array[0]);
				break;
			}
		}
	}

	private void pictureBox1_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] array2 = array;
			foreach (string path in array2)
			{
				if (Path.GetExtension(path).Equals(".jpg") || Path.GetExtension(path).Equals(".jpeg") || Path.GetExtension(path).Equals(".png") || Path.GetExtension(path).Equals(".gif") || Path.GetExtension(path).Equals(".bmp") || Path.GetExtension(path).Equals(".ico"))
				{
					e.Effect = DragDropEffects.Copy;
					return;
				}
			}
		}
		e.Effect = DragDropEffects.None;
		pictureBox1.Cursor = Cursors.No;
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
		//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertidorImagenes.redimension));
		this.panel1 = new System.Windows.Forms.Panel();
		this.lbaviso = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lbpix2 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.btcancel = new System.Windows.Forms.Button();
		this.btapli = new System.Windows.Forms.Button();
		this.txtancho = new System.Windows.Forms.TextBox();
		this.txtaltura = new System.Windows.Forms.TextBox();
		this.rbrecorte = new System.Windows.Forms.RadioButton();
		this.rbredi = new System.Windows.Forms.RadioButton();
		this.lbruta = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btzomout = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lbpxrc = new System.Windows.Forms.Label();
		this.lbpx = new System.Windows.Forms.Label();
		this.vistapre = new System.Windows.Forms.PictureBox();
		this.btzomin = new System.Windows.Forms.Button();
		this.btvisua = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.btguardar = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.vistapre).BeginInit();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.lbaviso);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.lbpix2);
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Location = new System.Drawing.Point(12, 65);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(433, 520);
		this.panel1.TabIndex = 1;
		this.lbaviso.AutoSize = true;
		this.lbaviso.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbaviso.Location = new System.Drawing.Point(93, 259);
		this.lbaviso.Name = "lbaviso";
		this.lbaviso.Size = new System.Drawing.Size(217, 13);
		this.lbaviso.TabIndex = 13;
		this.lbaviso.Text = "Arrastre o Click Derecho para cargar Imagen";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Transparent;
		this.label9.Location = new System.Drawing.Point(14, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(95, 13);
		this.label9.TabIndex = 14;
		this.label9.Text = "Resolucion en Pix:";
		this.lbpix2.AutoSize = true;
		this.lbpix2.BackColor = System.Drawing.Color.Transparent;
		this.lbpix2.Location = new System.Drawing.Point(115, 2);
		this.lbpix2.Name = "lbpix2";
		this.lbpix2.Size = new System.Drawing.Size(24, 13);
		this.lbpix2.TabIndex = 9;
		this.lbpix2.Text = "n/a";
		this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
		this.pictureBox1.Location = new System.Drawing.Point(17, 18);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(397, 480);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(pictureBox1_DragDrop);
		this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(pictureBox1_DragEnter);
		this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(pictureBox1_Paint);
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
		this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseMove);
		this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseUp);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(304, 27);
		this.label1.TabIndex = 2;
		this.label1.Text = "Redimensionador de imagenes";
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.button3);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.button1);
		this.panel2.Controls.Add(this.btcancel);
		this.panel2.Controls.Add(this.btapli);
		this.panel2.Controls.Add(this.txtancho);
		this.panel2.Controls.Add(this.txtaltura);
		this.panel2.Controls.Add(this.rbrecorte);
		this.panel2.Controls.Add(this.rbredi);
		this.panel2.Location = new System.Drawing.Point(499, 68);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(278, 194);
		this.panel2.TabIndex = 3;
		this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button3.Location = new System.Drawing.Point(134, 65);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(35, 25);
		this.button3.TabIndex = 8;
		this.button3.Text = " ←";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click_1);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(81, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(50, 13);
		this.label7.TabIndex = 7;
		this.label7.Text = "Altura pix";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(20, 47);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(54, 13);
		this.label6.TabIndex = 6;
		this.label6.Text = "Ancho pix";
		this.button1.Location = new System.Drawing.Point(13, 166);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(106, 23);
		this.button1.TabIndex = 5;
		this.button1.Text = "Cancelar Edicion";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.btcancel.Location = new System.Drawing.Point(120, 102);
		this.btcancel.Name = "btcancel";
		this.btcancel.Size = new System.Drawing.Size(75, 23);
		this.btcancel.TabIndex = 4;
		this.btcancel.Text = "Cancelar ";
		this.btcancel.UseVisualStyleBackColor = true;
		this.btcancel.Click += new System.EventHandler(btcancel_Click);
		this.btapli.Location = new System.Drawing.Point(22, 102);
		this.btapli.Name = "btapli";
		this.btapli.Size = new System.Drawing.Size(75, 23);
		this.btapli.TabIndex = 2;
		this.btapli.Text = "Aplicar";
		this.btapli.UseVisualStyleBackColor = true;
		this.btapli.Click += new System.EventHandler(btapli_Click);
		this.txtancho.Location = new System.Drawing.Point(23, 67);
		this.txtancho.Name = "txtancho";
		this.txtancho.Size = new System.Drawing.Size(46, 20);
		this.txtancho.TabIndex = 3;
		this.txtancho.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtancho_KeyPress);
		this.txtaltura.Location = new System.Drawing.Point(84, 67);
		this.txtaltura.Name = "txtaltura";
		this.txtaltura.Size = new System.Drawing.Size(46, 20);
		this.txtaltura.TabIndex = 2;
		this.txtaltura.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtaltura_KeyPress);
		this.rbrecorte.AutoSize = true;
		this.rbrecorte.Location = new System.Drawing.Point(20, 140);
		this.rbrecorte.Name = "rbrecorte";
		this.rbrecorte.Size = new System.Drawing.Size(66, 17);
		this.rbrecorte.TabIndex = 1;
		this.rbrecorte.Text = "Recortar";
		this.rbrecorte.UseVisualStyleBackColor = true;
		this.rbrecorte.CheckedChanged += new System.EventHandler(rbrecorte_CheckedChanged);
		this.rbredi.AutoSize = true;
		this.rbredi.Checked = true;
		this.rbredi.Location = new System.Drawing.Point(22, 18);
		this.rbredi.Name = "rbredi";
		this.rbredi.Size = new System.Drawing.Size(95, 17);
		this.rbredi.TabIndex = 0;
		this.rbredi.TabStop = true;
		this.rbredi.Text = "Redimensionar";
		this.rbredi.UseVisualStyleBackColor = true;
		this.rbredi.CheckedChanged += new System.EventHandler(rbredi_CheckedChanged);
		this.lbruta.AutoSize = true;
		this.lbruta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbruta.Location = new System.Drawing.Point(87, 588);
		this.lbruta.Name = "lbruta";
		this.lbruta.Size = new System.Drawing.Size(24, 13);
		this.lbruta.TabIndex = 9;
		this.lbruta.Text = "n/a";
		this.label3.AutoSize = true;
		this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label3.Location = new System.Drawing.Point(14, 588);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 13);
		this.label3.TabIndex = 6;
		this.label3.Text = "Ruta Origen:";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.btzomout);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.btzomin);
		this.panel3.Controls.Add(this.btvisua);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.btguardar);
		this.panel3.Location = new System.Drawing.Point(499, 268);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(373, 330);
		this.panel3.TabIndex = 11;
		this.btzomout.Location = new System.Drawing.Point(291, 287);
		this.btzomout.Name = "btzomout";
		this.btzomout.Size = new System.Drawing.Size(50, 23);
		this.btzomout.TabIndex = 16;
		this.btzomout.Text = "Zoom-";
		this.btzomout.UseVisualStyleBackColor = true;
		this.btzomout.Click += new System.EventHandler(button4_Click);
		this.panel4.Controls.Add(this.lbpxrc);
		this.panel4.Controls.Add(this.lbpx);
		this.panel4.Controls.Add(this.vistapre);
		this.panel4.Location = new System.Drawing.Point(8, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(348, 251);
		this.panel4.TabIndex = 15;
		this.lbpxrc.AutoSize = true;
		this.lbpxrc.BackColor = System.Drawing.Color.Transparent;
		this.lbpxrc.Location = new System.Drawing.Point(9, 226);
		this.lbpxrc.Name = "lbpxrc";
		this.lbpxrc.Size = new System.Drawing.Size(24, 13);
		this.lbpxrc.TabIndex = 9;
		this.lbpxrc.Text = "n/a";
		this.lbpx.AutoSize = true;
		this.lbpx.BackColor = System.Drawing.Color.Transparent;
		this.lbpx.Location = new System.Drawing.Point(9, 13);
		this.lbpx.Name = "lbpx";
		this.lbpx.Size = new System.Drawing.Size(24, 13);
		this.lbpx.TabIndex = 8;
		this.lbpx.Text = "n/a";
		this.vistapre.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.vistapre.Location = new System.Drawing.Point(3, 4);
		this.vistapre.Name = "vistapre";
		this.vistapre.Size = new System.Drawing.Size(342, 245);
		this.vistapre.TabIndex = 0;
		this.vistapre.TabStop = false;
		this.btzomin.Location = new System.Drawing.Point(235, 287);
		this.btzomin.Name = "btzomin";
		this.btzomin.Size = new System.Drawing.Size(50, 23);
		this.btzomin.TabIndex = 14;
		this.btzomin.Text = "Zoom+";
		this.btzomin.UseVisualStyleBackColor = true;
		this.btzomin.Click += new System.EventHandler(button3_Click);
		this.btvisua.Location = new System.Drawing.Point(95, 287);
		this.btvisua.Name = "btvisua";
		this.btvisua.Size = new System.Drawing.Size(113, 23);
		this.btvisua.TabIndex = 13;
		this.btvisua.Text = "Visualizar Imagen";
		this.btvisua.UseVisualStyleBackColor = true;
		this.btvisua.Click += new System.EventHandler(button2_Click);
		this.label2.AutoSize = true;
		this.label2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label2.Location = new System.Drawing.Point(11, 14);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(115, 13);
		this.label2.TabIndex = 12;
		this.label2.Text = "Vista previa de Imagen";
		this.btguardar.Location = new System.Drawing.Point(13, 287);
		this.btguardar.Name = "btguardar";
		this.btguardar.Size = new System.Drawing.Size(75, 23);
		this.btguardar.TabIndex = 1;
		this.btguardar.Text = "Guardar";
		this.btguardar.UseVisualStyleBackColor = true;
		this.btguardar.Click += new System.EventHandler(btguardar_Click);
		//this.button5.Image = ConvertidorImagenes.Properties.Resources.ResourceManager.;
		this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button5.Location = new System.Drawing.Point(666, 604);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(100, 43);
		this.button5.TabIndex = 30;
		this.button5.Text = "Minimizar";
		this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		//this.button6.Image = (System.Drawing.Image)resources.GetObject("button6.Image");
		this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button6.Location = new System.Drawing.Point(772, 604);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(100, 43);
		this.button6.TabIndex = 29;
		this.button6.Text = "Salir";
		this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
		//this.button4.Image = (System.Drawing.Image)resources.GetObject("button4.Image");
		this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button4.Location = new System.Drawing.Point(772, 17);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(100, 43);
		this.button4.TabIndex = 28;
		this.button4.Text = "Principal";
		this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 659);
		base.ControlBox = false;
		base.Controls.Add(this.button5);
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.lbruta);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "redimension";
		this.Text = "redimension";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(redimension_FormClosing);
		base.Load += new System.EventHandler(redimension_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.vistapre).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
