
using image_editor.Model;
using image_editor.MVVM;

namespace image_editor.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
		private MainCanvas _canvas;
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
		}
    }
}
