using image_editor.ViewModel;
using System.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace image_editor.View
{
	public partial class MenuBar : UserControl
	{
		public MenuBar()
		{
			InitializeComponent();
		}


		private void Open_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.Filter = "PNG File | *.png";

			bool? success = dialog.ShowDialog();

			if (success == true)
			{
				string path = dialog.FileName;

				if (DataContext is not MainWindowViewModel vm) return;
				vm.OpenPNG(path);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();

			dialog.Filter = "PNG File | *.png";

			bool? success = dialog.ShowDialog();

			if(success == true)
			{
				string path = dialog.FileName;

				if (DataContext is not MainWindowViewModel vm) return;
				vm.SaveAsPNG(path);
			}
		}
	}
}
