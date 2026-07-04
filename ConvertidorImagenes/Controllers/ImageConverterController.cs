using System;
using ConvertidorImagenes.Models;
using ConvertidorImagenes.Services;

namespace ConvertidorImagenes.Controllers
{
	public class ImageConverterController
	{
		private readonly ImageConversionService conversionService;

		public ImageConverterController(ImageConversionService conversionService)
		{
			if (conversionService == null)
			{
				throw new ArgumentNullException("conversionService");
			}

			this.conversionService = conversionService;
		}

		public string DefaultDestination
		{
			get { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
		}

		public string[] SupportedFormats
		{
			get { return conversionService.SupportedFormats; }
		}

		public bool IsSupportedImageFile(string fileName)
		{
			return conversionService.IsSupportedImageFile(fileName);
		}

		public bool RequiresPreviewBypass(string fileName)
		{
			return conversionService.RequiresPreviewBypass(fileName);
		}

		public ImageConversionResult ConvertImage(string sourcePath, string destinationDirectory, string outputName, string targetFormat, bool openDestinationWhenDone)
		{
			ImageConversionRequest request = new ImageConversionRequest
			{
				SourcePath = sourcePath,
				DestinationDirectory = destinationDirectory,
				OutputName = outputName,
				TargetFormat = targetFormat,
				OpenDestinationWhenDone = openDestinationWhenDone
			};

			return conversionService.Convert(request);
		}
	}
}
