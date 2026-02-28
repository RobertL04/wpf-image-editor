using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace image_editor.Model
{
	class MainCanvas
	{
		public WriteableBitmap WriteableBitmap { get; set; }
		public int PixelWidth { get; set; }
		public int PixelHeight { get; set; }

		public bool IsActive { get; set; }

		private byte[] _frameBuffer;
		private Int32Rect _dirtyRect;
		private int _minX, _maxX;
		private int _minY, _maxY;

		public MainCanvas(int width, int height)
		{
			PixelWidth = width;
			PixelHeight = height;
			InitFrameBuffer();
			IsActive = true;
			_dirtyRect = new Int32Rect();

			WriteableBitmap = new WriteableBitmap(PixelWidth, PixelHeight, 96, 96, PixelFormats.Bgra32, null);
			WriteableBitmap.WritePixels(new Int32Rect(0, 0, PixelWidth, PixelHeight), _frameBuffer, PixelWidth * 4, 0);
			CompositionTarget.Rendering += OnRender;

		}

		private void InitFrameBuffer()
		{
			_frameBuffer = new byte[4 * PixelWidth * PixelHeight];
			Array.Fill(_frameBuffer, (byte)255);
		}

		public void DrawPixelsBetween(Point p0, Point p1)
		{
			if (!IsActive) return;
			int x0 = p0.X, y0 = p0.Y, x1 = p1.X,  y1 = p1.Y;

			int dx = int.Abs(x1 - x0);
			int dy = int.Abs(y1 - y0);

			int xStep = x1 >= x0 ? 1 : -1;
			int yStep = y1 >= y0 ? 1 : -1;

			bool steep = dy > dx;

			if (steep)
			{
				Swap(ref x0, ref y0);
				Swap(ref x1, ref y1);
				Swap(ref dx, ref dy);
				Swap(ref xStep, ref yStep);
			}

			int err = dx;
			err >>= 1;

			for(int i = 0; i <= dx; i++)
			{
				if (!steep) SetPixel(x0, y0, Colors.Black);
				else SetPixel(y0, x0, Colors.Black);

				err += dy;
				x0 += xStep;
				if (err >= dx)
				{
					err -= dx;
					y0 += yStep;
				}

			}
		}

		private void Swap(ref int a, ref int b)
		{
			int temp = a; a = b; b = temp;
		}

		public void SetPixel(int x, int y, Color c)
		{
			if (!IsActive || x < 0 || x >= PixelWidth ||  y < 0 || y >= PixelHeight) return;
			int stride = PixelWidth * 4;
			_frameBuffer[4*x + stride * y] = c.B; // B
			_frameBuffer[4*x + stride * y + 1] = c.G; // G
			_frameBuffer[4*x + stride * y + 2] = c.R; // R
			_frameBuffer[4*x + stride * y + 3] = c.A; // A

			_minX = int.Min(x, _minX);
			_minY = int.Min(y, _minY);
			_maxX = int.Max(x, _maxX);
			_maxY = int.Max(y, _maxY);
		}

		private void SetBitmap()
		{
			int rectWidth = _maxX - _minX + 1;
			int rectHeight = _maxY - _minY + 1;

			rectWidth = int.Min(rectWidth, PixelWidth);
			rectHeight = int.Min(rectHeight, PixelHeight);

			_dirtyRect = new Int32Rect(_minX, _minY, rectWidth, rectHeight);

			WriteableBitmap.WritePixels(_dirtyRect, _frameBuffer, PixelWidth * 4, 0);
		}

		protected void OnRender(object? sender, EventArgs e)
		{
			SetBitmap();
		}

		public void LoadBitmap(BitmapImage image)
		{
			PixelWidth = image.PixelWidth;
			PixelHeight = image.PixelHeight;
			image.CopyPixels(_frameBuffer, PixelWidth * 4, 0);
			WriteableBitmap.WritePixels(new Int32Rect(0, 0, PixelWidth, PixelHeight), _frameBuffer, PixelWidth * 4, 0);
		}
	}
}
