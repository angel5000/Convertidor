// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.redimension
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ConvertidorImagenes;
using ConvertidorImagenes.Properties;

public class redimension : Form
{
	private Bitmap originalImage;

	private bool isSelecting;

	private bool isPanningImage;

	private Point selectionStart;

	private Point selectionEnd;

	private Point panStartMouse;

	private Point panStartScroll;

	private int contador = 1;

	private Rectangle cropRectangle;

	private const int MinCropZoom = 25;

	private const int MaxCropZoom = 400;

	private const int DefaultCropZoom = 100;

	private int cropZoom = DefaultCropZoom;

	private Size cropBaseSize = Size.Empty;

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

	private RoundedPanel panel1;

	private Panel imageCanvas;

	private Label label1;

	private RoundedPanel panel2;

	private RadioButton rbrecorte;

	private RadioButton rbredi;

	private TextBox txtancho;

	private TextBox txtaltura;

	private Label lbruta;

	private Label label3;

	private RoundedPanel panel3;

	private Button btguardar;

	private PictureBox vistapre;

	private Button btcancel;

	private Button btapli;

	private Label label2;

	private Label lbaviso;

	private Button button1;

	private Button btvisua;

	private Button btzomin;

	private RoundedPanel panel4;

	private Button btzomout;

	private Label label7;

	private Label label6;

	private Label lbpx;

	private Label label9;

	private Label lbpix2;

	private Label lbpxrc;

	private TrackBar trzoom;

	private Label lbzoom;

	private TrackBar trZoomPreview;
	private Label lbZoomPreview;

	private void UpdatePreviewZoom(object sender, EventArgs e)
	{
		if (vistapre.Image == null) return;
		float zoom = trZoomPreview.Value / 100f;
		lbZoomPreview.Text = trZoomPreview.Value + "%";
		vistapre.Size = new Size((int)(vistapre.Image.Width * zoom), (int)(vistapre.Image.Height * zoom));
		CenterPreviewImage();
	}

	private void CenterPreviewImage()
	{
		if (vistapre.Image == null || panel4 == null) return;
		int x = 0;
		int y = 0;
		if (vistapre.Width < panel4.ClientSize.Width)
			x = (panel4.ClientSize.Width - vistapre.Width) / 2;
		if (vistapre.Height < panel4.ClientSize.Height)
			y = (panel4.ClientSize.Height - vistapre.Height) / 2;
		
		vistapre.Location = new Point(x + panel4.AutoScrollPosition.X, y + panel4.AutoScrollPosition.Y);
	}

	private Button button3;

	private Button button4;

	private Button button5;

	private Button button6;

	public redimension()
	{
		InitializeComponent();
		this.DoubleBuffered = true;
		ApplyModernStyles();
		SetupDrawingToolsEvents();
		KeyPreview = true;
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
		ToggleEmptyState(true);
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

	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);
		UpdatePictureCursor();
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);
		UpdatePictureCursor();
	}

	private void redimension_Resize(object sender, EventArgs e)
	{
		if (WindowState == FormWindowState.Minimized || originalImage == null || isPanningImage)
		{
			return;
		}

		cropBaseSize = CalculateFitSize(originalImage.Size);
		SetCropZoom(cropZoom);
	}

	private void CargarImagen(string imagePath)
	{
		originalImage = new Bitmap(imagePath);
		pictureBox1.Image = originalImage;
		pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
		mover = true;
		ruta = imagePath;
		cropBaseSize = CalculateFitSize(originalImage.Size);
		SetCropZoom(DefaultCropZoom);
		cargado = true;
		pictureBox1.AllowDrop = false;
		lbpix2.Text = pictureBox1.Image.Width + "x" + pictureBox1.Image.Height;
		if (rbredi.Checked)
		{
			txtaltura.Enabled = true;
			txtancho.Enabled = true;
		}
	}

	private Size CalculateFitSize(Size imageSize)
	{
		int availableWidth = Math.Max(1, panel1.ClientSize.Width - (p1 * 2));
		int availableHeight = Math.Max(1, panel1.ClientSize.Height - p2 - 8);
		float scale = Math.Min((float)availableWidth / imageSize.Width, (float)availableHeight / imageSize.Height);
		scale = Math.Min(scale, 1f);

		return new Size(Math.Max(1, (int)(imageSize.Width * scale)), Math.Max(1, (int)(imageSize.Height * scale)));
	}

	private void SetCropZoom(int zoom)
	{
		if (originalImage == null || cropBaseSize == Size.Empty)
		{
			return;
		}

		int oldWidth = Math.Max(1, pictureBox1.Width);
		int oldHeight = Math.Max(1, pictureBox1.Height);
		Point currentScroll = new Point(-panel1.AutoScrollPosition.X, -panel1.AutoScrollPosition.Y);
		double centerRatioX = Math.Max(0, Math.Min(1, (currentScroll.X + panel1.ClientSize.Width / 2.0 - imageCanvas.Left - pictureBox1.Left) / oldWidth));
		double centerRatioY = Math.Max(0, Math.Min(1, (currentScroll.Y + panel1.ClientSize.Height / 2.0 - imageCanvas.Top - pictureBox1.Top) / oldHeight));

		cropZoom = Math.Max(MinCropZoom, Math.Min(MaxCropZoom, zoom));
		if (trzoom.Value != cropZoom)
		{
			trzoom.Value = cropZoom;
		}
		lbzoom.Text = cropZoom + "%";

		Size currentSize = new Size(
			Math.Max(1, cropBaseSize.Width * cropZoom / DefaultCropZoom),
			Math.Max(1, cropBaseSize.Height * cropZoom / DefaultCropZoom));

		panel1.SuspendLayout();
		panel1.AutoScrollPosition = new Point(0, 0);
		imageCanvas.Location = new Point(0, p2);
		imageCanvas.Size = new Size(
			Math.Max(panel1.ClientSize.Width - 2, currentSize.Width + (p1 * 2)),
			Math.Max(panel1.ClientSize.Height - p2 - 2, currentSize.Height + (p1 * 2)));
		pictureBox1.ClientSize = currentSize;
		int x = currentSize.Width < imageCanvas.ClientSize.Width ? (imageCanvas.ClientSize.Width - currentSize.Width) / 2 : p1;
		int y = currentSize.Height < imageCanvas.ClientSize.Height ? (imageCanvas.ClientSize.Height - currentSize.Height) / 2 : p1;
		pictureBox1.Location = new Point(x, y);

		int scrollX = imageCanvas.Width > panel1.ClientSize.Width
			? Math.Max(0, imageCanvas.Left + x + (int)Math.Round(centerRatioX * currentSize.Width) - panel1.ClientSize.Width / 2)
			: 0;
		int scrollY = imageCanvas.Bottom > panel1.ClientSize.Height
			? Math.Max(0, imageCanvas.Top + y + (int)Math.Round(centerRatioY * currentSize.Height) - panel1.ClientSize.Height / 2)
			: 0;
		panel1.AutoScrollPosition = new Point(scrollX, scrollY);
		panel1.ResumeLayout();
		UpdatePictureCursor();
		pictureBox1.Invalidate();
	}

	private bool CanPanImage()
	{
		return cargado
			&& (imageCanvas.Width > panel1.ClientSize.Width || imageCanvas.Bottom > panel1.ClientSize.Height);
	}

	private bool ShouldPanWithLeftButton()
	{
		if (!CanPanImage())
		{
			return false;
		}

		if (redimen)
		{
			return true;
		}

		return recor && (ModifierKeys & Keys.Control) == Keys.Control;
	}

	private void StartImagePan(MouseEventArgs e)
	{
		isPanningImage = true;
		panStartMouse = pictureBox1.PointToScreen(e.Location);
		panStartScroll = new Point(-panel1.AutoScrollPosition.X, -panel1.AutoScrollPosition.Y);
		pictureBox1.Cursor = Cursors.Hand;
		pictureBox1.Capture = true;
	}

	private void PanImage(MouseEventArgs e)
	{
		Point currentMouse = pictureBox1.PointToScreen(e.Location);
		int deltaX = currentMouse.X - panStartMouse.X;
		int deltaY = currentMouse.Y - panStartMouse.Y;
		int scrollX = Math.Max(0, panStartScroll.X - deltaX);
		int scrollY = Math.Max(0, panStartScroll.Y - deltaY);
		panel1.AutoScrollPosition = new Point(scrollX, scrollY);
	}

	private void StopImagePan()
	{
		isPanningImage = false;
		pictureBox1.Capture = false;
		UpdatePictureCursor();
	}

	private void UpdatePictureCursor()
	{
		if (!cargado)
		{
			pictureBox1.Cursor = Cursors.Default;
			return;
		}

		if (redimen && CanPanImage())
		{
			pictureBox1.Cursor = Cursors.Hand;
			return;
		}

		if (recor)
		{
			pictureBox1.Cursor = (CanPanImage() && (ModifierKeys & Keys.Control) == Keys.Control)
				? Cursors.Hand
				: Cursors.Cross;
			return;
		}

		pictureBox1.Cursor = Cursors.Default;
	}

	private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
	{
		if (isPanningImage)
		{
			PanImage(e);
			return;
		}

		if (isDrawing && e.Button == MouseButtons.Left && currentDrawingBitmap != null)
		{
			Bitmap temp = (Bitmap)currentDrawingBitmap.Clone();
			using (Graphics g = Graphics.FromImage(temp))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				Pen pen = new Pen(colorForma.BackColor, trGrosorPincel.Value);
				Brush brush = new SolidBrush(colorRelleno.BackColor);
				
				Point imgStart = ConvertDisplayPointToImagePoint(drawingStartPoint);
				Point imgEnd = ConvertDisplayPointToImagePoint(e.Location);
				
				int w = Math.Abs(imgEnd.X - imgStart.X);
				int h = Math.Abs(imgEnd.Y - imgStart.Y);
				int startX = Math.Min(imgStart.X, imgEnd.X);
				int startY = Math.Min(imgStart.Y, imgEnd.Y);
				
				if (currentTool == ToolMode.Pencil) {
					using (Graphics gBase = Graphics.FromImage(currentDrawingBitmap)) {
						gBase.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
						gBase.DrawLine(pen, imgStart, imgEnd);
					}
					drawingStartPoint = e.Location;
					temp.Dispose();
					temp = (Bitmap)currentDrawingBitmap.Clone();
				}
				else if (currentTool == ToolMode.Line) {
					g.DrawLine(pen, imgStart, imgEnd);
				}
				else if (currentTool == ToolMode.Rectangle) {
					if (colorRelleno.BackColor != Color.Transparent) g.FillRectangle(brush, startX, startY, w, h);
					g.DrawRectangle(pen, startX, startY, w, h);
				}
				else if (currentTool == ToolMode.Circle) {
					if (colorRelleno.BackColor != Color.Transparent) g.FillEllipse(brush, startX, startY, w, h);
					g.DrawEllipse(pen, startX, startY, w, h);
				}
			}
			Image oldImage = pictureBox1.Image;
			pictureBox1.Image = temp;
			if (oldImage != null && oldImage != currentDrawingBitmap && oldImage != originalImage) oldImage.Dispose();
			return;
		}

		if (isSelecting)
		{
			selectionEnd = new Point(Math.Max(Math.Min(e.Location.X, pictureBox1.Width - 1), 0), Math.Max(Math.Min(e.Location.Y, pictureBox1.Height - 1), 0));
			pictureBox1.Invalidate();
			return;
		}

		UpdatePictureCursor();
	}

	private void pictureBox1_MouseEnter(object sender, EventArgs e)
	{
		pictureBox1.Focus();
	}

	private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
	{
		if ((ModifierKeys & Keys.Control) != Keys.Control)
		{
			return;
		}

		MarkMouseWheelHandled(e);
		int step = e.Delta > 0 ? 10 : -10;
		SetCropZoom(cropZoom + step);
	}

	private void zoomSurface_MouseWheel(object sender, MouseEventArgs e)
	{
		if ((ModifierKeys & Keys.Control) != Keys.Control)
		{
			return;
		}

		MarkMouseWheelHandled(e);
		int step = e.Delta > 0 ? 10 : -10;
		SetCropZoom(cropZoom + step);
	}

	private void MarkMouseWheelHandled(MouseEventArgs e)
	{
		HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
		if (handledArgs != null)
		{
			handledArgs.Handled = true;
		}
	}

	private void trzoom_Scroll(object sender, EventArgs e)
	{
		SetCropZoom(trzoom.Value);
	}

	private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		pictureBox1.Focus();
		if (e.Button == MouseButtons.Right && !cargado)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Imágenes PNG (*.png)|*.png|Imágenes JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Imágenes BITMAP (*.bmp)|*.bmp|Imágenes GIF (*.gif)|*.gif|Imágenes ICO (*.ico)|*.ico";
			openFileDialog.Title = "Selecciona una imagen...";
			DialogResult dialogResult = openFileDialog.ShowDialog();
			nombrearch = openFileDialog.SafeFileName;
			if (dialogResult == DialogResult.OK)
			{
				ToggleEmptyState(false);
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

		if (e.Button == MouseButtons.Left && currentTool == ToolMode.Text && cargado)
		{
			if (floatingTextBox == null)
			{
				floatingTextBox = new TextBox();
				floatingTextBox.Multiline = true;
				floatingTextBox.BorderStyle = BorderStyle.FixedSingle;
				floatingTextBox.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter && !ev.Shift) { ev.SuppressKeyPress = true; CommitText(); } };
				floatingTextBox.LostFocus += (s, ev) => CommitText();
				pictureBox1.Controls.Add(floatingTextBox);
			}
			floatingTextBox.Font = new Font(cbFuenteTexto.SelectedItem.ToString(), trGrosorPincel.Value * 5);
			floatingTextBox.ForeColor = colorForma.BackColor;
			if (colorRelleno.BackColor == Color.Transparent) floatingTextBox.BackColor = Color.White;
			else floatingTextBox.BackColor = colorRelleno.BackColor;
			floatingTextBox.Location = e.Location;
			floatingTextBox.Size = new Size(200, (int)floatingTextBox.Font.Height + 10);
			floatingTextBox.Visible = true;
			floatingTextBox.Focus();
			return;
		}

		if (e.Button == MouseButtons.Left && currentTool != ToolMode.Select && cargado)
		{
			if (pictureBox1.Image != null)
			{
				SaveState();
				isDrawing = true;
				drawingStartPoint = e.Location;
				currentDrawingBitmap = (Bitmap)pictureBox1.Image.Clone();
			}
			return;
		}

		if (e.Button == MouseButtons.Left && ShouldPanWithLeftButton())
		{
			StartImagePan(e);
			return;
		}
		if (recor && !redimen && cargado && mover && e.Button == MouseButtons.Left)
		{
			selectionStart = e.Location;
			selectionEnd = e.Location;
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
		int num = int.Parse(txtancho.Text);
		int num2 = int.Parse(txtaltura.Text);
		if (num < pictureBox1.Image.Width && num2 < pictureBox1.Image.Height)
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num3 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num3);
			Image thumbnailImage = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage;
		}
		else
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num8 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num8);
			Image thumbnailImage2 = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage2;
		}
		if (num > pictureBox1.Image.Width && num2 > pictureBox1.Image.Height)
		{
			vistapre.SizeMode = PictureBoxSizeMode.Zoom;
			float num13 = (float)imagen.Height / (float)imagen.Width;
			num2 = (int)((float)num * num13);
			Image thumbnailImage3 = imagen.GetThumbnailImage(num, num2, null, IntPtr.Zero);
			vistapre.Image = thumbnailImage3;
		}
		lbpxrc.Text = vistapre.Image.Width + "x" + vistapre.Image.Height;
		txtaltura.Text = vistapre.Image.Height.ToString();
		if (trZoomPreview != null) { trZoomPreview.Value = 100; UpdatePreviewZoom(null, null); }
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
		imageCanvas.Size = new Size(Math.Max(1, panel1.ClientSize.Width - 2), Math.Max(1, panel1.ClientSize.Height - p2 - 2));
		imageCanvas.Location = new Point(0, p2);
		pictureBox1.Size = new Size(xori, yori);
		pictureBox1.Location = new Point(p1, p2);
		isSelecting = false;
		isPanningImage = false;
		panel1.AutoScrollPosition = new Point(0, 0);
		cropZoom = DefaultCropZoom;
		cropBaseSize = Size.Empty;
		trzoom.Value = DefaultCropZoom;
		lbzoom.Text = DefaultCropZoom + "%";
		ToggleEmptyState(true);
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
		Image imgToSave = vistapre.Image ?? pictureBox1.Image;
		if (imgToSave == null) return;

		SaveFileDialog saveFileDialog = new SaveFileDialog();
		string text = ruta.Substring(ruta.LastIndexOf(".") + 1);
		saveFileDialog.Title = "Guardar imagen";
		saveFileDialog.DefaultExt = "." + text;
		saveFileDialog.Filter = "Imágenes PNG (*.png)|*.png|Imágenes JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Imágenes BITMAP (*.bmp)|*.bmp|Imágenes GIF (*.gif)|*.gif|Imágenes ICO (*.ico)|*.ico";
		saveFileDialog.FileName = nombrearch + " -editado";
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
            imgToSave.Save(fileName, imageFormat);
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

	private Point ConvertDisplayPointToImagePoint(Point displayPoint)
	{
		if (originalImage == null || pictureBox1.ClientSize.Width == 0 || pictureBox1.ClientSize.Height == 0) return displayPoint;
		double scaleX = (double)originalImage.Width / pictureBox1.ClientSize.Width;
		double scaleY = (double)originalImage.Height / pictureBox1.ClientSize.Height;
		return new Point(Math.Max(0, (int)Math.Round(displayPoint.X * scaleX)), Math.Max(0, (int)Math.Round(displayPoint.Y * scaleY)));
	}

	private Rectangle ConvertDisplayRectangleToImageRectangle(Rectangle displayRectangle)
	{
		if (originalImage == null || pictureBox1.ClientSize.Width == 0 || pictureBox1.ClientSize.Height == 0)
		{
			return Rectangle.Empty;
		}

		double scaleX = (double)originalImage.Width / pictureBox1.ClientSize.Width;
		double scaleY = (double)originalImage.Height / pictureBox1.ClientSize.Height;

		int x = Math.Max(0, (int)Math.Round(displayRectangle.X * scaleX));
		int y = Math.Max(0, (int)Math.Round(displayRectangle.Y * scaleY));
		int width = Math.Max(1, (int)Math.Round(displayRectangle.Width * scaleX));
		int height = Math.Max(1, (int)Math.Round(displayRectangle.Height * scaleY));

		if (x + width > originalImage.Width)
		{
			width = originalImage.Width - x;
		}

		if (y + height > originalImage.Height)
		{
			height = originalImage.Height - y;
		}

		if (width <= 0 || height <= 0)
		{
			return Rectangle.Empty;
		}

		return new Rectangle(x, y, width, height);
	}

	private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
	{
		if (isPanningImage)
		{
			StopImagePan();
			return;
		}

		if (e.Button == MouseButtons.Left && isDrawing)
		{
			isDrawing = false;
			if (currentDrawingBitmap != null)
			{
				currentDrawingBitmap.Dispose();
				currentDrawingBitmap = null;
			}
			return;
		}

		if (e.Button == MouseButtons.Left)
		{
			if (!isSelecting)
			{
				return;
			}

			isSelecting = false;
			cropRectangle = MakeRectangle(selectionStart, selectionEnd);
			if (cropRectangle.Width > 0 && cropRectangle.Height > 0)
			{
				lbpx.Text = pictureBox1.Image.Width + "x" + pictureBox1.Image.Height;
				Rectangle imageRectangle = ConvertDisplayRectangleToImageRectangle(cropRectangle);
				if (imageRectangle == Rectangle.Empty)
				{
					return;
				}
				vistapre.SizeMode = PictureBoxSizeMode.Zoom;
				Bitmap currentImg = pictureBox1.Image as Bitmap;
				if (currentImg != null)
				{
					vistapre.Image = currentImg.Clone(imageRectangle, currentImg.PixelFormat);
				}
				lbpxrc.Text = vistapre.Image.Width + "x" + vistapre.Image.Height;
				if (trZoomPreview != null) { trZoomPreview.Value = 100; UpdatePreviewZoom(null, null); }
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

	private void CommitText()
	{
		TextBox tb = floatingTextBox;
		if (tb != null && tb.Visible)
		{
			floatingTextBox = null;
			if (!string.IsNullOrWhiteSpace(tb.Text) && pictureBox1.Image != null)
			{
				SaveState();
				Bitmap temp = (Bitmap)pictureBox1.Image.Clone();
				using (Graphics g = Graphics.FromImage(temp))
				{
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					Point imgPt = ConvertDisplayPointToImagePoint(tb.Location);
					using (Brush b = new SolidBrush(tb.ForeColor))
					{
						g.DrawString(tb.Text, tb.Font, b, imgPt);
					}
				}
				Image old = pictureBox1.Image;
				pictureBox1.Image = temp;
				if (old != null && old != originalImage && old != currentDrawingBitmap) old.Dispose();
			}
			tb.Visible = false;
			pictureBox1.Controls.Remove(tb);
			tb.Dispose();
		}
	}

	private Button CrearBotonFiltro(string texto, EventHandler onClick)
	{
		Button b = new Button { Text = texto, Size = new Size(100, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
		b.Click += onClick;
		return b;
	}

	private void AplicarFiltroColor(System.Drawing.Imaging.ColorMatrix matrix)
	{
		if (pictureBox1.Image == null) return;
		SaveState();
		
		Bitmap bmp = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
		using (Graphics g = Graphics.FromImage(bmp))
		using (System.Drawing.Imaging.ImageAttributes attrs = new System.Drawing.Imaging.ImageAttributes())
		{
			attrs.SetColorMatrix(matrix);
			g.DrawImage(pictureBox1.Image, new Rectangle(0, 0, bmp.Width, bmp.Height),
				0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height, GraphicsUnit.Pixel, attrs);
		}
		Image old = pictureBox1.Image;
		pictureBox1.Image = bmp;
		if (old != null && old != originalImage && old != currentDrawingBitmap) old.Dispose();
	}

	private System.Drawing.Imaging.ColorMatrix CreateGrayscaleMatrix()
	{
		return new System.Drawing.Imaging.ColorMatrix(new float[][] {
			new float[] { .3f, .3f, .3f, 0, 0 },
			new float[] { .59f, .59f, .59f, 0, 0 },
			new float[] { .11f, .11f, .11f, 0, 0 },
			new float[] { 0, 0, 0, 1, 0 },
			new float[] { 0, 0, 0, 0, 1 }
		});
	}

	private System.Drawing.Imaging.ColorMatrix CreateSepiaMatrix()
	{
		return new System.Drawing.Imaging.ColorMatrix(new float[][] {
			new float[] { .393f, .349f, .272f, 0, 0 },
			new float[] { .769f, .686f, .534f, 0, 0 },
			new float[] { .189f, .168f, .131f, 0, 0 },
			new float[] { 0, 0, 0, 1, 0 },
			new float[] { 0, 0, 0, 0, 1 }
		});
	}

	private System.Drawing.Imaging.ColorMatrix CreateInvertMatrix()
	{
		return new System.Drawing.Imaging.ColorMatrix(new float[][] {
			new float[] { -1, 0, 0, 0, 0 },
			new float[] { 0, -1, 0, 0, 0 },
			new float[] { 0, 0, -1, 0, 0 },
			new float[] { 0, 0, 0, 1, 0 },
			new float[] { 1, 1, 1, 0, 1 }
		});
	}

	private System.Drawing.Imaging.ColorMatrix CreateBrightnessMatrix(float b)
	{
		return new System.Drawing.Imaging.ColorMatrix(new float[][] {
			new float[] { 1, 0, 0, 0, 0 },
			new float[] { 0, 1, 0, 0, 0 },
			new float[] { 0, 0, 1, 0, 0 },
			new float[] { 0, 0, 0, 1, 0 },
			new float[] { b, b, b, 0, 1 }
		});
	}

	private System.Drawing.Imaging.ColorMatrix CreateContrastMatrix(float c)
	{
		float t = (1.0f - c) / 2.0f;
		return new System.Drawing.Imaging.ColorMatrix(new float[][] {
			new float[] { c, 0, 0, 0, 0 },
			new float[] { 0, c, 0, 0, 0 },
			new float[] { 0, 0, c, 0, 0 },
			new float[] { 0, 0, 0, 1, 0 },
			new float[] { t, t, t, 0, 1 }
		});
	}

	private void rbredi_CheckedChanged(object sender, EventArgs e)
	{
		if (rbredi.Checked)
		{
			recor = false;
			redimen = true;
			isSelecting = false;
			isPanningImage = false;
			UpdatePictureCursor();
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
		isSelecting = false;
		isPanningImage = false;
		pictureBox1.Cursor = Cursors.Default;
		imageCanvas.Size = new Size(Math.Max(1, panel1.ClientSize.Width - 2), Math.Max(1, panel1.ClientSize.Height - p2 - 2));
		imageCanvas.Location = new Point(0, p2);
		pictureBox1.Size = new Size(xori, yori);
		pictureBox1.Location = new Point(p1, p2);
		panel1.AutoScrollPosition = new Point(0, 0);
		cropZoom = DefaultCropZoom;
		cropBaseSize = Size.Empty;
		trzoom.Value = DefaultCropZoom;
		lbzoom.Text = DefaultCropZoom + "%";
		txtaltura.Text = "";
		txtancho.Text = "";
		btguardar.Enabled = false;
		btvisua.Enabled = false;
		btzomin.Enabled = false;
		btzomout.Enabled = false;
		lbpx.Text = "n/a";
		lbpix2.Text = "n/a";
		lbpxrc.Text = "n/a";
		ToggleEmptyState(true);
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
			isSelecting = false;
			isPanningImage = false;
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
			UpdatePictureCursor();
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
				ToggleEmptyState(false);
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
		this.panel1 = new RoundedPanel();
		this.imageCanvas = new System.Windows.Forms.Panel();
		this.lbaviso = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lbpix2 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new RoundedPanel();
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
		this.panel3 = new RoundedPanel();
		this.btzomout = new System.Windows.Forms.Button();
		this.panel4 = new RoundedPanel();
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
		this.trzoom = new System.Windows.Forms.TrackBar();
		this.lbzoom = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.imageCanvas.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.vistapre).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.trzoom).BeginInit();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.lbpix2);
		this.panel1.Controls.Add(this.imageCanvas);
		this.panel1.Location = new System.Drawing.Point(12, 65);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(433, 520);
		this.panel1.TabIndex = 1;
		this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(zoomSurface_MouseWheel);
		this.imageCanvas.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.imageCanvas.Controls.Add(this.pictureBox1);
		this.imageCanvas.Controls.Add(this.lbaviso);
		this.imageCanvas.Location = new System.Drawing.Point(0, 18);
		this.imageCanvas.Name = "imageCanvas";
		this.imageCanvas.Size = new System.Drawing.Size(431, 500);
		this.imageCanvas.TabIndex = 15;
		this.imageCanvas.MouseWheel += new System.Windows.Forms.MouseEventHandler(zoomSurface_MouseWheel);
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
		this.pictureBox1.TabStop = true;
		this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(pictureBox1_DragDrop);
		this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(pictureBox1_DragEnter);
		this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(pictureBox1_Paint);
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
		this.pictureBox1.MouseEnter += new System.EventHandler(pictureBox1_MouseEnter);
		this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseMove);
		this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseUp);
		this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseWheel);
		this.label1.AutoSize = true;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(304, 27);
		this.label1.TabIndex = 2;
		this.label1.Text = "Redimensionador de imagenes";
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.Controls.Add(this.lbzoom);
		this.panel2.Controls.Add(this.trzoom);
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
		this.trzoom.Location = new System.Drawing.Point(174, 35);
		this.trzoom.Maximum = 400;
		this.trzoom.Minimum = 25;
		this.trzoom.Name = "trzoom";
		this.trzoom.Size = new System.Drawing.Size(94, 45);
		this.trzoom.SmallChange = 10;
		this.trzoom.TabIndex = 9;
		this.trzoom.TickFrequency = 25;
		this.trzoom.Value = 100;
		this.trzoom.Scroll += new System.EventHandler(trzoom_Scroll);
		this.lbzoom.AutoSize = true;
		this.lbzoom.Location = new System.Drawing.Point(196, 79);
		this.lbzoom.Name = "lbzoom";
		this.lbzoom.Size = new System.Drawing.Size(33, 13);
		this.lbzoom.TabIndex = 10;
		this.lbzoom.Text = "100%";
		this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button3.Location = new System.Drawing.Point(134, 65);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(35, 25);
		this.button3.TabIndex = 8;
		this.button3.Text = "<-";
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
		this.lbruta.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lbruta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbruta.Location = new System.Drawing.Point(87, 588);
		this.lbruta.Name = "lbruta";
		this.lbruta.Size = new System.Drawing.Size(24, 13);
		this.lbruta.TabIndex = 9;
		this.lbruta.Text = "n/a";
		this.label3.AutoSize = true;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label3.Location = new System.Drawing.Point(14, 588);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 13);
		this.label3.TabIndex = 6;
		this.label3.Text = "Ruta Origen:";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
		this.button5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
		this.button6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
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
		base.ControlBox = true;
		base.Controls.Add(this.button5);
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.lbruta);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
		base.MaximizeBox = true;
		base.MinimumSize = new System.Drawing.Size(900, 698);
		base.Name = "redimension";
		this.Text = "Edicion de Imagenes";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(redimension_FormClosing);
		base.Load += new System.EventHandler(redimension_Load);
		base.Resize += new System.EventHandler(redimension_Resize);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.imageCanvas.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.vistapre).EndInit();
		((System.ComponentModel.ISupportInitialize)this.trzoom).EndInit();
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

    private RoundedPanel panelHerramientas;
    private RoundedPanel panelFiltros;
    public enum ToolMode { Select, Pencil, Line, Rectangle, Circle, Text }
    private ToolMode currentTool = ToolMode.Select;
    private Stack<Bitmap> undoStack = new Stack<Bitmap>();
    private Stack<Bitmap> redoStack = new Stack<Bitmap>();
    private Bitmap currentDrawingBitmap = null;
    private Point drawingStartPoint;
    private bool isDrawing = false;
    private TextBox floatingTextBox;

    private Button btnSeleccion, btnTexto, btnPincel, btnRectangulo, btnCirculo, btnLinea, btnFlecha, btnDeshacer, btnRehacer, btnRestaurar;
    private ComboBox cbFuenteTexto;
    private Panel colorTexto, colorForma, colorRelleno;
    private TrackBar trGrosorPincel, trFiltroIntensidad;
    private Button[] btnFiltros = new Button[8];

    #region History and Drawing Tools

    private void SaveState()
    {
        if (pictureBox1.Image == null) return;
        undoStack.Push((Bitmap)pictureBox1.Image.Clone());
        if (undoStack.Count > 15)
        {
            var arr = undoStack.ToArray();
            undoStack.Clear();
            for (int i = 13; i >= 0; i--) undoStack.Push(arr[i]);
        }
        redoStack.Clear();
        UpdateHistoryButtons();
    }

    private void Undo()
    {
        if (undoStack.Count > 0)
        {
            if (pictureBox1.Image != null) redoStack.Push((Bitmap)pictureBox1.Image.Clone());
            pictureBox1.Image = undoStack.Pop();
            UpdateHistoryButtons();
        }
    }

    private void Redo()
    {
        if (redoStack.Count > 0)
        {
            if (pictureBox1.Image != null) undoStack.Push((Bitmap)pictureBox1.Image.Clone());
            pictureBox1.Image = redoStack.Pop();
            UpdateHistoryButtons();
        }
    }

    private void RestoreOriginal()
    {
        if (originalImage != null)
        {
            SaveState();
            pictureBox1.Image = (Image)originalImage.Clone();
            UpdateHistoryButtons();
        }
    }

    private void UpdateHistoryButtons()
    {
        if (btnDeshacer != null) btnDeshacer.Enabled = undoStack.Count > 0;
        if (btnRehacer != null) btnRehacer.Enabled = redoStack.Count > 0;
    }
    
    private void UpdateToolSelectionUI()
    {
        Button[] btns = { btnSeleccion, btnPincel, btnLinea, btnRectangulo, btnCirculo, btnTexto };
        foreach (var b in btns) {
            if (b != null) {
                b.BackColor = Color.White;
                b.ForeColor = Color.Black;
            }
        }
        
        Button activeBtn = null;
        switch (currentTool) {
            case ToolMode.Select: activeBtn = btnSeleccion; break;
            case ToolMode.Pencil: activeBtn = btnPincel; break;
            case ToolMode.Line: activeBtn = btnLinea; break;
            case ToolMode.Rectangle: activeBtn = btnRectangulo; break;
            case ToolMode.Circle: activeBtn = btnCirculo; break;
            case ToolMode.Text: activeBtn = btnTexto; break;
        }
        
        if (activeBtn != null) {
            activeBtn.BackColor = Color.FromArgb(108, 99, 255);
            activeBtn.ForeColor = Color.White;
        }
        
        if (currentTool != ToolMode.Select) {
            isPanningImage = false;
        }
    }

    private void SetupDrawingToolsEvents()
    {
        btnSeleccion.Click += (s, e) => { currentTool = ToolMode.Select; UpdateToolSelectionUI(); };
        btnPincel.Click += (s, e) => { currentTool = ToolMode.Pencil; UpdateToolSelectionUI(); };
        btnLinea.Click += (s, e) => { currentTool = ToolMode.Line; UpdateToolSelectionUI(); };
        btnRectangulo.Click += (s, e) => { currentTool = ToolMode.Rectangle; UpdateToolSelectionUI(); };
        btnCirculo.Click += (s, e) => { currentTool = ToolMode.Circle; UpdateToolSelectionUI(); };
        btnTexto.Click += (s, e) => { currentTool = ToolMode.Text; UpdateToolSelectionUI(); };

        btnDeshacer.Click += (s, e) => Undo();
        btnRehacer.Click += (s, e) => Redo();
        btnRestaurar.Click += (s, e) => RestoreOriginal();

        colorForma.Click += (s, e) => { using (ColorDialog cd = new ColorDialog { Color = colorForma.BackColor }) { if (cd.ShowDialog() == DialogResult.OK) colorForma.BackColor = cd.Color; } };
        colorRelleno.Click += (s, e) => { using (ColorDialog cd = new ColorDialog { Color = colorRelleno.BackColor }) { if (cd.ShowDialog() == DialogResult.OK) colorRelleno.BackColor = cd.Color; } };
        
        UpdateToolSelectionUI();
        UpdateHistoryButtons();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.Z)) { Undo(); return true; }
        if (keyData == (Keys.Control | Keys.Y)) { Redo(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    #endregion

    private Panel pnlEmptyState;
    
    private void ToggleEmptyState(bool show)
    {
        if (lbaviso != null) { lbaviso.Visible = show; }
        if (pnlEmptyState != null) { pnlEmptyState.Visible = show; }
    }

    private void InitEmptyState(Panel parentPanel)
    {
        pnlEmptyState = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(13, 17, 23) };
        parentPanel.Controls.Add(pnlEmptyState);
        pnlEmptyState.BringToFront();

        PictureBox pbGif = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.StretchImage };
        string gifPath = System.IO.Path.Combine(Application.StartupPath, "..", "..", "Assets", "waves_stars.gif");
        if (System.IO.File.Exists(gifPath)) pbGif.Image = Image.FromFile(gifPath);
        else if (System.IO.File.Exists("Assets\\waves_stars.gif")) pbGif.Image = Image.FromFile("Assets\\waves_stars.gif");
        pnlEmptyState.Controls.Add(pbGif);

        Panel pnlDashed = new Panel { Size = new Size(360, 260) };
        pbGif.Controls.Add(pnlDashed);
        pnlDashed.BackColor = Color.Transparent;

        Action centerDashed = () => { pnlDashed.Location = new Point((pbGif.Width - pnlDashed.Width) / 2, (pbGif.Height - pnlDashed.Height) / 2); };
        pbGif.Resize += (s, e) => centerDashed();
        centerDashed();

        pnlDashed.Paint += (s, e) => {
            using (Pen pen = new Pen(Color.FromArgb(100, 255, 255, 255), 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            {
                e.Graphics.DrawRectangle(pen, 0, 0, pnlDashed.Width - 1, pnlDashed.Height - 1);
            }
        };

        Label lblIcon = new Label { Text = "🖼️", Font = new Font("Segoe UI", 36), AutoSize = true, ForeColor = Color.White };
        pnlDashed.Controls.Add(lblIcon);
        lblIcon.Location = new Point((pnlDashed.Width - 60) / 2, 30);

        Label lblTitle = new Label { Text = "Carga tu imagen", Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, ForeColor = Color.White };
        pnlDashed.Controls.Add(lblTitle);
        lblTitle.Location = new Point((pnlDashed.Width - 180) / 2, 100);

        Label lblSub = new Label { Text = "Presiona el botón para seleccionar\no arrastra tu imagen aquí", Font = new Font("Segoe UI", 10), AutoSize = true, ForeColor = Color.LightGray, TextAlign = ContentAlignment.MiddleCenter };
        pnlDashed.Controls.Add(lblSub);
        lblSub.Location = new Point((pnlDashed.Width - 230) / 2, 140);

        Button btnSelect = new Button { Text = "📁 Seleccionar Imagen", Size = new Size(200, 40), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = Color.FromArgb(40, 20, 80), Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
        btnSelect.FlatAppearance.BorderColor = Color.FromArgb(100, 50, 150);
        btnSelect.FlatAppearance.BorderSize = 2;
        pnlDashed.Controls.Add(btnSelect);
        btnSelect.Location = new Point((pnlDashed.Width - btnSelect.Width) / 2, 190);

        btnSelect.Click += (s, e) => {
            button1_Click(s, e);
        };

        DragEventHandler dEnter = (s, e) => { if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; };
        DragEventHandler dDrop = (s, e) => {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0 && IsImageFile(files[0]))
            {
                pictureBox1.Image = Image.FromFile(files[0]);
                lbruta.Text = files[0];
                CargarImagen(files[0]);
                ToggleEmptyState(false);
            }
        };

        pbGif.AllowDrop = true;
        pnlDashed.AllowDrop = true;
        pbGif.DragEnter += dEnter;
        pbGif.DragDrop += dDrop;
        pnlDashed.DragEnter += dEnter;
        pnlDashed.DragDrop += dDrop;
    }

    private void ApplyModernStyles()
    {
        this.MouseDown += new MouseEventHandler(DragForm_MouseDown);
        
        Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
        Font subtitleFont = new Font("Segoe UI", 12, FontStyle.Bold);
        Font regularFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        Font smallFont = new Font("Segoe UI", 8.5f, FontStyle.Regular);

        Color primaryColor = Color.FromArgb(103, 80, 164);
        Color secondaryColor = Color.White;
        Color formBg = Color.FromArgb(245, 246, 248);
        Color textColor = Color.FromArgb(40, 40, 40);
        Color borderLight = Color.FromArgb(222, 226, 230);

        this.BackColor = formBg;
        this.ClientSize = new Size(1300, 850);
        this.MinimumSize = new Size(1200, 750);

        // Titles and Bottom buttons
        if (label1 != null) { label1.Visible = false; }
        Label titleLabel = new Label { Text = "Edición de Imágenes", Font = titleFont, ForeColor = textColor, AutoSize = true, Location = new Point(40, 15) };
        this.Controls.Add(titleLabel);
        
        Label subTitleLabel = new Label { Text = "Redimensiona, recorta, agrega texto, aplica filtros y personaliza tu imagen.", Font = regularFont, ForeColor = Color.Gray, AutoSize = true, Location = new Point(40, 45) };
        this.Controls.Add(subTitleLabel);

        if (button6 != null) { button6.BackColor = primaryColor; button6.ForeColor = secondaryColor; button6.Location = new Point(this.ClientSize.Width - 150, 20); button6.Size = new Size(120, 35); button6.FlatAppearance.BorderSize = 0; button6.Text = "Volver al inicio"; button6.Anchor = AnchorStyles.Top | AnchorStyles.Right; }
        
        if (button5 != null) { button5.Location = new Point(this.ClientSize.Width - 130, this.ClientSize.Height - 50); button5.Size = new Size(100, 35); button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; }
        if (button4 != null) { button4.Location = new Point(this.ClientSize.Width - 240, this.ClientSize.Height - 50); button4.Size = new Size(100, 35); button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; }

        Panel pnlRuta = new Panel { Size = new Size(540, 40), Location = new Point(40, this.ClientSize.Height - 50), BackColor = formBg, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        this.Controls.Add(pnlRuta);
        if (label3 != null) { label3.Location = new Point(0, 10); label3.Parent = pnlRuta; label3.Font = new Font("Segoe UI", 9f, FontStyle.Bold); } 
        if (lbruta != null) { lbruta.AutoSize = false; lbruta.Size = new Size(420, 20); lbruta.AutoEllipsis = true; lbruta.Location = new Point(90, 10); lbruta.Parent = pnlRuta; } 
        if (lbaviso != null) { lbaviso.Location = new Point(40, this.ClientSize.Height - 20); lbaviso.AutoSize = true; lbaviso.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; }

        // Grid Layout
        TableLayoutPanel tlpMain = new TableLayoutPanel { Location = new Point(40, 80), Size = new Size(this.ClientSize.Width - 80, this.ClientSize.Height - 140), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, ColumnCount = 2, RowCount = 1 };
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63f));
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37f));
        
        TableLayoutPanel tlpLeft = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = new Padding(0) };
        tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 75f));
        tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
        tlpMain.Controls.Add(tlpLeft, 0, 0);

        TableLayoutPanel tlpRight = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = new Padding(15, 0, 0, 0) };
        tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
        tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
        tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        tlpMain.Controls.Add(tlpRight, 1, 0);

        tlpMain.SuspendLayout();
        this.Controls.Add(tlpMain);

        // LEFT 1: Canvas
        if (panel1 != null) { 
            panel1.Size = new Size(1000, 1000); panel1.BackColor = secondaryColor; panel1.Dock = DockStyle.Fill; panel1.Margin = new Padding(0, 0, 0, 15); panel1.BorderStyle = BorderStyle.None; panel1.BorderColor = borderLight; panel1.BorderThickness = 1; 
            tlpLeft.Controls.Add(panel1, 0, 0);
            
            Panel bottomBar = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = secondaryColor };
            panel1.Controls.Add(bottomBar);

            if (imageCanvas != null) { imageCanvas.Dock = DockStyle.Fill; imageCanvas.BackColor = Color.LightGray; imageCanvas.Location = new Point(0, 0); imageCanvas.BringToFront(); }
            
            if (trzoom != null) { trzoom.Parent = bottomBar; trzoom.Dock = DockStyle.Fill; trzoom.TickStyle = TickStyle.None; trzoom.Minimum = 10; trzoom.Maximum = 500; }
            if (lbzoom != null) { lbzoom.Parent = bottomBar; lbzoom.Dock = DockStyle.Right; lbzoom.TextAlign = ContentAlignment.MiddleCenter; lbzoom.AutoSize = false; lbzoom.Width = 50; }
            Button btnAjustar = new Button { Text = "Ajustar", Dock = DockStyle.Right, Width = 80, FlatStyle = FlatStyle.Flat, ForeColor = textColor, BackColor = secondaryColor, Font = regularFont };
            btnAjustar.FlatAppearance.BorderColor = borderLight;
            bottomBar.Controls.Add(btnAjustar);
            bottomBar.Controls.Add(lbzoom);
            bottomBar.Controls.Add(trzoom);
            btnAjustar.BringToFront();
            lbzoom.BringToFront();
            lbzoom.BringToFront();
            
            InitEmptyState(panel1);
            ToggleEmptyState(pictureBox1.Image == null);
        }

        // LEFT 2: Tools
        panelHerramientas = new RoundedPanel { Size = new Size(1000, 1000), BackColor = secondaryColor, Dock = DockStyle.Fill, Margin = new Padding(0), BorderColor = borderLight, BorderThickness = 1 };
        tlpLeft.Controls.Add(panelHerramientas, 0, 1);
        Label lblTools = new Label { Text = " 🛠 Herramientas de Dibujo", Font = subtitleFont, Location = new Point(20, 15), AutoSize = true, ForeColor = primaryColor };
        panelHerramientas.Controls.Add(lblTools);

        int btnY = 50;
        btnSeleccion = new Button { Text = "↖ Selec", Location = new Point(20, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = primaryColor, ForeColor = Color.White, Cursor = Cursors.Hand };
        btnPincel = new Button { Text = "🖌 Pincel", Location = new Point(105, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnLinea = new Button { Text = "📏 Línea", Location = new Point(190, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnRectangulo = new Button { Text = "⬛ Rect", Location = new Point(275, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnCirculo = new Button { Text = "⏺ Círc", Location = new Point(360, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnTexto = new Button { Text = "📝 Texto", Location = new Point(445, btnY), Size = new Size(80, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };

        int propY = 90;
        Label lblGrosor = new Label { Text = "Grosor:", Location = new Point(20, propY + 5), AutoSize = true, ForeColor = textColor };
        trGrosorPincel = new TrackBar { Location = new Point(70, propY), Minimum = 1, Maximum = 20, Value = 3, Size = new Size(120, 30), TickStyle = TickStyle.None };
        
        Label lblColor = new Label { Text = "Borde:", Location = new Point(200, propY + 5), AutoSize = true, ForeColor = textColor };
        colorForma = new Panel { Location = new Point(250, propY + 2), Size = new Size(25, 25), BackColor = Color.Black, BorderStyle = BorderStyle.FixedSingle, Cursor = Cursors.Hand };
        
        Label lblRelleno = new Label { Text = "Relleno:", Location = new Point(290, propY + 5), AutoSize = true, ForeColor = textColor };
        colorRelleno = new Panel { Location = new Point(350, propY + 2), Size = new Size(25, 25), BackColor = Color.Transparent, BorderStyle = BorderStyle.FixedSingle, Cursor = Cursors.Hand };
        
        Label lblFuente = new Label { Text = "Fuente:", Location = new Point(390, propY + 5), AutoSize = true, ForeColor = textColor };
        cbFuenteTexto = new ComboBox { Location = new Point(445, propY + 2), Size = new Size(130, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cbFuenteTexto.Items.AddRange(new object[] { "Arial", "Times New Roman", "Consolas", "Segoe UI", "Verdana" });
        cbFuenteTexto.SelectedIndex = 0;

        btnDeshacer = new Button { Text = "↩ Deshacer", Location = new Point(20, 130), Size = new Size(100, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnRehacer = new Button { Text = "↪ Rehacer", Location = new Point(125, 130), Size = new Size(100, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
        btnRestaurar = new Button { Text = "⏮ Original", Location = new Point(230, 130), Size = new Size(100, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };

        panelHerramientas.Controls.AddRange(new Control[] { btnSeleccion, btnPincel, btnLinea, btnRectangulo, btnCirculo, btnTexto, lblGrosor, trGrosorPincel, lblColor, colorForma, lblRelleno, colorRelleno, lblFuente, cbFuenteTexto, btnDeshacer, btnRehacer, btnRestaurar });

        // RIGHT 1: Ajustes moved to panelHerramientas
        int ajustX = 590; // offset right
        Label lblAjustes = new Label { Text = "Ajustes de Imagen", Font = subtitleFont, Location = new Point(ajustX, 15), AutoSize = true, ForeColor = textColor };
        panelHerramientas.Controls.Add(lblAjustes);
        
        if (rbredi != null) { panelHerramientas.Controls.Add(rbredi); rbredi.Location = new Point(ajustX, 45); rbredi.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); rbredi.ForeColor = textColor; }
        if (label6 != null) { panelHerramientas.Controls.Add(label6); label6.Location = new Point(ajustX + 20, 70); label6.ForeColor = textColor; label6.AutoSize = true; } // Ancho
        if (txtancho != null) { panelHerramientas.Controls.Add(txtancho); txtancho.Location = new Point(ajustX + 20, 87); txtancho.Size = new Size(50, 25); }
        if (label7 != null) { panelHerramientas.Controls.Add(label7); label7.Location = new Point(ajustX + 80, 70); label7.ForeColor = textColor; label7.AutoSize = true; } // Altura
        if (txtaltura != null) { panelHerramientas.Controls.Add(txtaltura); txtaltura.Location = new Point(ajustX + 80, 87); txtaltura.Size = new Size(50, 25); }
        if (button3 != null) { panelHerramientas.Controls.Add(button3); button3.Location = new Point(ajustX + 135, 85); button3.Size = new Size(25, 27); button3.Text = "🗑"; button3.FlatStyle = FlatStyle.Flat; button3.BackColor = secondaryColor; button3.FlatAppearance.BorderColor = borderLight; }
        
        if (btapli != null) { panelHerramientas.Controls.Add(btapli); btapli.Location = new Point(ajustX + 20, 125); btapli.Size = new Size(70, 30); }
        if (btcancel != null) { panelHerramientas.Controls.Add(btcancel); btcancel.Location = new Point(ajustX + 100, 125); btcancel.Size = new Size(80, 30); }
        
        if (rbrecorte != null) { panelHerramientas.Controls.Add(rbrecorte); rbrecorte.Location = new Point(ajustX + 210, 45); rbrecorte.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); rbrecorte.ForeColor = textColor; }
        if (button1 != null) { panelHerramientas.Controls.Add(button1); button1.Location = new Point(ajustX + 210, 75); button1.Size = new Size(110, 30); button1.Text = "Cancelar Edicion"; }

        if (panel2 != null) panel2.Visible = false;
        tlpRight.RowStyles[0] = new RowStyle(SizeType.Absolute, 0);

        // RIGHT 2: Filtros
        panelFiltros = new RoundedPanel { Size = new Size(1000, 1000), BackColor = secondaryColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 15), BorderColor = borderLight, BorderThickness = 1 };
        tlpRight.Controls.Add(panelFiltros, 0, 1);
        Label lblFiltros = new Label { Text = "✨ Filtros", Font = subtitleFont, Location = new Point(15, 10), AutoSize = true, ForeColor = textColor };
        panelFiltros.Controls.Add(lblFiltros);
        
        FlowLayoutPanel flpFiltros = new FlowLayoutPanel { Location = new Point(15, 45), Size = new Size(350, 150), AutoScroll = true };
        panelFiltros.Controls.Add(flpFiltros);
        
        btnFiltros[0] = CrearBotonFiltro("🌫 Grises", (s, e) => AplicarFiltroColor(CreateGrayscaleMatrix()));
        btnFiltros[1] = CrearBotonFiltro("🎞 Sepia", (s, e) => AplicarFiltroColor(CreateSepiaMatrix()));
        btnFiltros[2] = CrearBotonFiltro("🌙 Invertir", (s, e) => AplicarFiltroColor(CreateInvertMatrix()));
        btnFiltros[3] = CrearBotonFiltro("🔆 + Brillo", (s, e) => AplicarFiltroColor(CreateBrightnessMatrix(0.1f)));
        btnFiltros[4] = CrearBotonFiltro("🔅 - Brillo", (s, e) => AplicarFiltroColor(CreateBrightnessMatrix(-0.1f)));
        btnFiltros[5] = CrearBotonFiltro("➕ + Contraste", (s, e) => AplicarFiltroColor(CreateContrastMatrix(1.2f)));
        btnFiltros[6] = CrearBotonFiltro("➖ - Contraste", (s, e) => AplicarFiltroColor(CreateContrastMatrix(0.8f)));
        
        foreach (var b in btnFiltros) if (b != null) flpFiltros.Controls.Add(b);

        // RIGHT 3: Vista Previa
        if (panel3 != null) { 
            panel3.Size = new Size(1000, 1000); panel3.BackColor = secondaryColor; panel3.Dock = DockStyle.Fill; panel3.Margin = new Padding(0); panel3.BorderStyle = BorderStyle.None; panel3.BorderColor = borderLight; panel3.BorderThickness = 1; 
            tlpRight.Controls.Add(panel3, 0, 2);

            if (label2 != null) { label2.Location = new Point(15, 10); label2.Font = subtitleFont; label2.ForeColor = primaryColor; }
            if (panel4 != null) { 
                panel4.BackColor = Color.LightGray; panel4.Location = new Point(15, 40); panel4.Size = new Size(panel3.Width - 30, panel3.Height - 90); panel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; 
                panel4.AutoScroll = true;
                panel4.Resize += (s, e) => CenterPreviewImage();
                if (vistapre != null) { vistapre.Dock = DockStyle.None; vistapre.SizeMode = PictureBoxSizeMode.Zoom; vistapre.BackColor = Color.LightGray; }
                if (lbpxrc != null) { lbpxrc.Parent = panel3; lbpxrc.Location = new Point(15, panel3.Height - 65); lbpxrc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; lbpxrc.BringToFront(); lbpxrc.BackColor = Color.Transparent; }
                if (lbpx != null) { lbpx.Parent = panel3; lbpx.Location = new Point(15, 15); lbpx.Anchor = AnchorStyles.Top | AnchorStyles.Left; lbpx.BringToFront(); lbpx.BackColor = Color.Transparent; }
            }
            
            if (btguardar != null) { btguardar.Location = new Point(15, panel3.Height - 40); btguardar.Size = new Size(90, 30); btguardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; }
            if (btvisua != null) { btvisua.Location = new Point(115, panel3.Height - 40); btvisua.Size = new Size(120, 30); btvisua.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; }
            
            trZoomPreview = new TrackBar { Parent = panel3, Minimum = 10, Maximum = 500, Value = 100, TickStyle = TickStyle.None, Location = new Point(245, panel3.Height - 45), Size = new Size(100, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, BackColor = secondaryColor };
            trZoomPreview.Scroll += UpdatePreviewZoom;
            lbZoomPreview = new Label { Parent = panel3, Text = "100%", Location = new Point(350, panel3.Height - 35), AutoSize = true, Anchor = AnchorStyles.Bottom | AnchorStyles.Left, ForeColor = textColor, BackColor = secondaryColor };
            
            if (btzomin != null) btzomin.Visible = false;
            if (btzomout != null) btzomout.Visible = false;
        }

        // Apply styles
        Button[] primaryButtons = { btapli, btguardar }; 
        foreach (var b in primaryButtons) { if (b != null) { b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0; b.BackColor = primaryColor; b.ForeColor = secondaryColor; b.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); b.Cursor = Cursors.Hand; } }
        
        Button[] secondaryButtons = { btcancel, button1, btvisua, button4, button5 }; 
        foreach (var b in secondaryButtons) { if (b != null) { b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderColor = borderLight; b.FlatAppearance.BorderSize = 1; b.BackColor = secondaryColor; b.ForeColor = textColor; b.Font = new Font("Segoe UI", 9f, FontStyle.Regular); b.Cursor = Cursors.Hand; } }

        tlpMain.ResumeLayout(false);
    }

}

