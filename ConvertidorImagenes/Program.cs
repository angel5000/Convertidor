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
		// Usando la clave de licencia desde ApiKeys.cs (que está en .gitignore)
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(ApiKeys.SyncfusionLicenseKey);
		
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Principal());
	}
}
