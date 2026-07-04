// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.OCR
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvertidorImagenes;
using ConvertidorImagenes.Properties;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ConvertidorImagenes
{
public class OCR : Form
{
	private string nombreft = "";

	private string rtdestino = "C:\\Users\\" + Environment.UserName + "\\Desktop";

	private string ext = "";

	private string ruta2 = "";

	public bool img = true;

	public bool pdf = false;

	public bool paginas = false;

	public bool ingresado = false;

	public bool visor = false;

	public bool completado = true;

	private int pagini = 1;

	private PictureBox pictureBox;

	private CancellationTokenSource can;

	private IContainer components = null;

	private Panel panel1;

	private RadioButton rbpdf;

	private RadioButton rbima;

	private Panel panel2;

	private Label label1;

	private Label lbruta;

	private Label label3;

	private Panel panel4;

	private Label label6;

	private Panel panel3;

	private Label label5;

	private Button button4;

	private Label label7;

	private Button btcopiar;

	private Button button2;

	private Panel panel5;

	private Button btlect;

	private CheckBox chpag;

	public ProgressBar bar3;

	public Label lbbar;

	public Label label2;

	public RichTextBox txtresult;

	public Button btconver;

	public Button btcan;

	private Button btpegar;

	private Button button3;

	private Button btguarda;

	private Label label8;

	private Label label4;

	private Label lbfia;

	private Label label9;

	private Label label10;

	private ComboBox cboidioma;

	public OCR()
	{
		InitializeComponent();
		btconver.Enabled = false;
		txtresult.ReadOnly = true;
		Control.CheckForIllegalCrossThreadCalls = false;
		pictureBox = new PictureBox();
		btlect.Enabled = false;
		btcan.Enabled = false;
		btpegar.Enabled = false;
		base.KeyPreview = true;
		label2.Show();
		btcopiar.Enabled = false;
		btguarda.Enabled = false;
		chpag.Enabled = false;
       cboidioma.Items.Clear();
		cboidioma.Items.Add("Español");
		cboidioma.Items.Add("Inglés");
		if (File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata", "spa.traineddata")))
		{
			cboidioma.SelectedItem = "Español";
		}
		else
		{
			cboidioma.SelectedItem = "Inglés";
		}
	}

	private void panel2_DragEnter(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		string[] array2 = array;
		foreach (string path in array2)
		{
			if ((System.IO.Path.GetExtension(path).Equals(".jpg") && img) || (System.IO.Path.GetExtension(path).Equals(".jpeg") && img) || (System.IO.Path.GetExtension(path).Equals(".png") && img) || (System.IO.Path.GetExtension(path).Equals(".gif") && img) || (System.IO.Path.GetExtension(path).Equals(".bmp") && img) || (System.IO.Path.GetExtension(path).Equals(".PNG") && img) || (System.IO.Path.GetExtension(path).Equals(".JPG") && img))
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				break;
			}
			if (System.IO.Path.GetExtension(path).Equals(".pdf") && pdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				break;
			}
		}
	}

	private void panel2_DragDrop(object sender, DragEventArgs e)
	{
		if (!completado)
		{
			return;
		}
		object data = e.Data.GetData(DataFormats.FileDrop);
		label2.Hide();
		btconver.Enabled = false;
		if (data == null)
		{
			return;
		}
		string[] nombre = data as string[];
		if (nombre.Length != 0 && img)
		{
			Task.Factory.StartNew(delegate
			{
				lbbar.Text = "Analizando...";
				label2.Text = "Archivo Cargado...";
				panel2.Enabled = false;
				bar3.Value = 50;
				Image image = Image.FromFile(nombre[0]);
				pictureBox = new PictureBox();
				pictureBox.Image = image;
				pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				pictureBox.Dock = DockStyle.Fill;
				panel1.Invoke((Action)delegate
				{
					panel2.Controls.Add(pictureBox);
				});
			});
		}
		if (nombre.Length == 0)
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			string[] array = nombre;
			foreach (string text in array)
			{
				ext = text.Substring(text.LastIndexOf(".") + 1);
				bar3.Value = 70;
				lbruta.Text = nombre[0];
				ingresado = true;
				convertir(img, pdf, nombre[0], ext);
			}
		});
	}

  public void convertir(bool img, bool pdf, string ruta, string ext)
	{
		panel2.Enabled = false;
		btpegar.Enabled = false;
		if (img && !pdf)
		{
			rbpdf.Enabled = false;
			chpag.Enabled = false;
			btlect.Enabled = false;
            string selectedLanguage = GetSelectedLanguageCode();
          Task.Factory.StartNew(delegate
			{
                ProcesarImagenConTesseract(ruta, false, selectedLanguage);
			});
		}

		Task.Factory.StartNew(delegate
		{
			if (!img && pdf)
			{
				if (ingresado && paginas)
				{
					ruta2 = ruta;
					btlect.Enabled = true;
					label2.Text = "Seleccione la cantidad de Paginas a Escanear";
					lbbar.Text = "A espera de Paginas...";
					btcan.Enabled = true;
					rbima.Enabled = false;
					chpag.Enabled = false;
				}
				if (ingresado && !paginas)
				{
					StringBuilder stringBuilder = new StringBuilder();
					try
					{
						using (PdfReader pdfReader = new PdfReader(ruta))
						{
							for (pagini = 1; pagini <= pdfReader.NumberOfPages; pagini++)
							{
								string textFromPage = PdfTextExtractor.GetTextFromPage(pdfReader, pagini, new LocationTextExtractionStrategy());
								stringBuilder.Append(textFromPage);
							}
						}
						bar3.Value = 100;
						lbbar.Text = "Finalizo!";
						label2.Text = "Finalizo!";
						lbfia.Text = "100";
						btconver.Enabled = true;
						btcan.Enabled = false;
						txtresult.Text = stringBuilder.ToString();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}
		});
	}

	public static Image ResizeImage(Image image, int width, int height)
	{
		Bitmap bitmap = new Bitmap(width, height);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.DrawImage(image, new Rectangle(0, 0, width, height));
		}
		return bitmap;
	}

  private void ProcesarImagenConTesseract(string imagePath, bool removeTempFile, string languageCode)
	{
     string enhancedImagePath = null;
		try
		{
			lbbar.Text = "Leyendo...";
			bar3.Value = 90;
			float confidence;
          string textFromImage = LeerTextoConTesseract(imagePath, languageCode, out confidence);

			if (confidence < 90f)
			{
				enhancedImagePath = CreateEnhancedOcrImage(imagePath);
				float enhancedConfidence;
				string enhancedText = LeerTextoConTesseract(enhancedImagePath, languageCode, out enhancedConfidence);
				if (enhancedConfidence > confidence)
				{
					confidence = enhancedConfidence;
					textFromImage = enhancedText;
				}
			}

			bar3.Value = 100;
			txtresult.Text = textFromImage;
			lbfia.Text = confidence.ToString("0.00") + "%";
			lbbar.Text = "Finalizo!";
			btcopiar.Enabled = true;
			btguarda.Enabled = true;
			btconver.Enabled = true;
			bar3.Value = 0;
			completado = false;
			panel2.Enabled = false;
		}
		catch (Exception ex)
		{
            MessageBox.Show("Error! Verifique que el texto de la imagen sea suficientemente claro, y en una resolucion optima.\n" + GetDetailedExceptionMessage(ex));
			lbbar.Text = "Error de Lectura!";
			label2.Text = "Error de Lectura!";
			bar3.Value = 0;
			btconver.Enabled = true;
		}
		finally
		{
         if (!string.IsNullOrEmpty(enhancedImagePath))
			{
				try
				{
					if (File.Exists(enhancedImagePath))
					{
						File.Delete(enhancedImagePath);
					}
				}
				catch
				{
				}
			}

			if (removeTempFile)
			{
				try
				{
					if (File.Exists(imagePath))
					{
						File.Delete(imagePath);
					}
				}
				catch
				{
				}
			}
		}
	}

	private string GetSelectedLanguageCode()
	{
		if (cboidioma != null && cboidioma.SelectedItem != null && cboidioma.SelectedItem.ToString() == "Inglés")
		{
			return "eng";
		}
		return "spa";
	}

 private static string LeerTextoConTesseract(string imagePath, string languageCode, out float confidence)
	{
		Assembly tesseractAssembly = LoadTesseractAssembly();
		Type tesseractEngineType = tesseractAssembly.GetType("Tesseract.TesseractEngine", true);
		Type pixType = tesseractAssembly.GetType("Tesseract.Pix", true);
		Type engineModeType = tesseractAssembly.GetType("Tesseract.EngineMode", true);

		string[] tessdataCandidates = GetTessdataCandidatePaths();

      string language = string.IsNullOrWhiteSpace(languageCode) ? "eng" : languageCode;
      string fallbackLanguage = language == "spa" ? "eng" : "spa";
		string tessdataPath = null;

		for (int i = 0; i < tessdataCandidates.Length; i++)
		{
			if (File.Exists(System.IO.Path.Combine(tessdataCandidates[i], language + ".traineddata")))
			{
				tessdataPath = tessdataCandidates[i];
				break;
			}
		}

		if (string.IsNullOrEmpty(tessdataPath))
		{
            for (int j = 0; j < tessdataCandidates.Length; j++)
			{
                if (File.Exists(System.IO.Path.Combine(tessdataCandidates[j], fallbackLanguage + ".traineddata")))
				{
					language = fallbackLanguage;
					tessdataPath = tessdataCandidates[j];
					break;
				}
			}
        }

		if (string.IsNullOrEmpty(tessdataPath))
		{
			if (tessdataCandidates.Length == 0)
			{
				throw new DirectoryNotFoundException("No se encontro la carpeta tessdata en el directorio de salida ni en el proyecto.");
			}
			else
			{
				throw new FileNotFoundException("No se encontraron archivos de idioma OCR. Faltan: spa.traineddata y eng.traineddata");
			}
		}
		object engineModeDefault = Enum.Parse(engineModeType, "Default");

		object engine = null;
		object pix = null;
		object page = null;
		try
		{
            try
			{
				engine = Activator.CreateInstance(tesseractEngineType, new object[3] { tessdataPath, language, engineModeDefault });
               TryConfigureEngineForAccuracy(engine);
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				}
				throw;
			}

			pix = InvokeStaticMethodByName(pixType, "LoadFromFile", new object[1] { imagePath });
          Type pageSegModeType = tesseractAssembly.GetType("Tesseract.PageSegMode", false);
			object autoPageSegMode = pageSegModeType != null ? Enum.Parse(pageSegModeType, "Auto") : null;
			page = InvokeMethodByName(engine, "Process", new object[2] { pix, autoPageSegMode });
			string text = InvokeMethodByName(page, "GetText", new object[0]) as string;
			object meanConfidence = InvokeMethodByName(page, "GetMeanConfidence", new object[0]);
			confidence = Convert.ToSingle(meanConfidence) * 100f;
			return text ?? string.Empty;
		}
		finally
		{
			CloseOrDispose(page);
			CloseOrDispose(pix);
			CloseOrDispose(engine);
		}
	}

	private static string[] GetTessdataCandidatePaths()
	{
		string[] candidates = new string[3]
		{
			System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"),
			System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\tessdata")),
			System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\tessdata"))
		};

		StringBuilder uniqueBuilder = new StringBuilder();
		for (int i = 0; i < candidates.Length; i++)
		{
			if (Directory.Exists(candidates[i]))
			{
				if (uniqueBuilder.Length > 0)
				{
					uniqueBuilder.Append("|");
				}
				uniqueBuilder.Append(candidates[i]);
			}
		}

		if (uniqueBuilder.Length == 0)
		{
			return new string[0];
		}

		return uniqueBuilder.ToString().Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
	}

	private static void TryConfigureEngineForAccuracy(object engine)
	{
		try
		{
			InvokeMethodByName(engine, "SetVariable", new object[2] { "user_defined_dpi", "300" });
		}
		catch
		{
		}

		try
		{
			InvokeMethodByName(engine, "SetVariable", new object[2] { "preserve_interword_spaces", "1" });
		}
		catch
		{
		}
	}

	private static string CreateEnhancedOcrImage(string imagePath)
	{
		using (Bitmap source = new Bitmap(imagePath))
		{
			int targetWidth = source.Width < 1600 ? source.Width * 2 : source.Width;
			int targetHeight = source.Width < 1600 ? source.Height * 2 : source.Height;

			using (Bitmap scaled = new Bitmap(targetWidth, targetHeight, PixelFormat.Format24bppRgb))
			{
				using (Graphics g = Graphics.FromImage(scaled))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.DrawImage(source, 0, 0, targetWidth, targetHeight);
				}

				using (Bitmap binarized = new Bitmap(targetWidth, targetHeight, PixelFormat.Format24bppRgb))
				{
					for (int y = 0; y < targetHeight; y++)
					{
						for (int x = 0; x < targetWidth; x++)
						{
							Color pixel = scaled.GetPixel(x, y);
							double gray = (pixel.R * 0.299) + (pixel.G * 0.587) + (pixel.B * 0.114);
							gray = (gray - 128d) * 1.25d + 128d;
							byte value = gray >= 150d ? (byte)255 : (byte)0;
							binarized.SetPixel(x, y, Color.FromArgb(value, value, value));
						}
					}

					string enhancedPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N") + "-ocr.png");
					binarized.Save(enhancedPath, ImageFormat.Png);
					return enhancedPath;
				}
			}
		}
	}

	private static string GetDetailedExceptionMessage(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Exception ex2 = ex;
		while (ex2 != null)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" -> ");
			}
			stringBuilder.Append(ex2.Message);
			ex2 = ex2.InnerException;
		}
		return stringBuilder.ToString();
	}

	private static Assembly LoadTesseractAssembly()
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			if (assemblies[i].GetName().Name == "Tesseract")
			{
				return assemblies[i];
			}
		}

     string[] candidates = new string[4]
		{
			System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tesseract.dll"),
           System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\packages\\Tesseract.5.2.0\\lib\\net47\\Tesseract.dll")),
			System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\packages\\Tesseract.5.2.0\\lib\\net47\\Tesseract.dll")),
			System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\packages\\Tesseract.5.2.0\\lib\\net47\\Tesseract.dll"))
		};

		for (int j = 0; j < candidates.Length; j++)
		{
			if (File.Exists(candidates[j]))
			{
                EnsureTesseractNativePath(candidates[j]);
				return Assembly.LoadFrom(candidates[j]);
			}
		}

		throw new FileNotFoundException("No se encontro Tesseract.dll. Instale/restaure el paquete NuGet Tesseract 5.2.0.");
	}

 private static void EnsureTesseractNativePath(string tesseractAssemblyPath)
	{
		string text = Environment.Is64BitProcess ? "x64" : "x86";
     string text2 = null;

		try
		{
			string directoryName = System.IO.Path.GetDirectoryName(tesseractAssemblyPath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				string text3 = System.IO.Path.GetFullPath(System.IO.Path.Combine(directoryName, "..\\.."));
				text2 = System.IO.Path.Combine(text3, text);
			}
		}
		catch
		{
		}

		if (string.IsNullOrEmpty(text2) || !Directory.Exists(text2))
		{
			text2 = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\packages\\Tesseract.5.2.0\\" + text));
		}

		if (!Directory.Exists(text2))
		{
			return;
		}

		EnsureLeptonicaCompatibility(text2);

		string environmentVariable = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
		if (environmentVariable.IndexOf(text2, StringComparison.OrdinalIgnoreCase) < 0)
		{
			Environment.SetEnvironmentVariable("PATH", text2 + ";" + environmentVariable);
		}
	}

	private static void EnsureLeptonicaCompatibility(string nativeDirectory)
	{
		try
		{
			string text = System.IO.Path.Combine(nativeDirectory, "leptonica-1.82.1.dll");
			string text2 = System.IO.Path.Combine(nativeDirectory, "leptonica-1.82.0.dll");
			if (!File.Exists(text) && File.Exists(text2))
			{
				File.Copy(text2, text);
			}

			string text3 = Environment.Is64BitProcess ? "x64" : "x86";
			string text4 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text3);
			if (Directory.Exists(text4))
			{
				string text5 = System.IO.Path.Combine(text4, "leptonica-1.82.0.dll");
				string text6 = System.IO.Path.Combine(text4, "leptonica-1.82.1.dll");
				string text7 = System.IO.Path.Combine(text4, "tesseract50.dll");
				string text8 = System.IO.Path.Combine(nativeDirectory, "tesseract50.dll");

				if (!File.Exists(text5) && File.Exists(text2))
				{
					File.Copy(text2, text5);
				}
				if (!File.Exists(text6) && (File.Exists(text) || File.Exists(text2)))
				{
					File.Copy(File.Exists(text) ? text : text2, text6);
				}
				if (!File.Exists(text7) && File.Exists(text8))
				{
					File.Copy(text8, text7);
				}
			}
		}
		catch
		{
		}
	}

	private static object InvokeMethodByName(object instance, string methodName, object[] args)
	{
		MethodInfo[] methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
		for (int i = 0; i < methods.Length; i++)
		{
          if (methods[i].Name == methodName && AreParametersCompatible(methods[i].GetParameters(), args))
			{
               try
				{
					return methods[i].Invoke(instance, args);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					}
					throw;
				}
			}
		}
		throw new MissingMethodException(instance.GetType().FullName, methodName);
	}

	private static object InvokeStaticMethodByName(Type type, string methodName, object[] args)
	{
		MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
		for (int i = 0; i < methods.Length; i++)
		{
          if (methods[i].Name == methodName && AreParametersCompatible(methods[i].GetParameters(), args))
			{
               try
				{
					return methods[i].Invoke(null, args);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					}
					throw;
				}
			}
		}
		throw new MissingMethodException(type.FullName, methodName);
	}

	private static bool AreParametersCompatible(ParameterInfo[] parameters, object[] args)
	{
		if (parameters.Length != args.Length)
		{
			return false;
		}

		for (int i = 0; i < parameters.Length; i++)
		{
			object arg = args[i];
			Type parameterType = parameters[i].ParameterType;

			if (arg == null)
			{
				if (parameterType.IsValueType && Nullable.GetUnderlyingType(parameterType) == null)
				{
					return false;
				}
				continue;
			}

			if (!parameterType.IsInstanceOfType(arg))
			{
				return false;
			}
		}

		return true;
	}

	private static void CloseOrDispose(object instance)
	{
		if (instance == null)
		{
			return;
		}
		try
		{
			MethodInfo method = instance.GetType().GetMethod("Dispose", Type.EmptyTypes);
			if (method != null)
			{
				method.Invoke(instance, null);
			}
		}
		catch
		{
		}
	}

	private void panel2_MouseDown(object sender, MouseEventArgs e)
	{
		if (img && !pdf && completado)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Archivos PNG (*.png)|*.png|Archivos JPG (*.jpg)|*.jpg|Archivos GIF (*.gif)|*.gif|Archivos BMP (*.bmp)|*.bmp|Archivos JPEG (*.jpeg)|*.jpeg";
			openFileDialog.Title = "Selecciona una iamgen a convertir...";
			DialogResult dialogResult = openFileDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				label2.Text = "Archivo Cargado...";
				label2.Hide();
				string ruta = openFileDialog.FileName;
				bar3.Value = 50;
				string[] array = new string[1] { ruta };
				Task.Factory.StartNew(delegate
				{
					btconver.Enabled = false;
					lbbar.Text = "Analizando...";
					panel2.Enabled = false;
					bar3.Value = 50;
					Image image = Image.FromFile(ruta);
					pictureBox = new PictureBox();
					pictureBox.Image = image;
					pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
					pictureBox.Dock = DockStyle.Fill;
					panel1.Invoke((Action)delegate
					{
						panel2.Controls.Add(pictureBox);
					});
				});
				Task.Factory.StartNew(delegate
				{
					if (ruta.Length > 0)
					{
						bar3.Value = 70;
						ext = ruta.Substring(ruta.LastIndexOf(".") + 1);
						lbruta.Text = ruta;
						convertir(img, pdf, ruta, ext);
					}
				});
			}
		}
		if (!img && pdf)
		{
			OpenFileDialog openFileDialog2 = new OpenFileDialog();
			openFileDialog2.Filter = "Archivos PDF (*.pdf)|*.pdf";
			openFileDialog2.Title = "Selecciona un PDF a convertir...";
			DialogResult dialogResult2 = openFileDialog2.ShowDialog();
			string fileName = openFileDialog2.FileName;
			label2.Hide();
			if (dialogResult2 == DialogResult.OK)
			{
				bar3.Value = 50;
				ruta2 = fileName;
				ingresado = true;
				bar3.Value = 70;
				btconver.Enabled = true;
				ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
				lbruta.Text = fileName;
				convertir(img, pdf, fileName, ext);
				btcan.Enabled = true;
				btconver.Enabled = false;
			}
		}
	}

	private void OCR_Load(object sender, EventArgs e)
	{
		panel2.AllowDrop = true;
		if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib) && completado)
		{
			btpegar.Enabled = true;
		}
		else
		{
			btpegar.Enabled = false;
		}
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	private void rbima_CheckedChanged(object sender, EventArgs e)
	{
		if (rbima.Checked)
		{
			img = true;
			btlect.Enabled = false;
			pdf = false;
			chpag.Enabled = false;
		}
	}

	private void rbpdf_CheckedChanged(object sender, EventArgs e)
	{
		if (rbpdf.Checked)
		{
			img = false;
			pdf = true;
			btpegar.Enabled = false;
			chpag.Enabled = true;
		}
		else
		{
			btlect.Enabled = false;
		}
	}

	private void btcan_Click(object sender, EventArgs e)
	{
		lbfia.Text = "n/a";
		panel2.Enabled = true;
		bar3.Value = 0;
		chpag.Checked = false;
		btlect.Enabled = false;
		lbbar.Text = "n/a";
		ruta2 = "";
		ingresado = false;
		paginas = false;
		btcan.Enabled = false;
		rbima.Enabled = true;
		chpag.Enabled = true;
		label2.Text = "Arrastre o busque el archivo";
		btcopiar.Enabled = false;
		btguarda.Enabled = false;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(txtresult.Text);
		MessageBox.Show("Texto copiado");
	}

	private void OCR_Activated(object sender, EventArgs e)
	{
		if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib) && img && !pdf)
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
		string text = "";
		lbruta.Text = "Portapapeles";
		if (!Clipboard.ContainsImage() || !completado)
		{
			return;
		}
		Image image = Clipboard.GetImage();
		int num = image.Width;
		int num2 = image.Height;
		if ((num >= pictureBox.Width && num2 >= pictureBox.Height) || (num >= pictureBox.Width && num2 < pictureBox.Height))
		{
			text = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".jpg");
			image.Save(text, ImageFormat.Jpeg);
			ext = text.Substring(text.LastIndexOf(".") + 1);
			img = true;
			pictureBox = new PictureBox();
			int num3 = 320;
			float num4 = (float)image.Height / (float)image.Width;
			int thumbHeight = (int)((float)num3 * num4);
			Image thumbnailImage = image.GetThumbnailImage(num3, thumbHeight, null, IntPtr.Zero);
			pictureBox.Image = thumbnailImage;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Dock = DockStyle.Fill;
			pictureBox.Size = new Size(num3, thumbHeight);
			int num5 = (panel2.Width - pictureBox.Width) / 2;
			int num6 = (panel2.Height - pictureBox.Height) / 2;
			pictureBox.Location = new Point(num5, num6);
			panel1.Invoke((Action)delegate
			{
				panel2.Controls.Add(pictureBox);
			});
			int num7 = (int)((double)image.Width / (double)image.Height * 490.0);
			Image image2 = ResizeImage(image, num7, 490);
			if (ext.Equals("jpg"))
			{
				thumbnailImage.Save("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img.jpg", ImageFormat.Jpeg);
			}
			if (ext.Equals("png"))
			{
				thumbnailImage.Save("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img.png", ImageFormat.Png);
			}
			ext = text.Substring(text.LastIndexOf(".") + 1);
			convertir(img, pdf, "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img." + ext, ext);
		}
		else
		{
			text = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".jpg");
			image.Save(text, ImageFormat.Jpeg);
			ext = text.Substring(text.LastIndexOf(".") + 1);
			img = true;
			convertir(img, pdf, text, ext);
		}
	}

	private void OCR_KeyDown(object sender, KeyEventArgs e)
	{
		string text = "";
		lbruta.Text = "Portapapeles";
		if (e.KeyData != (Keys.V | Keys.Control) || pdf || !completado || !Clipboard.ContainsImage())
		{
			return;
		}
		Image image = Clipboard.GetImage();
		int num = image.Width;
		int num2 = image.Height;
		if ((num >= pictureBox.Width && num2 >= pictureBox.Height) || (num >= pictureBox.Width && num2 < pictureBox.Height))
		{
			text = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".jpg");
			image.Save(text, ImageFormat.Jpeg);
			img = true;
			pictureBox = new PictureBox();
			int num3 = 320;
			float num4 = (float)image.Height / (float)image.Width;
			int thumbHeight = (int)((float)num3 * num4);
			Image thumbnailImage = image.GetThumbnailImage(num3, thumbHeight, null, IntPtr.Zero);
			pictureBox.Image = thumbnailImage;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Dock = DockStyle.Fill;
			pictureBox.Size = new Size(num3, thumbHeight);
			int num5 = (panel2.Width - pictureBox.Width) / 2;
			int num6 = (panel2.Height - pictureBox.Height) / 2;
			pictureBox.Location = new Point(num5, num6);
			panel1.Invoke((Action)delegate
			{
				panel2.Controls.Add(pictureBox);
			});
			int num7 = (int)((double)image.Width / (double)image.Height * 490.0);
			Image image2 = ResizeImage(image, num7, 490);
			if (ext.Equals("jpg"))
			{
				thumbnailImage.Save("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img.jpg", ImageFormat.Jpeg);
			}
			if (ext.Equals("png"))
			{
				thumbnailImage.Save("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img.png", ImageFormat.Png);
			}
			ext = text.Substring(text.LastIndexOf(".") + 1);
			convertir(img, pdf, "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\img." + ext, ext);
			e.Handled = true;
		}
		else
		{
			text = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "." + ext);
			if (ext.Equals("jpg"))
			{
				image.Save(text, ImageFormat.Jpeg);
			}
			if (ext.Equals("png"))
			{
				image.Save(text, ImageFormat.Png);
			}
			ext = text.Substring(text.LastIndexOf(".") + 1);
			img = true;
			convertir(img, pdf, text, ext);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void btguarda_Click(object sender, EventArgs e)
	{
		string contents = txtresult.Text;
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
		saveFileDialog.Title = "Guardar archivo de texto";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = saveFileDialog.FileName;
			File.WriteAllText(fileName, contents);
		}
	}

	private void OCR_FormClosing(object sender, FormClosingEventArgs e)
	{
		Environment.Exit(0);
	}

	private void OCR_Deactivate(object sender, EventArgs e)
	{
		if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib) && img && !pdf)
		{
			btpegar.Enabled = true;
		}
		else
		{
			btpegar.Enabled = false;
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Dispose();
		Principal principal = new Principal();
		principal.Show();
	}

	private void chpag_CheckedChanged(object sender, EventArgs e)
	{
		if (chpag.Checked)
		{
			paginas = true;
			return;
		}
		paginas = false;
		btlect.Enabled = false;
	}

	private void btlect_Click(object sender, EventArgs e)
	{
		Lector lector = new Lector(ruta2, txtresult, label2, lbbar, bar3, btconver, btcan, btguarda, btcopiar, btlect, lbfia);
		lector.ShowDialog();
	}

	public void mostrar()
	{
	}

	private void btconver_Click(object sender, EventArgs e)
	{
		completado = true;
		btconver.Enabled = false;
		panel2.Enabled = true;
		txtresult.Text = "";
		bar3.Value = 0;
		lbfia.Text = "n/a";
		btlect.Enabled = false;
		panel2.Controls.Remove(pictureBox);
		label2.Text = "Arrastre o busque el archivo";
		btcan.Enabled = false;
		rbpdf.Enabled = true;
		lbbar.Text = "n/a";
		lbruta.Text = "n/a";
		btlect.Enabled = true;
		label2.Show();
		btcopiar.Enabled = false;
		btguarda.Enabled = false;
		btlect.Enabled = false;
		rbima.Enabled = true;
		if (pdf)
		{
			chpag.Enabled = true;
		}
		else
		{
			chpag.Checked = false;
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void btruta_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.Description = "Seleccione carpeta de destino";
		folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
		DialogResult dialogResult = folderBrowserDialog.ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertidorImagenes.OCR));
		this.panel1 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btpegar = new System.Windows.Forms.Button();
		this.btcan = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.chpag = new System.Windows.Forms.CheckBox();
		this.btlect = new System.Windows.Forms.Button();
		this.rbpdf = new System.Windows.Forms.RadioButton();
		this.rbima = new System.Windows.Forms.RadioButton();
		this.label7 = new System.Windows.Forms.Label();
		this.lbbar = new System.Windows.Forms.Label();
		this.btconver = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.bar3 = new System.Windows.Forms.ProgressBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btguarda = new System.Windows.Forms.Button();
		this.btcopiar = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.txtresult = new System.Windows.Forms.RichTextBox();
		this.lbruta = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lbfia = new System.Windows.Forms.Label();
        this.label10 = new System.Windows.Forms.Label();
		this.cboidioma = new System.Windows.Forms.ComboBox();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.button3);
		this.panel1.Controls.Add(this.button4);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.lbruta);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Location = new System.Drawing.Point(2, 58);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(912, 411);
		this.panel1.TabIndex = 0;
        this.button3.Image = null;
		this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button3.Location = new System.Drawing.Point(680, 358);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(100, 43);
		this.button3.TabIndex = 28;
		this.button3.Text = "Minimizar";
		this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button4.Image = (System.Drawing.Image)resources.GetObject("button4.Image");
		this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button4.Location = new System.Drawing.Point(786, 358);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(100, 43);
		this.button4.TabIndex = 27;
		this.button4.Text = "Salir";
		this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.btpegar);
		this.panel4.Controls.Add(this.btcan);
		this.panel4.Controls.Add(this.panel5);
      this.panel4.Controls.Add(this.cboidioma);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.lbbar);
		this.panel4.Controls.Add(this.btconver);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.bar3);
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Location = new System.Drawing.Point(3, 26);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(508, 310);
		this.panel4.TabIndex = 25;
		this.btpegar.Image = (System.Drawing.Image)resources.GetObject("btpegar.Image");
		this.btpegar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btpegar.Location = new System.Drawing.Point(8, 264);
		this.btpegar.Name = "btpegar";
		this.btpegar.Size = new System.Drawing.Size(104, 29);
		this.btpegar.TabIndex = 27;
		this.btpegar.Text = "Pegar Imagen";
		this.btpegar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btpegar.UseVisualStyleBackColor = true;
		this.btpegar.Click += new System.EventHandler(btpegar_Click);
		this.btcan.Image = (System.Drawing.Image)resources.GetObject("btcan.Image");
		this.btcan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btcan.Location = new System.Drawing.Point(346, 264);
		this.btcan.Name = "btcan";
		this.btcan.Size = new System.Drawing.Size(116, 29);
		this.btcan.TabIndex = 32;
		this.btcan.Text = "Cancelar Proceso";
		this.btcan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btcan.UseVisualStyleBackColor = true;
		this.btcan.Click += new System.EventHandler(btcan_Click);
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.chpag);
		this.panel5.Controls.Add(this.btlect);
		this.panel5.Controls.Add(this.rbpdf);
		this.panel5.Controls.Add(this.rbima);
		this.panel5.Location = new System.Drawing.Point(346, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(142, 112);
		this.panel5.TabIndex = 31;
     this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(343, 39);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 13);
		this.label10.TabIndex = 33;
		this.label10.Text = "Idioma OCR:";
		this.cboidioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboidioma.FormattingEnabled = true;
		this.cboidioma.Location = new System.Drawing.Point(415, 35);
		this.cboidioma.Name = "cboidioma";
		this.cboidioma.Size = new System.Drawing.Size(73, 21);
		this.cboidioma.TabIndex = 34;
		this.chpag.AutoSize = true;
		this.chpag.Location = new System.Drawing.Point(14, 55);
		this.chpag.Name = "chpag";
		this.chpag.Size = new System.Drawing.Size(64, 17);
		this.chpag.TabIndex = 4;
		this.chpag.Text = "Paginas";
		this.chpag.UseVisualStyleBackColor = true;
		this.chpag.CheckedChanged += new System.EventHandler(chpag_CheckedChanged);
		this.btlect.Location = new System.Drawing.Point(3, 75);
		this.btlect.Name = "btlect";
		this.btlect.Size = new System.Drawing.Size(125, 23);
		this.btlect.TabIndex = 3;
		this.btlect.Text = "Seleccionar Paginas";
		this.btlect.UseVisualStyleBackColor = true;
		this.btlect.Click += new System.EventHandler(btlect_Click);
		this.rbpdf.AutoSize = true;
		this.rbpdf.Location = new System.Drawing.Point(14, 31);
		this.rbpdf.Name = "rbpdf";
		this.rbpdf.Size = new System.Drawing.Size(46, 17);
		this.rbpdf.TabIndex = 2;
		this.rbpdf.Text = "PDF";
		this.rbpdf.UseVisualStyleBackColor = true;
		this.rbpdf.CheckedChanged += new System.EventHandler(rbpdf_CheckedChanged);
		this.rbima.AutoSize = true;
		this.rbima.Checked = true;
		this.rbima.Location = new System.Drawing.Point(14, 8);
		this.rbima.Name = "rbima";
		this.rbima.Size = new System.Drawing.Size(60, 17);
		this.rbima.TabIndex = 1;
		this.rbima.TabStop = true;
		this.rbima.Text = "Imagen";
		this.rbima.UseVisualStyleBackColor = true;
		this.rbima.CheckedChanged += new System.EventHandler(rbima_CheckedChanged);
		this.label7.AutoSize = true;
		this.label7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label7.Location = new System.Drawing.Point(343, 239);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(43, 13);
		this.label7.TabIndex = 29;
		this.label7.Text = "Estado:";
		this.lbbar.AutoSize = true;
		this.lbbar.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbbar.Location = new System.Drawing.Point(392, 239);
		this.lbbar.Name = "lbbar";
		this.lbbar.Size = new System.Drawing.Size(24, 13);
		this.lbbar.TabIndex = 24;
		this.lbbar.Text = "n/a";
		this.btconver.Image = (System.Drawing.Image)resources.GetObject("btconver.Image");
		this.btconver.Location = new System.Drawing.Point(346, 176);
		this.btconver.Name = "btconver";
		this.btconver.Size = new System.Drawing.Size(116, 29);
		this.btconver.TabIndex = 28;
		this.btconver.Text = "Otra Conversion";
		this.btconver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btconver.UseVisualStyleBackColor = true;
		this.btconver.Click += new System.EventHandler(btconver_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label6.Location = new System.Drawing.Point(3, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(199, 27);
		this.label6.TabIndex = 26;
		this.label6.Text = "Conversion a Texto";
		this.bar3.Location = new System.Drawing.Point(346, 211);
		this.bar3.Name = "bar3";
		this.bar3.Size = new System.Drawing.Size(142, 23);
		this.bar3.TabIndex = 3;
		this.panel2.AllowDrop = true;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Location = new System.Drawing.Point(8, 49);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(323, 203);
		this.panel2.TabIndex = 0;
		this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(panel2_DragDrop);
		this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(panel2_DragEnter);
		this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(panel2_MouseDown);
		this.label2.AutoSize = true;
		this.label2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label2.Location = new System.Drawing.Point(69, 94);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(139, 13);
		this.label2.TabIndex = 23;
		this.label2.Text = "Arrastre o busque el archivo";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.lbfia);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.btguarda);
		this.panel3.Controls.Add(this.btcopiar);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtresult);
		this.panel3.Location = new System.Drawing.Point(539, 26);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(368, 310);
		this.panel3.TabIndex = 24;
        this.btguarda.Image = null;
		this.btguarda.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btguarda.Location = new System.Drawing.Point(136, 268);
		this.btguarda.Name = "btguarda";
		this.btguarda.Size = new System.Drawing.Size(104, 29);
		this.btguarda.TabIndex = 27;
		this.btguarda.Text = "Guardar Texto";
		this.btguarda.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btguarda.UseVisualStyleBackColor = true;
		this.btguarda.Click += new System.EventHandler(btguarda_Click);
		this.btcopiar.Image = (System.Drawing.Image)resources.GetObject("btcopiar.Image");
		this.btcopiar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btcopiar.Location = new System.Drawing.Point(8, 268);
		this.btcopiar.Name = "btcopiar";
		this.btcopiar.Size = new System.Drawing.Size(104, 29);
		this.btcopiar.TabIndex = 26;
		this.btcopiar.Text = "Copiar texto";
		this.btcopiar.UseVisualStyleBackColor = true;
		this.btcopiar.Click += new System.EventHandler(button1_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label5.Location = new System.Drawing.Point(3, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(180, 27);
		this.label5.TabIndex = 25;
		this.label5.Text = "Texto Convertido";
		this.txtresult.BackColor = System.Drawing.SystemColors.Window;
		this.txtresult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtresult.Location = new System.Drawing.Point(8, 62);
		this.txtresult.Name = "txtresult";
		this.txtresult.Size = new System.Drawing.Size(352, 194);
		this.txtresult.TabIndex = 23;
		this.txtresult.Text = "";
		this.lbruta.AutoSize = true;
		this.lbruta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbruta.Location = new System.Drawing.Point(83, 348);
		this.lbruta.Name = "lbruta";
		this.lbruta.Size = new System.Drawing.Size(24, 13);
		this.lbruta.TabIndex = 19;
		this.lbruta.Text = "n/a";
		this.label3.AutoSize = true;
		this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label3.Location = new System.Drawing.Point(10, 348);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 13);
		this.label3.TabIndex = 16;
		this.label3.Text = "Ruta Origen:";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 21.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(73, 33);
		this.label1.TabIndex = 1;
		this.label1.Text = "OCR";
		this.button2.Image = (System.Drawing.Image)resources.GetObject("button2.Image");
		this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button2.Location = new System.Drawing.Point(789, 9);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(100, 43);
		this.button2.TabIndex = 27;
		this.button2.Text = "Principal";
		this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.label4.AutoSize = true;
		this.label4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label4.Location = new System.Drawing.Point(10, 373);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(36, 13);
		this.label4.TabIndex = 29;
		this.label4.Text = "Aviso:";
		this.label8.AutoSize = true;
		this.label8.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label8.Location = new System.Drawing.Point(52, 373);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(397, 13);
		this.label8.TabIndex = 30;
		this.label8.Text = "Los texto posible de leer en imagenes son textos cortos y lo suficientemente claros.";
		this.label9.AutoSize = true;
		this.label9.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label9.Location = new System.Drawing.Point(259, 268);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(93, 13);
		this.label9.TabIndex = 31;
		this.label9.Text = "Nivel de fiabilidad:";
		this.lbfia.AutoSize = true;
		this.lbfia.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.lbfia.Location = new System.Drawing.Point(259, 284);
		this.lbfia.Name = "lbfia";
		this.lbfia.Size = new System.Drawing.Size(24, 13);
		this.lbfia.TabIndex = 31;
		this.lbfia.Text = "n/a";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(916, 472);
		base.ControlBox = false;
		base.Controls.Add(this.button2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "OCR";
		this.Text = "OCR";
		base.Activated += new System.EventHandler(OCR_Activated);
		base.Deactivate += new System.EventHandler(OCR_Deactivate);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OCR_FormClosing);
		base.Load += new System.EventHandler(OCR_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(OCR_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
}
