using System.Windows;

namespace NetworkProgrammingCW_HttpClient
{
    public partial class ImageWindow : Window
    {
        public ImageWindow(string imageSource)
        {
            InitializeComponent();

            DataContext = new ImageViewModel(imageSource);
        }
    }
}