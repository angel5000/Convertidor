namespace ConvertidorImagenes.Models
{
	public class ImageConversionRequest
	{
		public string SourcePath { get; set; }

		public string DestinationDirectory { get; set; }

		public string OutputName { get; set; }

		public string TargetFormat { get; set; }

		public bool OpenDestinationWhenDone { get; set; }
	}
}
