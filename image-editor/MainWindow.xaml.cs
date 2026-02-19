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
			Point pos = e.GetPosition(MainCanvas);
			if (DataContext is MainWindowViewModel vm) vm.HandleMousLeftButttonDown(pos);
		}

		private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			Point pos = e.GetPosition(MainCanvas);
			Debug.WriteLine(pos);
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (DataContext is MainWindowViewModel vm) vm.HandleMouseLeftClickDrag(pos);
			}
		}
	}
}
