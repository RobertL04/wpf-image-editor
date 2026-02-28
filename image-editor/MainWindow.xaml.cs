using image_editor.ViewModel;
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
		private ScaleTransform _scaleTransform;

        public MainWindow()
        {
            InitializeComponent();
			MainWindowViewModel vm = new MainWindowViewModel();
			DataContext = vm;

			MainCanvas.RenderTransformOrigin = new Point(0.5f, 0.5f);
			_targetScale = 1;
			_scaleTransform = new ScaleTransform(1, 1);
			MainCanvas.RenderTransform = _scaleTransform;

			CompositionTarget.Rendering += ScaleCanvas;
        }

		private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;
			
			Point pos = e.GetPosition(MainCanvas);
			vm.HandleMousLeftButttonDown(pos);
			MainCanvas.CaptureMouse();
		}
		private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			MainCanvas.ReleaseMouseCapture();
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (DataContext is not MainWindowViewModel vm) return;

			Point pos = e.GetPosition(MainCanvas);

			if (e.LeftButton == MouseButtonState.Pressed && MainCanvas.IsMouseCaptured)
			{
				vm.HandleMouseLeftClickDrag(pos);
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
			_scaleTransform.ScaleX = Double.Lerp(_scaleTransform.ScaleX, _targetScale, 1 - Math.Exp(-0.3));

			_scaleTransform.ScaleX = Math.Clamp(_scaleTransform.ScaleX, 0.4, 20);
			_scaleTransform.ScaleY = _scaleTransform.ScaleX;

			ImageBorder.Width = MainCanvas.ActualWidth * _scaleTransform.ScaleX;
			ImageBorder.Height = MainCanvas.ActualHeight * _scaleTransform.ScaleY;
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				e.Handled = true;

				double scale = Math.Sign(e.Delta) * 0.2;
				_targetScale = _scaleTransform.ScaleX + _scaleTransform.ScaleX * scale;
			}
			else
			{
				e.Handled = false;
			}
		}
	}
}
