using System;
using System.Diagnostics;
using System.IO;
using ImageMagick;
using ConvertidorImagenes.Models;

namespace ConvertidorImagenes.Services
{
	public class ImageConversionService
	{
		public string[] SupportedFormats
		{
			get { return new string[6] { "JPG", "PNG", "WEBP", "GIF", "BITMAP(BMP)", "ICO" }; }
		}

		public bool IsSupportedImageFile(string fileName)
		{
			string extension = Path.GetExtension(fileName).ToLowerInvariant();
			return extension == ".jpg"
				|| extension == ".jpeg"
				|| extension == ".png"
				|| extension == ".gif"
				|| extension == ".webp"
				|| extension == ".bmp"
				|| extension == ".ico";
		}

		public bool RequiresPreviewBypass(string fileName)
		{
			return Path.GetExtension(fileName).Equals(".webp", StringComparison.OrdinalIgnoreCase);
		}

		public ImageConversionResult Convert(ImageConversionRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			if (!File.Exists(request.SourcePath))
			{
				throw new FileNotFoundException("No se encontro la imagen de origen.", request.SourcePath);
			}

			if (string.IsNullOrWhiteSpace(request.DestinationDirectory))
			{
				throw new ArgumentException("La ruta de destino no puede estar vacia.", "DestinationDirectory");
			}

			Directory.CreateDirectory(request.DestinationDirectory);

			MagickFormat magickFormat;
			string extension;
			ResolveFormat(request.TargetFormat, out magickFormat, out extension);

			string outputName = string.IsNullOrWhiteSpace(request.OutputName)
				? Path.GetFileName(request.SourcePath)
				: request.OutputName;

			string outputPath = GetAvailableOutputPath(request.DestinationDirectory, outputName, extension);

			using (MagickImage image = new MagickImage(request.SourcePath))
			{
				image.Format = magickFormat;
				image.Write(outputPath);
			}

			if (request.OpenDestinationWhenDone)
			{
				Process.Start("explorer.exe", request.DestinationDirectory);
			}

			return new ImageConversionResult(outputPath);
		}

		private static void ResolveFormat(string format, out MagickFormat magickFormat, out string extension)
		{
			switch (format)
			{
				case "JPG":
					magickFormat = MagickFormat.Jpg;
					extension = "jpg";
					return;
				case "PNG":
					magickFormat = MagickFormat.Png;
					extension = "png";
					return;
				case "WEBP":
					magickFormat = MagickFormat.WebP;
					extension = "webp";
					return;
				case "GIF":
					magickFormat = MagickFormat.Gif;
					extension = "gif";
					return;
				case "BITMAP(BMP)":
					magickFormat = MagickFormat.Bmp;
					extension = "bmp";
					return;
				case "ICO":
					magickFormat = MagickFormat.Ico;
					extension = "ico";
					return;
				default:
					throw new NotSupportedException("Formato no soportado: " + format);
			}
		}

		private static string GetAvailableOutputPath(string destinationDirectory, string outputName, string extension)
		{
			string baseName = outputName + "-Convertido";
			string path = Path.Combine(destinationDirectory, baseName + "." + extension);

			if (!File.Exists(path))
			{
				return path;
			}

			int counter = 1;
			string candidate;
			do
			{
				candidate = Path.Combine(destinationDirectory, baseName + "(" + counter + ")." + extension);
				counter++;
			}
			while (File.Exists(candidate));

			return candidate;
		}
	}
}
