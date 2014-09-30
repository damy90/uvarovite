using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TrackApp.Wpf
{
    /// <summary>
    /// Interaction logic for Preview.xaml
    /// </summary>
    public partial class Preview : Window
    {
        public Preview(string path)
        {
            InitializeComponent();
            Uri uri = new Uri(path, UriKind.Absolute);
            imgPreview.Source = new BitmapImage(uri);
        }
    }
}
