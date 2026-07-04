// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.Offices
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ConvertidorImagenes;
using ConvertidorImagenes.Properties;
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using SautinSoft;
using Button = System.Windows.Forms.Button;
using Excel = Microsoft.Office.Interop.Excel;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;


namespace ConvertidorImagenes
{
public partial class Offices : Form
{
	private string nombreft = "";

	private string nombreft2 = "";

	private int contador = 0;

	private bool apdf = true;

	private string rtdestino = "C:\\Users\\" + Environment.UserName + "\\Desktop";

	private string nombarch = "";

	private IContainer components = null;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private ProgressBar bar2;

	private Label lbdt;

	private Label lbro;

	private Label label3;

	private Label label2;

	private Button button2;

	private Button button1;

	private ComboBox cbof;

	private Label label6;

	private Panel panel3;

	private RadioButton pdfaof;

	private RadioButton ofapdf;

	private Panel panel5;

	private Label label8;

	private Button button3;

	private ComboBox cbpdf;

	private Panel panel4;

	private Label label7;

	private Button button5;

	private Label lbestado;

	private Label label4;

	private Button btay;

	private Button button6;

	private Button button4;

	private Button button7;

	private const string SyncfusionTrialLicense = "Ngo9BigBOggjHTQxAR8/V1JHaF1cXmhOYVJ3WmFZfVhgd19EaVZSTWY/P1ZhSXxVdkJjXX5bcn1WT2BeUUJ9XEE=";

	private string rutaArchivoSeleccionado = "";

	public Offices()
	{
		InitializeComponent();
		TryRegisterSyncfusionLicense();
		string[] array = new string[3] { "WORD", "EXCEL", "POWERPOINT" };
		string[] array2 = new string[2] { "WORD", "POWERPOINT(AUN NO DESARROLLADO)" };
		string[] array3 = array;
		foreach (string item in array3)
		{
			cbof.Items.Add(item);
		}
		string[] array4 = array2;
		foreach (string item2 in array4)
		{
			cbpdf.Items.Add(item2);
		}
		cbof.SelectedItem = "WORD";
		cbpdf.SelectedItem = "WORD";
		lbdt.Text = rtdestino;
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
			panel2.Enabled = true;
		}
	}

	private void pdfaof_CheckedChanged(object sender, EventArgs e)
	{
		if (pdfaof.Checked)
		{
			panel5.Enabled = true;
			panel4.Enabled = false;
			apdf = false;
		}
	}

	private void Offices_Load(object sender, EventArgs e)
	{
		panel2.AllowDrop = true;
		btay.FlatStyle = FlatStyle.Flat;
		btay.FlatAppearance.BorderSize = 0;
		Button button = btay;
		int num = (btay.Width = 25);
		button.Height = num;
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddEllipse(0, 0, btay.Width, btay.Height);
		btay.Region = new Region(graphicsPath);
		btay.Location = new Point(720, 3);
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	private void panel2_MouseDown(object sender, MouseEventArgs e)
	{
	}

	private void panel2_DragDrop(object sender, DragEventArgs e)
	{
		bar2.Value = 20;
		object data = e.Data.GetData(DataFormats.FileDrop);
		if (data != null)
		{
			string[] array = data as string[];
          if (array.Length != 0)
			{
				lbro.Text = array[0];
                rutaArchivoSeleccionado = array[0];
				nombarch = Path.GetFileNameWithoutExtension(array[0]);
				contador = 40;
				bar2.Value = contador;
               lbestado.Text = "Archivo cargado. Presione Iniciar conversion";
			}
		}
	}

	private void button7_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(rutaArchivoSeleccionado) || !File.Exists(rutaArchivoSeleccionado))
		{
			MessageBox.Show("Primero arrastre un archivo valido al panel.");
			return;
		}

		if (apdf)
		{
			convertirpdf(nombreft, rutaArchivoSeleccionado);
		}
		else
		{
			convertirAdoc(rutaArchivoSeleccionado, nombreft2);
		}
	}

	public void convertirpdf(string formato, string ruta)
	{
		panel2.Enabled = false;
		cbof.Enabled = false;
		string outputFileName = Path.Combine(rtdestino, nombarch + ".pdf");
		try
		{
			switch (formato)
			{
			case "WORD":
				lbestado.Text = "Convirtiendo Word con Syncfusion";
				bar2.Value = 60;
				ConvertWordToPdfWithSyncfusion(ruta, outputFileName);
				break;
			case "EXCEL":
				lbestado.Text = "Convirtiendo Excel con Syncfusion";
				bar2.Value = 60;
				ConvertExcelToPdfWithSyncfusion(ruta, outputFileName);
				break;
			case "POWERPOINT":
				lbestado.Text = "Convirtiendo PowerPoint con Syncfusion";
				bar2.Value = 60;
				ConvertPowerPointToPdfWithSyncfusion(ruta, outputFileName);
				break;
			default:
				throw new InvalidOperationException("Formato no soportado: " + formato);
			}
			bar2.Value = 100;
			lbestado.Text = "Finalizo Conversion";
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error al convertir con Syncfusion: " + ex.Message);
			lbestado.Text = "Error";
			panel2.Enabled = true;
			cbof.Enabled = true;
		}
	}

	private void ConvertWordToPdfWithSyncfusion(string inputFilePath, string outputFilePath)
	{
		object wordDocument = null;
		object converter = null;
		object pdfDocument = null;
		try
		{
			Type wordDocumentType = GetRequiredType("Syncfusion.DocIO.DLS.WordDocument, Syncfusion.DocIO.Base", "Syncfusion.DocIO.DLS.WordDocument, Syncfusion.DocIO.NET");
			Type formatType = GetRequiredType("Syncfusion.DocIO.FormatType, Syncfusion.DocIO.Base", "Syncfusion.DocIO.FormatType, Syncfusion.DocIO.NET");
			Type converterType = GetRequiredType("Syncfusion.DocToPDFConverter.DocToPDFConverter, Syncfusion.DocToPDFConverter.Base", "Syncfusion.DocToPDFConverter.DocToPDFConverter, Syncfusion.DocToPDFConverter.NET");

			object automaticFormat = Enum.Parse(formatType, "Automatic");
			wordDocument = Activator.CreateInstance(wordDocumentType, new object[] { inputFilePath, automaticFormat });
			converter = Activator.CreateInstance(converterType);
			pdfDocument = InvokeMethodByName(converter, "ConvertToPDF", new object[] { wordDocument });
			SavePdfDocument(pdfDocument, outputFilePath);
		}
		finally
		{
			CloseOrDispose(pdfDocument);
			CloseOrDispose(converter);
			CloseOrDispose(wordDocument);
		}
	}

	private void ConvertExcelToPdfWithSyncfusion(string inputFilePath, string outputFilePath)
	{
		object excelEngine = null;
		object workbook = null;
     object converter = null;
		object pdfDocument = null;
     Exception syncfusionError = null;
		try
		{
            EnsureAssembliesLoaded(
				"Syncfusion.ExcelToPdfConverter.Base",
				"Syncfusion.Pdf.Base",
				"Syncfusion.XlsIO.Base",
				"Syncfusion.OfficeChart.Base",
				"Syncfusion.Compression.Base");

			Type excelEngineType = GetRequiredType("Syncfusion.XlsIO.ExcelEngine, Syncfusion.XlsIO.Base", "Syncfusion.XlsIO.ExcelEngine, Syncfusion.XlsIO.NET");
			excelEngine = Activator.CreateInstance(excelEngineType);
			object application = excelEngineType.GetProperty("Excel").GetValue(excelEngine, null);

			Type excelVersionType = GetRequiredType("Syncfusion.XlsIO.ExcelVersion, Syncfusion.XlsIO.Base", "Syncfusion.XlsIO.ExcelVersion, Syncfusion.XlsIO.NET");
			object xlsxVersion = Enum.Parse(excelVersionType, "Xlsx");
			application.GetType().GetProperty("DefaultVersion").SetValue(application, xlsxVersion, null);

			object workbooks = application.GetType().GetProperty("Workbooks").GetValue(application, null);
			workbook = InvokeMethodByName(workbooks, "Open", new object[] { inputFilePath });

            Type converterType = GetRequiredType(
				"Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter, Syncfusion.ExcelToPdfConverter.Base",
				"Syncfusion.XlsIORenderer.XlsIORenderer, Syncfusion.XlsIORenderer.Base",
				"Syncfusion.XlsIORenderer.XlsIORenderer, Syncfusion.XlsIORenderer",
				"Syncfusion.XlsIORenderer.XlsIORenderer, Syncfusion.XlsIORenderer.NET");
			converter = Activator.CreateInstance(converterType);

			try
			{
				pdfDocument = InvokeMethodByName(converter, "Convert", new object[] { workbook });
			}
			catch (MissingMethodException)
			{
				pdfDocument = InvokeMethodByName(converter, "ConvertToPDF", new object[] { workbook });
			}

			SavePdfDocument(pdfDocument, outputFilePath);
		}
     catch (Exception ex)
		{
			syncfusionError = ex;
		}
		finally
		{
			CloseOrDispose(pdfDocument);
           CloseOrDispose(converter);
			CloseOrDispose(workbook);
			CloseOrDispose(excelEngine);
		}

		if (syncfusionError != null)
		{
			try
			{
				ConvertExcelToPdfWithInterop(inputFilePath, outputFilePath);
			}
			catch (Exception interopEx)
			{
				throw new InvalidOperationException("No se pudo convertir Excel con Syncfusion ni con Interop. Syncfusion: " + syncfusionError.Message + " | Interop: " + interopEx.Message);
			}
		}
	}

	private void ConvertExcelToPdfWithInterop(string inputFilePath, string outputFilePath)
	{
		Excel.Application excelApp = null;
		Excel.Workbook excelWorkbook = null;

		try
		{
			excelApp = new Excel.Application();
			excelApp.DisplayAlerts = false;
			excelWorkbook = excelApp.Workbooks.Open(inputFilePath);
			excelWorkbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, outputFilePath);
		}
		finally
		{
			if (excelWorkbook != null)
			{
				excelWorkbook.Close(false);
				Marshal.FinalReleaseComObject(excelWorkbook);
			}

			if (excelApp != null)
			{
				excelApp.Quit();
				Marshal.FinalReleaseComObject(excelApp);
			}
		}
	}

	private void ConvertPowerPointToPdfWithSyncfusion(string inputFilePath, string outputFilePath)
	{
		object presentation = null;
		object pdfDocument = null;
     Exception syncfusionError = null;
		try
		{
			Type presentationType = GetRequiredType("Syncfusion.Presentation.Presentation, Syncfusion.Presentation.Base", "Syncfusion.Presentation.Presentation, Syncfusion.Presentation.NET");
			presentation = InvokeStaticMethodByName(presentationType, "Open", new object[] { inputFilePath });

			Type converterType = GetRequiredType("Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter, Syncfusion.PresentationToPdfConverter.Base", "Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter, Syncfusion.PresentationToPdfConverter.NET");
			pdfDocument = InvokeStaticMethodByName(converterType, "Convert", new object[] { presentation });
			SavePdfDocument(pdfDocument, outputFilePath);
		}
     catch (Exception ex)
		{
			syncfusionError = ex;
		}
		finally
		{
			CloseOrDispose(pdfDocument);
			CloseOrDispose(presentation);
		}

		if (syncfusionError != null)
		{
			try
			{
				ConvertPowerPointToPdfWithInterop(inputFilePath, outputFilePath);
			}
			catch (Exception interopEx)
			{
				throw new InvalidOperationException("No se pudo convertir PowerPoint con Syncfusion ni con Interop. Syncfusion: " + syncfusionError.Message + " | Interop: " + interopEx.Message);
			}
		}
	}

	private void ConvertPowerPointToPdfWithInterop(string inputFilePath, string outputFilePath)
	{
		PowerPoint.Application powerPointApp = null;
		PowerPoint.Presentation powerPointPresentation = null;

		try
		{
			powerPointApp = new PowerPoint.Application();
			powerPointPresentation = powerPointApp.Presentations.Open(
				inputFilePath,
				Office.MsoTriState.msoFalse,
				Office.MsoTriState.msoFalse,
				Office.MsoTriState.msoFalse);

			powerPointPresentation.SaveAs(outputFilePath, PowerPoint.PpSaveAsFileType.ppSaveAsPDF, Office.MsoTriState.msoFalse);
		}
		finally
		{
			if (powerPointPresentation != null)
			{
				powerPointPresentation.Close();
				Marshal.FinalReleaseComObject(powerPointPresentation);
			}

			if (powerPointApp != null)
			{
				powerPointApp.Quit();
				Marshal.FinalReleaseComObject(powerPointApp);
			}
		}
	}

	private static void TryRegisterSyncfusionLicense()
	{
		try
		{
			Type licenseProviderType = GetRequiredType("Syncfusion.Licensing.SyncfusionLicenseProvider, Syncfusion.Licensing");
			MethodInfo registerMethod = licenseProviderType.GetMethod("RegisterLicense", BindingFlags.Public | BindingFlags.Static);
			if (registerMethod != null)
			{
				registerMethod.Invoke(null, new object[] { SyncfusionTrialLicense });
			}
		}
		catch
		{
			// Se ignora aquí para no bloquear el formulario. El error aparece al intentar convertir.
		}
	}

	private static Type GetRequiredType(params string[] candidates)
	{
		for (int i = 0; i < candidates.Length; i++)
		{
         Type resolvedType = ResolveType(candidates[i]);
			if (resolvedType != null)
			{
				return resolvedType;
			}
		}
		throw new InvalidOperationException("No se encontraron los ensamblados de Syncfusion requeridos. Instala los paquetes NuGet de Syncfusion para Office a PDF.");
	}

	private static Type ResolveType(string candidate)
	{
		Type resolvedType = Type.GetType(candidate, false);
		if (resolvedType != null)
		{
			return resolvedType;
		}

		string[] parts = candidate.Split(new char[] { ',' }, 2);
		if (parts.Length != 2)
		{
			return null;
		}

		string typeName = parts[0].Trim();
		string assemblyName = parts[1].Trim();

		Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < loadedAssemblies.Length; i++)
		{
			AssemblyName currentAssemblyName = loadedAssemblies[i].GetName();
			if (string.Equals(currentAssemblyName.Name, assemblyName, StringComparison.OrdinalIgnoreCase))
			{
				resolvedType = loadedAssemblies[i].GetType(typeName, false);
				if (resolvedType != null)
				{
					return resolvedType;
				}
			}
		}

		try
		{
          Assembly assembly = TryLoadAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly.GetType(typeName, false);
			}
			return null;
		}
		catch
		{
			return null;
		}
	}

	private static void EnsureAssembliesLoaded(params string[] assemblyNames)
	{
		for (int i = 0; i < assemblyNames.Length; i++)
		{
			TryLoadAssembly(assemblyNames[i]);
		}
	}

	private static Assembly TryLoadAssembly(string assemblyName)
	{
		try
		{
			return Assembly.Load(new AssemblyName(assemblyName));
		}
		catch
		{
		}

		try
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string localAssemblyPath = Path.Combine(baseDirectory, assemblyName + ".dll");
			if (File.Exists(localAssemblyPath))
			{
				return Assembly.LoadFrom(localAssemblyPath);
			}

			string packagesDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..\\..\\packages"));
			if (Directory.Exists(packagesDirectory))
			{
				string[] matches = Directory.GetFiles(packagesDirectory, assemblyName + ".dll", SearchOption.AllDirectories);
				if (matches.Length > 0)
				{
					return Assembly.LoadFrom(matches[0]);
				}
			}
		}
		catch
		{
		}

		return null;
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

	private static void SavePdfDocument(object pdfDocument, string outputFilePath)
	{
		InvokeMethodByName(pdfDocument, "Save", new object[] { outputFilePath });
	}

	private static void CloseOrDispose(object instance)
	{
		if (instance == null)
		{
			return;
		}

		try
		{
			MethodInfo closeMethod = instance.GetType().GetMethod("Close", Type.EmptyTypes);
			if (closeMethod != null)
			{
				closeMethod.Invoke(instance, null);
			}
		}
		catch
		{
		}

		try
		{
			MethodInfo disposeMethod = instance.GetType().GetMethod("Dispose", Type.EmptyTypes);
			if (disposeMethod != null)
			{
				disposeMethod.Invoke(instance, null);
			}
		}
		catch
		{
		}
	}

	public void convertirAdoc(string ruta, string formato)
	{
		if (formato == "WORD")
		{
			try
			{
				PdfFocus pdfFocus = new PdfFocus();
				pdfFocus.OpenPdf(ruta);
				pdfFocus.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Docx;
				if (pdfFocus.ToWord(rtdestino + "doc.docx") == 0)
				{
					Process.Start(rtdestino + "doc.docx");
				}
				pdfFocus.ClosePdf();
				bar2.Value = 100;
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ingrese el archivo correcto " + ex.InnerException);
				return;
			}
		}
		panel2.Enabled = false;
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
			if (Path.GetExtension(path).Equals(".docx") && nombreft.Equals("WORD") && apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
			if (Path.GetExtension(path).Equals(".xlsx") && nombreft.Equals("EXCEL") && apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
			if (Path.GetExtension(path).Equals(".pptx") && nombreft.Equals("POWERPOINT") && apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
			if (Path.GetExtension(path).Equals(".pdf") && nombreft2.Equals("WORD") && !apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
			if (Path.GetExtension(path).Equals(".pdf") && nombreft2.Equals("EXCEL") && !apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
			if (Path.GetExtension(path).Equals(".pdf") && nombreft2.Equals("POWERPOINT") && !apdf)
			{
				e.Effect = DragDropEffects.Copy;
				panel2.Cursor = Cursors.Hand;
				return;
			}
		}
		e.Effect = DragDropEffects.None;
		panel2.Cursor = Cursors.No;
	}

	private void cbof_SelectedIndexChanged(object sender, EventArgs e)
	{
		nombreft = cbof.SelectedItem.ToString();
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
		bar2.Value = 0;
		lbestado.Text = "n/a";
       rutaArchivoSeleccionado = "";
		lbro.Text = "n/a";
	}

	private void button5_Click(object sender, EventArgs e)
	{
		Dispose();
		Principal principal = new Principal();
		principal.Show();
	}

	private void btay_Click(object sender, EventArgs e)
	{
     MessageBox.Show("Aviso:\nLa conversion de Office a PDF ahora usa Syncfusion y no requiere Microsoft Office instalado.\nLa conversion de PDF a Office aun esta en desarrollo y puede tener resultados limitados.");
	}

	private void button4_Click_1(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void button6_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		panel2.Enabled = true;
		cbof.Enabled = true;
		bar2.Value = 0;
		lbestado.Text = "n/a";
       rutaArchivoSeleccionado = "";
		lbro.Text = "n/a";
	}

	private void button2_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.Description = "Seleccione carpeta de destino";
		folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
		DialogResult dialogResult = folderBrowserDialog.ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
			lbdt.Text = folderBrowserDialog.SelectedPath;
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.btay = new System.Windows.Forms.Button();
		this.lbestado = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.pdfaof = new System.Windows.Forms.RadioButton();
		this.ofapdf = new System.Windows.Forms.RadioButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.button3 = new System.Windows.Forms.Button();
		this.cbpdf = new System.Windows.Forms.ComboBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.cbof = new System.Windows.Forms.ComboBox();
		this.bar2 = new System.Windows.Forms.ProgressBar();
		this.lbdt = new System.Windows.Forms.Label();
		this.lbro = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
        this.button7 = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.btay);
		this.panel1.Controls.Add(this.lbestado);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.bar2);
		this.panel1.Controls.Add(this.lbdt);
		this.panel1.Controls.Add(this.lbro);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.button2);
      this.panel1.Controls.Add(this.button7);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Location = new System.Drawing.Point(12, 61);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(757, 404);
		this.panel1.TabIndex = 0;
		this.btay.BackColor = System.Drawing.SystemColors.ActiveCaption;
		this.btay.Location = new System.Drawing.Point(703, 3);
		this.btay.Name = "btay";
		this.btay.Size = new System.Drawing.Size(49, 23);
		this.btay.TabIndex = 11;
		this.btay.Text = "?";
		this.btay.UseVisualStyleBackColor = false;
		this.btay.Click += new System.EventHandler(btay_Click);
		this.lbestado.AutoSize = true;
		this.lbestado.Location = new System.Drawing.Point(255, 296);
		this.lbestado.Name = "lbestado";
		this.lbestado.Size = new System.Drawing.Size(24, 13);
		this.lbestado.TabIndex = 10;
		this.lbestado.Text = "n/a";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(214, 296);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 13);
		this.label4.TabIndex = 9;
		this.label4.Text = "Estado:";
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.pdfaof);
		this.panel3.Controls.Add(this.ofapdf);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Location = new System.Drawing.Point(441, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(234, 327);
		this.panel3.TabIndex = 8;
		this.pdfaof.AutoSize = true;
		this.pdfaof.Location = new System.Drawing.Point(103, 17);
		this.pdfaof.Name = "pdfaof";
		this.pdfaof.Size = new System.Drawing.Size(86, 17);
		this.pdfaof.TabIndex = 6;
		this.pdfaof.Text = "PDF a Office";
		this.pdfaof.UseVisualStyleBackColor = true;
		this.pdfaof.CheckedChanged += new System.EventHandler(pdfaof_CheckedChanged);
		this.ofapdf.AutoSize = true;
		this.ofapdf.Checked = true;
		this.ofapdf.Location = new System.Drawing.Point(12, 17);
		this.ofapdf.Name = "ofapdf";
		this.ofapdf.Size = new System.Drawing.Size(86, 17);
		this.ofapdf.TabIndex = 5;
		this.ofapdf.TabStop = true;
		this.ofapdf.Text = "Office a PDF";
		this.ofapdf.UseVisualStyleBackColor = true;
		this.ofapdf.CheckedChanged += new System.EventHandler(ofapdf_CheckedChanged);
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.button3);
		this.panel5.Controls.Add(this.cbpdf);
		this.panel5.Enabled = false;
		this.panel5.Location = new System.Drawing.Point(12, 194);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(200, 117);
		this.panel5.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(3, 11);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 13);
		this.label8.TabIndex = 3;
		this.label8.Text = "PDF a Office";
		this.button3.Location = new System.Drawing.Point(9, 76);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(107, 23);
		this.button3.TabIndex = 2;
		this.button3.Text = "Otra Conversion";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.cbpdf.FormattingEnabled = true;
		this.cbpdf.Location = new System.Drawing.Point(3, 41);
		this.cbpdf.Name = "cbpdf";
		this.cbpdf.Size = new System.Drawing.Size(171, 21);
		this.cbpdf.TabIndex = 1;
		this.cbpdf.SelectedIndexChanged += new System.EventHandler(cbpdf_SelectedIndexChanged);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.button1);
		this.panel4.Controls.Add(this.cbof);
		this.panel4.Location = new System.Drawing.Point(12, 52);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(200, 114);
		this.panel4.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(3, 10);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(68, 13);
		this.label7.TabIndex = 1;
		this.label7.Text = "Office a PDF";
		this.button1.Location = new System.Drawing.Point(9, 75);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(107, 23);
		this.button1.TabIndex = 2;
		this.button1.Text = "Otra Conversion";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.cbof.FormattingEnabled = true;
		this.cbof.Location = new System.Drawing.Point(3, 41);
		this.cbof.Name = "cbof";
		this.cbof.Size = new System.Drawing.Size(171, 21);
		this.cbof.TabIndex = 1;
		this.cbof.SelectedIndexChanged += new System.EventHandler(cbof_SelectedIndexChanged);
		this.bar2.Location = new System.Drawing.Point(214, 266);
		this.bar2.Name = "bar2";
		this.bar2.Size = new System.Drawing.Size(153, 23);
		this.bar2.TabIndex = 7;
		this.lbdt.AutoSize = true;
		this.lbdt.Location = new System.Drawing.Point(199, 372);
		this.lbdt.Name = "lbdt";
		this.lbdt.Size = new System.Drawing.Size(24, 13);
		this.lbdt.TabIndex = 6;
		this.lbdt.Text = "n/a";
		this.lbro.AutoSize = true;
		this.lbro.Location = new System.Drawing.Point(199, 341);
		this.lbro.Name = "lbro";
		this.lbro.Size = new System.Drawing.Size(24, 13);
		this.lbro.TabIndex = 5;
		this.lbro.Text = "n/a";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(116, 372);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 13);
		this.label3.TabIndex = 4;
		this.label3.Text = "Ruta Destino:";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(116, 341);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "Ruta Origen:";
		this.button2.Location = new System.Drawing.Point(3, 367);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(86, 23);
		this.button2.TabIndex = 3;
		this.button2.Text = "Ruta Destino";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
       this.button7.Location = new System.Drawing.Point(16, 266);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(126, 23);
		this.button7.TabIndex = 12;
		this.button7.Text = "Iniciar conversion";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Click += new System.EventHandler(button7_Click);
		this.panel2.AllowDrop = true;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Location = new System.Drawing.Point(16, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(400, 233);
		this.panel2.TabIndex = 0;
		this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(panel2_DragDrop);
		this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(panel2_DragEnter);
		this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(panel2_MouseDown);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(125, 102);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(115, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Arrastre el archivo aqui";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Times New Roman", 24f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(344, 36);
		this.label6.TabIndex = 1;
		this.label6.Text = "Convertidor Office a PDF";
                this.button5.Image = null;
		this.button5.Location = new System.Drawing.Point(670, 12);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(99, 43);
		this.button5.TabIndex = 9;
		this.button5.Text = "Principal";
		this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
                this.button6.Image = null;
		this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button6.Location = new System.Drawing.Point(558, 478);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(100, 43);
		this.button6.TabIndex = 29;
		this.button6.Text = "Minimizar";
		this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
              this.button4.Image = null;
		this.button4.Location = new System.Drawing.Point(670, 478);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(108, 44);
		this.button4.TabIndex = 30;
		this.button4.Text = "Salir";
		this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 530);
		base.ControlBox = false;
		base.Controls.Add(this.button4);
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button5);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.panel1);
		base.Name = "Offices";
		this.Text = "Offices a PDF";
		base.Load += new System.EventHandler(Offices_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
}
