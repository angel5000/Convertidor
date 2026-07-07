// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.Program
using System;
using System.Windows.Forms;
using ConvertidorImagenes;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		// TODO: Pega tu clave de licencia de Syncfusion aquí abajo (Versión 33.2.*)
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWXhednZVRWddUkBzXEVWYEo=");
		
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Principal());
	}
}
