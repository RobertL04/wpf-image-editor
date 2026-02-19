using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace image_editor.Model
{
	class MainCanvas
	{
		public WriteableBitmap WriteableBitmap { get; set; }
		public int PixelWidth { get; set; }
		public int PixelHeight { get; set; }

		private byte[] _frameBuffer;
		private Int32Rect _dirtyRect;
		private int _minX, _maxX;
		private int _minY, _maxY;

		public MainCanvas(int width, int height)
		{
			PixelWidth = width;
			PixelHeight = height;
			InitFrameBuffer();
			_dirtyRect = new Int32Rect();


			WriteableBitmap = new WriteableBitmap(PixelWidth, PixelHeight, 96, 96, PixelFormats.Bgra32, null);
			WriteableBitmap.WritePixels(new Int32Rect(0, 0, PixelWidth, PixelHeight), _frameBuffer, PixelWidth * 4, 0);
			CompositionTarget.Rendering += OnRender;

			/*DrawPixelsBetween(new(0,199), new(99,149));
			DrawPixelsBetween(new(0,199), new(99,0));
			DrawPixelsBetween(new(199,199), new(99,149));
			DrawPixelsBetween(new(199,199), new(99,0));*/
		}

		private void InitFrameBuffer()
		{
			_frameBuffer = new byte[4 * PixelWidth * PixelHeight];
			Array.Fill(_frameBuffer, (byte)255);
		}

		public void DrawPixelsBetween(Point p0, Point p1)
		{
			int xDiff = p1.X - p0.X;
			int yDiff = p1.Y - p0.Y;

			int dx = xDiff < 0 ? -2 * xDiff : 2 * xDiff;
			int dy = yDiff < 0 ? 2 * yDiff : -2 * yDiff;


			int d = dy + dx;
			int x = p0.X;
			int y = p0.Y;

			if(xDiff >= 0)
			{
				bool isSteep = xDiff < -yDiff;
				while (x <= p1.X)
				{
					SetPixel(x, y, Colors.Black);
					if (d < 0)
					{
						if (!isSteep)
						{
							d += dy + dx;
							y--;
						}
						else
						{
							d += dx;
						}
					}
					else
					{
						if (!isSteep)
						{
							d += dy;
						}
						else
						{
							d += dy + dx;
							x++;
						}
					}

					if (!isSteep) x++;
					else y--;

				}
			}
			else
			{
				bool isSteep = -xDiff < -yDiff;
				while (x > p1.X)
				{
					SetPixel(x, y, Colors.Black);
					if (d < 0)
					{
						if (!isSteep)
						{
							d += dy + dx;
							y--;
						}
						else
						{
							d += dx;
						}
					}
					else
					{
						if (!isSteep)
						{
							d += dy;
						}
						else
						{
							d += dy + dx;
							x--;
						}
					}

					if (!isSteep) x--;
					else y--;

				}
			}
		}

		public void SetPixel(int x, int y, Color c)
		{
			if(x < 0 || x >= PixelWidth ||  y < 0 || y >= PixelHeight) return;
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
	}
}
