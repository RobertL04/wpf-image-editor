using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace image_editor.Model
{
	class MainCanvas
	{
		public WriteableBitmap WriteableBitmap { get; set; }
		public int PixelWidth { get; set; }
		public int PixelHeight { get; set; }

		public MainCanvas(int width, int height)
		{
			PixelWidth = width;
			PixelHeight = height;

			WriteableBitmap = new WriteableBitmap(PixelWidth, PixelHeight, 96, 96, PixelFormats.Bgra32, null);

			WriteableBitmap.WritePixels(new Int32Rect(0, 0, PixelWidth, PixelHeight), InitPixels(), PixelWidth * 4, 0);
		}

		private byte[] InitPixels()
		{
			byte[] pixels = new byte[4 * PixelWidth * PixelHeight];

			Random random = new Random();

			random.NextBytes(pixels);

			return pixels;
		}

	}
}
