using image_editor.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace image_editor
{
    public partial class MainWindow : Window
    {
		private double _targetScale;
		private double _currentScale;

        public MainWindow()
        {
            InitializeComponent();
			MainWindowViewModel vm = new MainWindowViewModel();
			DataContext = vm;

			_currentScale = 1.0;
			_targetScale = 1.0;

			CompositionTarget.Rendering += ScaleCanvas;
		}

		private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;

			Point imagePos = e.GetPosition(MainCanvas);
			Point bitmapPos = new Point(imagePos.X * vm.PixelWidth / ImageBorder.Width, imagePos.Y * vm.PixelHeight / ImageBorder.Height);

			vm.HandleMousLeftButttonDown(bitmapPos);
			MainCanvas.CaptureMouse();
		}
		private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			MainCanvas.ReleaseMouseCapture();
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;

			Point imagePos = e.GetPosition(MainCanvas);
			Point bitmapPos = new Point(imagePos.X * vm.PixelWidth / ImageBorder.Width, imagePos.Y * vm.PixelHeight / ImageBorder.Height);

			if (e.LeftButton == MouseButtonState.Pressed && MainCanvas.IsMouseCaptured)
			{
				vm.HandleMouseLeftClickDrag(bitmapPos);
			}
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;
			if (!MainCanvas.IsMouseDirectlyOver) vm.SetCanvasStatus(false);
		}

		private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;
			vm.SetCanvasStatus(true);
		}

		private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			
		}

		private void ScaleCanvas(object? sender, EventArgs e)
		{
			_currentScale = Double.Lerp(_currentScale, _targetScale, 1 - Math.Exp(-0.3));

			_currentScale = Math.Clamp(_currentScale, 0.2, 20);

			if (DataContext is not MainWindowViewModel vm) return;

			ImageBorder.Width = vm.PixelWidth * _currentScale;
			ImageBorder.Height = vm.PixelHeight * _currentScale;
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				e.Handled = true;

				double scale = 1 + Math.Sign(e.Delta) * 0.2;
				_targetScale = _currentScale * scale;
			}
			else
			{
				e.Handled = false;
			}
		}
	}
}
