
using image_editor.Model;
using image_editor.MVVM;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace image_editor.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
		private MainCanvas _canvas;
		private System.Drawing.Point _prevMousePosition;
		public MainCanvas Canvas
		{
			get { return _canvas; }
			set
			{
				_canvas = value;
				OnPropertyChanged();
			}
		}

		public MainWindowViewModel()
		{
			Canvas = new MainCanvas(200, 200);
			_prevMousePosition = new System.Drawing.Point();
		}

		public void HandleMouseLeftClickDrag(Point mousePos)
		{
			System.Drawing.Point p0 = _prevMousePosition;
			System.Drawing.Point p1 = new System.Drawing.Point((int)double.Round(mousePos.X), (int)double.Round(mousePos.Y));

			Canvas.DrawPixelsBetween(p0, p1);

			_prevMousePosition = p1;
		}

		public void HandleMousLeftButttonDown(Point mousePos)
		{
			Canvas.SetPixel((int)double.Round(mousePos.X), (int)double.Round(mousePos.Y), Colors.Black);
			SetPreviousMousePosition(mousePos);
		}

		public void SetPreviousMousePosition(Point mousePos)
		{
			_prevMousePosition = new System.Drawing.Point((int)double.Round(mousePos.X), (int)double.Round(mousePos.Y));
		}

		public void SetCanvasStatus(bool status)
		{
			Canvas.IsActive = status;
		}

		public void SaveAsPNG(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(Canvas.WriteableBitmap));
				encoder.Save(stream);
			}
		}

		public void OpenPNG(string path)
		{
			BitmapImage image = new BitmapImage(new Uri(path));
			Canvas.LoadBitmap(image);
		}
    }
}
