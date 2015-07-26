using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TrackApp.Logic;

namespace TrackApp.Wpf
{
    // TODO collect data from user
    public partial class MainWindow : Window
    {
        private OpenFileDialog loadFileDialog;
        private SaveFileDialog saveFileDialog;
        private ProjectSettings settings;
        private SetPreviewTime prevTimeWind;

        // Preview prevWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            this.loadFileDialog = new OpenFileDialog();
            this.settings = ProjectSettings.GetSettings();
            this.IniializeVisibilities();
            this.InitializeContent();
        }

        #region Helping Methods
        private void IniializeVisibilities()
        {
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
        }

        private void InitializeContent()
        {
            // Video Settings
            this.cmbEncoding.SelectedIndex = (int)this.settings.Format;
            this.cmbSpeed.SelectedIndex = this.settings.VideoSpeed - 1;
            this.cmbOutputQuality.SelectedIndex = this.settings.VideoQuality - 1;

            // Track
            this.cbEnableTrack.IsChecked = this.settings.ShowTrack;
            this.txtTrackX.Text = this.settings.TrackPostion.X.ToString();
            this.txtTrackY.Text = this.settings.TrackPostion.Y.ToString();
            this.txtTrackHeight.Text = this.settings.TrackHeight.ToString();
            this.cpTrackTravelledColor.SelectedColor = Color.FromArgb(this.settings.TraveledTrackColor.A, this.settings.TraveledTrackColor.R, this.settings.TraveledTrackColor.G, this.settings.TraveledTrackColor.B);
            this.txtTrackTravelledWidth.Text = this.settings.TraveledTrackLineWidth.ToString();
            this.cpTrackWholeColor.SelectedColor = Color.FromArgb(this.settings.WholeTrackColor.A, this.settings.WholeTrackColor.R, this.settings.WholeTrackColor.G, this.settings.WholeTrackColor.B);
            this.txtTrackWholeWidth.Text = this.settings.WholeTrackLineWidth.ToString();
            this.cbShowTravelledTrack.IsChecked = this.settings.ShowTraveledTrack;

            // Position marker
            this.cbEnablePM.IsChecked = this.settings.ShowPositionMarker;
            this.txtPMSize.Text = this.settings.PositionMarkerSize.ToString();
            this.cpPMColor.SelectedColor = Color.FromArgb(this.settings.PositionMarkerColor.A, this.settings.PositionMarkerColor.R, this.settings.PositionMarkerColor.G, this.settings.PositionMarkerColor.B);

            // Overlay Image
            this.cbEnableOverlayImage.IsChecked = this.settings.ShowOverlayImage;
            this.txtOIX.Text = this.settings.OverlayImagePosition.X.ToString();
            this.txtOIY.Text = this.settings.OverlayImagePosition.Y.ToString();

            // Map
            this.cbEnableMap.IsChecked = this.settings.ShowMap;
            //this.txtMapHeight.Text = this.settings.MapHeight.ToString();
            this.dudMapOpacity.Value = this.settings.MapOpacity;

            // Distance
            this.cbEnableDistance.IsChecked = this.settings.ShowDistanceWidget;
            this.txtDistanceX.Text = this.settings.DistanceWidgetPosition.X.ToString();
            this.txtDistanceY.Text = this.settings.DistanceWidgetPosition.Y.ToString();
            this.cbDistanceFont.SelectedValue = new FontFamily(this.settings.PDistanceWidgetFont.FontFamily);
            this.txtDistanceFontSize.Value = this.settings.PDistanceWidgetFont.Size;
            this.cpDistanceColor.SelectedColor = Color.FromArgb(this.settings.DistanceWidgetColor.A, this.settings.DistanceWidgetColor.R, this.settings.DistanceWidgetColor.G, this.settings.DistanceWidgetColor.B);

            // Speed
            this.cbEnableSpeed.IsChecked = this.settings.ShowSpeedWidget;
            this.txtSpeedX.Text = this.settings.SpeedWidgetPosition.X.ToString();
            this.txtSpeedY.Text = this.settings.SpeedWidgetPosition.Y.ToString();
            this.cbSpeedFont.SelectedValue = new FontFamily(this.settings.PSpeedWidgetFont.FontFamily);
            this.txtSpeedFontSize.Value = (int)this.settings.PSpeedWidgetFont.Size;
            this.cpSpeedColor.SelectedColor = Color.FromArgb(this.settings.SpeedWidgetColor.A, this.settings.SpeedWidgetColor.R, this.settings.SpeedWidgetColor.G, this.settings.SpeedWidgetColor.B);
        }

        private void ChangeIconBackground(string path, System.Windows.Controls.Button btn)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            imgBrush.ImageSource = new BitmapImage(uri);
            btn.Background = imgBrush;
        }

        // TODO: do all parameters validation in TrackApp.Logic
        private bool CheckFileGridInput()
        {
            if (string.IsNullOrEmpty(this.settings.VideoInputPath))
            {
                return false;
            }
            else if (this.settings.GPXPath == null || this.settings.GPXPath == string.Empty)
            {
                return false;
            }
            else if (this.settings.Format == null)
            {
                return false;
            }

            return true;
        }

        private void InitializeSyncronization()
        {
            //Syncronization
            this.udTrackHours.Value = (int)this.settings.TrackStart / 3600;
            this.udTrackMinutes.Value = (int)(this.settings.TrackStart - this.udTrackHours.Value * 3600) / 60;
            this.udTrackSeconds.Value = (int)(this.settings.TrackStart - this.udTrackHours.Value * 3600 - this.udTrackMinutes.Value * 60);
            this.udTrackEndHours.Value = (int)this.settings.TrackEnd / 3600;
            this.udTrackEndMinutes.Value = (int)(this.settings.TrackEnd - this.udTrackEndHours.Value * 3600) / 60;
            this.udTrackEndSeconds.Value = (int)(this.settings.TrackEnd - this.udTrackEndHours.Value * 3600 - this.udTrackEndMinutes.Value * 60);
            this.udVideoStHours.Value = (int)this.settings.VideoStart / 3600;
            this.udVideoStMinutes.Value = (int)(this.settings.VideoStart - this.udVideoStHours.Value * 3600) / 60;
            this.udVideoStSeconds.Value = (int)(this.settings.VideoStart - this.udVideoStHours.Value * 3600 - this.udVideoStMinutes.Value * 60);
            this.udVideoEndHours.Value = (int)this.settings.VideoEnd / 3600;
            this.udVideoEndMinutes.Value = (int)(this.settings.VideoEnd - this.udVideoEndHours.Value * 3600) / 60;
            this.udVideoEndSeconds.Value = (int)(this.settings.VideoEnd - this.udVideoEndHours.Value * 3600 - this.udVideoEndMinutes.Value * 60);
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
            this.loadFileDialog.Filter = "GPX Files (.gpx)|*.gpx";
            this.loadFileDialog.FilterIndex = 1;
            this.loadFileDialog.Multiselect = false;
            this.loadFileDialog.ShowDialog();
            if (this.loadFileDialog != null)
            {
                this.txtTrackDir.Text = this.loadFileDialog.FileName;
                ProjectSettings.GetSettings().GPXPath = this.loadFileDialog.FileName;
                this.svTrackDir.Visibility = Visibility.Visible;
                this.loadFileDialog.FileName = null;
            }
        }

        private void btnLoadVideoClick(object sender, RoutedEventArgs e)
        {
            this.loadFileDialog.Filter = "Video Files (.avi)|*.avi";
            this.loadFileDialog.FilterIndex = 1;
            this.loadFileDialog.Multiselect = false;
            this.loadFileDialog.ShowDialog();
            if (this.loadFileDialog != null)
            {
                this.txtInputVideoDir.Text = this.loadFileDialog.FileName;
                ProjectSettings.GetSettings().VideoInputPath = this.loadFileDialog.FileName;
                this.svInputVideoDir.Visibility = Visibility.Visible;
                this.loadFileDialog.FileName = null;
            }
        }

        // BackgroundWorker worker;
        public ProgressWindow pd;

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            // TODO:add validations
            //settings.Serialize();
            //settings.VideoEnd = 300;
            //settings = settings.Deserialize();
            this.PassSettingsDown();
            //try
            //{
                //pd.ShowDialog();
                VideoCompositor.RenderVideo();
                MessageBox.Show("Success!");

                //comment this if you don't want to start the video immediately after the rendering
                Process.Start(this.settings.VideoOutputPath);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void PassSettingsDown()
        {
            //TODO:add validations
            //settings.Serialize();
            //settings.VideoEnd = 300;
            //settings = settings.Deserialize();
            this.settings.VideoOutputPath = this.txtOutputDir.Text;
            this.settings.TrackStart = Convert.ToInt32(udTrackHours.Value) * 3600f + Convert.ToInt32(udTrackMinutes.Value) * 60 + Convert.ToInt32(udTrackSeconds.Value);
            this.settings.TrackEnd = Convert.ToInt32(udTrackEndHours.Value) * 3600f + Convert.ToInt32(udTrackEndMinutes.Value) * 60 + Convert.ToInt32(udTrackEndSeconds.Value);
            this.settings.VideoStart = Convert.ToInt32(udVideoStHours.Value) * 3600f + Convert.ToInt32(udVideoStMinutes.Value) * 60 + Convert.ToInt32(udVideoStSeconds.Value);
            this.settings.VideoEnd = Convert.ToInt32(udVideoEndHours.Value) * 3600f + Convert.ToInt32(udVideoEndMinutes.Value) * 60 + Convert.ToInt32(udVideoEndSeconds.Value);
            this.settings.VideoSpeed = this.cmbSpeed.SelectedIndex + 1;
            this.settings.VideoQuality = this.cmbOutputQuality.SelectedIndex + 1;
            //settings.Format = (VideoFormats)this.cmbEncoding.SelectedIndex; //not tested
            //settings.BitRate = 20000000;
            //Track
            this.settings.ShowTrack = (bool)this.cbEnableTrack.IsChecked;
            if ((bool)this.cbEnableTrack.IsChecked)
            {
                this.settings.TrackPostion = new System.Drawing.Point(Convert.ToInt32(this.txtTrackX.Text), Convert.ToInt32(this.txtTrackY.Text));
                this.settings.TrackHeight = Convert.ToInt32(this.txtTrackHeight.Text);
                this.settings.TraveledTrackColor = System.Drawing.Color.FromArgb(this.cpTrackTravelledColor.SelectedColor.A, this.cpTrackTravelledColor.SelectedColor.R, this.cpTrackTravelledColor.SelectedColor.G, this.cpTrackTravelledColor.SelectedColor.B);
                this.settings.TraveledTrackLineWidth = Convert.ToInt32(this.txtTrackTravelledWidth.Text);
                this.settings.WholeTrackColor = System.Drawing.Color.FromArgb(this.cpTrackWholeColor.SelectedColor.A, this.cpTrackWholeColor.SelectedColor.R, this.cpTrackWholeColor.SelectedColor.G, this.cpTrackWholeColor.SelectedColor.B);
                this.settings.WholeTrackLineWidth = Convert.ToInt32(this.txtTrackWholeWidth.Text);
                this.settings.ShowTraveledTrack = (bool)this.cbShowTravelledTrack.IsChecked;
            }

            //Position marker
            this.settings.ShowPositionMarker = (bool)this.cbEnablePM.IsChecked;
            if ((bool)this.cbEnablePM.IsChecked)
            {
                this.settings.PositionMarkerSize = Convert.ToInt32(this.txtPMSize.Text);
                this.settings.PositionMarkerColor = System.Drawing.Color.FromArgb(this.cpPMColor.SelectedColor.A, this.cpPMColor.SelectedColor.R, this.cpPMColor.SelectedColor.G, this.cpPMColor.SelectedColor.B);
            }

            //Overlay Image
            this.settings.ShowOverlayImage = (bool)this.cbEnableOverlayImage.IsChecked;
            if ((bool)this.cbEnableOverlayImage.IsChecked)
            {
                this.settings.OverlayImagePosition = new System.Drawing.Point(Convert.ToInt32(this.txtOIX.Text), Convert.ToInt32(this.txtOIY.Text));
            }

            //Elevation widget - not included in the UI so giving it the hardcoded data
            //settings.ShowElevationWidget = false;
            this.settings.ElevationWidgetPosition = new System.Drawing.Point(300, 0);
            this.settings.ElevationWidgetFont = new System.Drawing.Font("Arial", 28);
            this.settings.ElevationWidgetColor = System.Drawing.Color.White;

            //Map
            this.settings.ShowMap = (bool)this.cbEnableMap.IsChecked;
            if ((bool)this.cbEnableMap.IsChecked)
            {
                //this.settings.MapHeight = Convert.ToInt32(this.txtMapHeight.Text);
                this.settings.MapOpacity = (float)this.dudMapOpacity.Value;
            }

            //Distance
            this.settings.ShowDistanceWidget = (bool)this.cbEnableDistance.IsChecked;
            if ((bool)this.cbEnableDistance.IsChecked)
            {
                this.settings.DistanceWidgetPosition = new System.Drawing.Point(Convert.ToInt32(this.txtDistanceX.Text), Convert.ToInt32(this.txtDistanceY.Text));
                this.settings.DistanceWidgetFont = new System.Drawing.Font(this.cbDistanceFont.SelectedValue.ToString(), Convert.ToInt32(this.txtDistanceFontSize.Text));
                this.settings.DistanceWidgetColor = System.Drawing.Color.FromArgb(this.cpDistanceColor.SelectedColor.A, this.cpDistanceColor.SelectedColor.R, this.cpDistanceColor.SelectedColor.G, this.cpDistanceColor.SelectedColor.B);
            }

            //Speed
            if ((bool)this.cbEnableSpeed.IsChecked)
            {
                this.settings.ShowSpeedWidget = (bool)this.cbEnableSpeed.IsChecked;
                this.settings.SpeedWidgetPosition = new System.Drawing.Point(Convert.ToInt32(this.txtSpeedX.Text), Convert.ToInt32(this.txtSpeedY.Text));
                this.settings.SpeedWidgetFont = new System.Drawing.Font(this.cbSpeedFont.SelectedValue.ToString(), Convert.ToInt32(this.txtSpeedFontSize.Text));
                this.settings.SpeedWidgetColor = System.Drawing.Color.FromArgb(this.cpSpeedColor.SelectedColor.A, this.cpSpeedColor.SelectedColor.R, this.cpSpeedColor.SelectedColor.G, this.cpSpeedColor.SelectedColor.B);
            }
        }

        #endregion

        #region HoverEffects

        public void btnWidgets_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Widgets_black.png", this.btnWidgets);
        }

        public void btnWidgets_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Widgets_white.png", this.btnWidgets);
        }

        public void btnFiles_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Files_black.png", this.btnFiles);
        }

        public void btnFiles_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Files_white.png", this.btnFiles);
        }

        private void btnTrack_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Track_black.png", this.btnTrack);
        }

        private void btnTrack_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Track_white.png", this.btnTrack);
        }

        private void btnSpeed_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Speed_black.png", this.btnSpeed);
        }

        private void btnSpeed_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Speed_white.png", this.btnSpeed);
        }

        private void btnDistance_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Distance_black.png", this.btnDistance);
        }

        private void btnDistance_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Distance_white.png", this.btnDistance);
        }

        private void btnPositionMarker_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Position-Marker_black.png", this.btnPositionMarker);
        }

        private void btnPositionMarker_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Position-Marker_white.png", this.btnPositionMarker);
        }

        private void btnOverlayImage_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Overlay_Image_black.png", this.btnOverlayImage);
        }

        private void btnOverlayImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Overlay_Image_white.png", this.btnOverlayImage);
        }

        private void btnMap_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Map_black.png", this.btnMap);
        }

        private void btnMap_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Map_white.png", this.btnMap);
        }

        private void btnLoadVideo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/iconVideo_enter.png", this.btnLoadVideo);
        }

        private void btnLoadVideo_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/iconVideo_leave.png", this.btnLoadVideo);
        }

        private void btnLoadGPX_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/iconGPX_enter.png", this.btnLoadGPX);
        }

        private void btnLoadGPX_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/iconGPX_leave.png", this.btnLoadGPX);
        }

        private void btnSync_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_enter.png", this.btnSync);
        }

        private void btnSync_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_leave.png", this.btnSync);
        }

        #endregion
        #region ComboEffects
        private void cmbEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoFormats format = (VideoFormats)cmbEncoding.SelectedIndex;
            ProjectSettings.GetSettings().Format = format;
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
            this.loadFileDialog.Filter = "JPG Files (.jpg)|*.jpg|(.png)|*.png";
            this.loadFileDialog.FilterIndex = 1;
            this.loadFileDialog.Multiselect = false;
            this.loadFileDialog.ShowDialog();
            if (this.loadFileDialog != null)
            {
                ProjectSettings.GetSettings().OverlayImageFile = this.loadFileDialog.FileName;
                this.loadFileDialog.FileName = null;
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

        private void cbEnableOI_Checked(object sender, RoutedEventArgs e)
        {
            this.grdEnabledOI.Visibility = Visibility.Visible;
        }

        private void cbEnableOI_Uncheck(object sender, RoutedEventArgs e)
        {
            this.grdEnabledOI.Visibility = Visibility.Hidden;
        }

        private void btnOutputFileName_Click(object sender, RoutedEventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.Filter = "AVI Files (.avi)|*.avi";
            this.saveFileDialog.FilterIndex = 1;
            this.saveFileDialog.ShowDialog();
            if (this.saveFileDialog != null)
            {
                this.svOutputDir.Visibility = Visibility.Visible;
                this.txtOutputDir.Text = this.saveFileDialog.FileName;
            }
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.Filter = "XML (.XML)|*.xml";
            this.saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(this.saveFileDialog.FileName))
            {
                this.PassSettingsDown(); //TODO check weird color names
            }

            this.settings.Serialize(this.saveFileDialog.FileName);
        }

        private void btnLoadSettings_Click(object sender, RoutedEventArgs e)
        {
            this.loadFileDialog = new OpenFileDialog();
            this.loadFileDialog.Filter = "XML (.XMl)|*.xml";
            this.loadFileDialog.Multiselect = false;
            this.loadFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(this.loadFileDialog.FileName))
            {
                this.settings = this.settings.Deserialize(this.loadFileDialog.FileName);
                this.InitializeSyncronization();
                this.InitializeContent();
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            this.PassSettingsDown();
            this.prevTimeWind = new SetPreviewTime();
            this.prevTimeWind.Show();
        }
    }
        #endregion
}