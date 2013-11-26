using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
namespace TrackApp
{
    //render video-commented out-->will be in main window
    //TODO collect data from user
    public partial class MainWindow : Window
    {
        OpenFileDialog loadFileDialog;
        const int spaceBetweenGrids = 5;
        const int rowHeight = 40;
        const int colWidth = 150;
        ProjectSettings settings;

        public MainWindow()
        {

            InitializeComponent();
            settings = ProjectSettings.GetSettings();
            loadFileDialog = new OpenFileDialog();
            this.grdControlButtons.Visibility = Visibility.Visible;
            this.grdFiles.Visibility = Visibility.Visible;
            this.grdSync.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Hidden;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
            this.grdEnabledTrack.Visibility = Visibility.Hidden;
            this.grdEnabledSpeed.Visibility = Visibility.Hidden;
            this.grdEnabledDistance.Visibility = Visibility.Hidden;
            this.grdEnabledPM.Visibility = Visibility.Hidden;
            this.grdEnabledOI.Visibility = Visibility.Hidden;
            this.grdEnabledMap.Visibility = Visibility.Hidden;

            foreach (var format in Enum.GetValues(typeof(VideoFormats)))
            {
                this.cmbEncoding.Items.Add(format);
            }
            VideoCompositor.RenderVideo();
        }

        #region Helping Methods     
        private void ResizeGrid(Grid grid)
        {
            if (grid.Height == rowHeight)
            {
                grid.Height = grid.RowDefinitions.Count * rowHeight;
            }
            else
            {
                grid.Height = rowHeight;
            }
        }

        private void AnimationDown(Grid grd)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = grd.Margin;
            ta.To = new Thickness(grd.Margin.Left, (3 * rowHeight + spaceBetweenGrids), 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            grd.BeginAnimation(Grid.MarginProperty, ta);
        }
        private void AnimationUp(Grid grid)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = grid.Margin;
            ta.To = new Thickness(grid.Margin.Left, rowHeight + spaceBetweenGrids, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            grid.BeginAnimation(Grid.MarginProperty, ta);
        }
        private void ChangeIconBackground(string path, System.Windows.Controls.Button btn)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            imgBrush.ImageSource = new BitmapImage(uri);
            btn.Background = imgBrush;
        }
        private bool CheckFileGridInput()
        {
            if (settings.VideoInputPath == null || settings.VideoInputPath == string.Empty)
            {
                return false;
            }
            else if (settings.GPXPath == null || settings.GPXPath == string.Empty)
            {
                return false;
            }
            else if (this.txtOutputFileName.Text == null || this.txtOutputFileName.Text == string.Empty)
            {
                return false;
            }
            else if (settings.Format == null)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region ClickEH

        private void btnFiles_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Visible;
            this.grdSync.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Hidden;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;

        }
        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            this.grdSync.Visibility = Visibility.Visible;
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Hidden;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnWidgets_Click(object sender, RoutedEventArgs e)
        {
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Visible;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
            this.grdSync.Visibility = Visibility.Hidden;
        }
        private void btnTrack_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Visible;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnSpeed_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Visible;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnDistance_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Visible;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnPositionMarker_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Visible;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnOverlayImage_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Visible;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Hidden;
        }
        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            this.grdFiles.Visibility = Visibility.Hidden;
            this.grdWidgets.Visibility = Visibility.Visible;
            this.grdSpeed.Visibility = Visibility.Hidden;
            this.grdTrack.Visibility = Visibility.Hidden;
            this.grdDistance.Visibility = Visibility.Hidden;
            this.grdOverlayImage.Visibility = Visibility.Hidden;
            this.grdPositionMarker.Visibility = Visibility.Hidden;
            this.grdMap.Visibility = Visibility.Visible;
        }
        private void btnLoadGPXClick(object sender, RoutedEventArgs e)
        {
            //GPXFileLoader g = new GPXFileLoader();
            //g.LoadPoints("../../koprivshtica-dushanci.gpx");
            loadFileDialog.Filter = "GPX Files (.gpx)|*.gpx";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            if (loadFileDialog != null)
            {
                settings.GPXPath = loadFileDialog.FileName;
                loadFileDialog.FileName = null;
            }
        }
        private void btnLoadVideoClick(object sender, RoutedEventArgs e)
        {
            loadFileDialog.Filter = "Video Files (.avi)|*.avi";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            if (loadFileDialog != null)
            {
                settings.VideoInputPath = loadFileDialog.FileName;
                loadFileDialog.FileName = null;
            }
        }
        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFileGridInput())
            {
                settings.VideoOutputPath = this.txtOutputFileName.Text;
                settings.TrackStart = Convert.ToInt32(udTrackHours.Value) * 3600f + Convert.ToInt32(udTrackMinutes.Value) * 60 + Convert.ToInt32(udTrackSeconds.Value);
                settings.VideoStart = Convert.ToInt32(udVideoStHours.Value) * 3600f + Convert.ToInt32(udVideoStMinutes.Value) * 60 + Convert.ToInt32(udVideoStSeconds.Value);
                settings.VideoStart = Convert.ToInt32(udVideoEndHours.Value) * 3600f + Convert.ToInt32(udVideoEndMinutes.Value) * 60 + Convert.ToInt32(udVideoEndSeconds.Value);
            }
            else
            {
                MessageBoxResult error = MessageBox.Show("Not all required fields filled!");
            }
        }
        #endregion
        #region HoverEffects
        public void btnWidgets_MouseEnter(object sender, MouseEventArgs e)
        {
        }
        public void btnWidgets_MouseLeave(object sender, MouseEventArgs e)
        {
        }
        public void btnFiles_MouseEnter(object sender, MouseEventArgs e)
        {
        }
        public void btnFiles_MouseLeave(object sender, MouseEventArgs e)
        {
        }
        private void btnTrack_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnTrack_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnSpeed_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnSpeed_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnDistance_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnDistance_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnPositionMarker_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnPositionMarker_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnOverlayImage_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnOverlayImage_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnMap_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnMap_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private void btnLoadVideo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/iconVideo_enter.png", btnLoadVideo);
        }
        private void btnLoadVideo_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/iconVideo_leave.png", btnLoadVideo);
        }
        private void btnLoadGPX_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/iconGPX_enter.png", btnLoadGPX);
        }
        private void btnLoadGPX_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/iconGPX_leave.png", btnLoadGPX);
        }
        private void btnSync_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_enter.png", btnSync);
        }
        private void btnSync_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_leave.png", btnSync);
        }
        #endregion
        private void cmbEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoFormats format = (VideoFormats)cmbEncoding.SelectedIndex;
            settings.Format = format;
        }

        private void cbEnableTrack_Checked(object sender, RoutedEventArgs e)
        {     
            this.grdEnabledTrack.Visibility = Visibility.Visible;
        }

        private void cbEnableTrack_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledTrack.Visibility = Visibility.Hidden;
        }

        private void cbEnableSpeed_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledSpeed.Visibility = Visibility.Hidden;
        }
        private void cbEnableSpeed_Checked(object sender, RoutedEventArgs e)
        {
            this.grdEnabledSpeed.Visibility = Visibility.Visible;
        }

        private void cbEnableDistance_Checked(object sender, RoutedEventArgs e)
        {
            this.grdEnabledDistance.Visibility = Visibility.Visible;
        }
        private void cbEnableDistance_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledDistance.Visibility = Visibility.Hidden;
        }

        private void cbEnablePM_Checked(object sender, RoutedEventArgs e)
        {
            this.grdEnabledPM.Visibility = Visibility.Visible;
        }

        private void cbEnablePM_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledPM.Visibility = Visibility.Hidden;
        }

        private void btnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            loadFileDialog.Filter = "JPG Files (.jpg)|*.jpg";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            if (loadFileDialog != null)
            {
                settings.overlayImageFile = loadFileDialog.FileName;
                loadFileDialog.FileName = null;
            }
        }

        private void cbEnableMap_Checked(object sender, RoutedEventArgs e)
        {
            this.grdEnabledMap.Visibility = Visibility.Visible;
        }

        private void cbEnableMap_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledMap.Visibility = Visibility.Hidden;
        }
    }
}