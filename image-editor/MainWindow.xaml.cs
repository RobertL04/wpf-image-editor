using image_editor.Model;
using image_editor.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace image_editor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			MainWindowViewModel vm = new MainWindowViewModel();
			DataContext = vm;
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

		
	}
}
