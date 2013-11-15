using Microsoft.Win32;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TrackApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //слагам ги глобални за по-лесен достъп из класа
        OpenFileDialog loadFileDialog;
        string inputVideoPath;
        string outputVideoPath;
        public MainWindow()
        {
            InitializeComponent();
            loadFileDialog = new OpenFileDialog();
        }

        private void btnLoadClick(object sender, RoutedEventArgs e)
        {
            loadFileDialog.Filter = "Video Files (.avi)|*.avi";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            inputVideoPath = loadFileDialog.FileName;
        }
        private void btnAdjustSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings windowSettings = new Settings();
            windowSettings.Topmost = true;
            windowSettings.Show();
        }
        private void btnLoadGPXClick(object sender, RoutedEventArgs e)
        {
            loadFileDialog.Filter = "GPX Files (.gpx)|*.gpx";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            inputVideoPath = loadFileDialog.FileName;
        }
        #region hoverEffects
        private void btnLoadVideo_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconVideo_enter.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnLoadVideo.Background = imgBrush;
        }

        private void btnLoadVideo_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconVideo_leave.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnLoadVideo.Background = imgBrush;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnAdjustSettings_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconSettings_enter.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnAdjustSettings.Background = imgBrush;
        }
        private void btnAdjustSettings_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconSettings_Leave.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnAdjustSettings.Background = imgBrush;
        }

        private void btnLoadGpx_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconGPX_enter.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnLoadGpx.Background = imgBrush;
        }
        private void btnLoadGpx_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri("Resources/iconGPX_leave.png", UriKind.Relative);
            imgBrush.ImageSource = new BitmapImage(uri);
            btnLoadGpx.Background = imgBrush;
        }
        #endregion

        private void btnOKClick(object sender, RoutedEventArgs e)
        {
            //тук може да извършваме проверка дали всичко ни е въведено, след което да инициализираме класове от бекенда, ако не да пращаме message box
        }
    }
        
}
