using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
namespace TrackApp
{
    /// <summary>
    /// Interaction logic for Preview.xaml
    /// </summary>
    public partial class Preview : Window
    {
        public Preview()
        {
            InitializeComponent();
            //tuk gurmi no basically trqbva samo da se naglasi urito i e gotovo
            //Uri uri = new Uri(@"pack://application:,,,bin/Debug/testPreviewFrame.png", UriKind.RelativeOrAbsolute);
            //imgPreview.Source = new BitmapImage(uri);
        }
    }
}
