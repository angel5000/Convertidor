namespace ConvertidorImagenes.Models
{
	public class ImageConversionResult
	{
		public ImageConversionResult(string outputPath)
		{
			OutputPath = outputPath;
		}

		public string OutputPath { get; private set; }
	}
}
