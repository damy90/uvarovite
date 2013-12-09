using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
namespace TrackApp
{
    //TODO collect data from user
    public partial class MainWindow : Window
    {
        OpenFileDialog loadFileDialog;
        SaveFileDialog saveFileDialog;
        ProjectSettings settings;
        SetPreviewTime prevTimeWind;
        Preview prevWindow;
        public MainWindow()
        {
            InitializeComponent();
            loadFileDialog = new OpenFileDialog();
            settings = ProjectSettings.GetSettings();
            IniializeVisibilities();
            InitializeContent();
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
            //Video Settings
            this.cmbEncoding.SelectedIndex = (int)settings.Format;
            this.cmbSpeed.SelectedIndex = settings.VideoSpeed - 1;
            this.cmbOutputQuality.SelectedIndex = settings.VideoQuality - 1;
            //Track
            this.cbEnableTrack.IsChecked = settings.ShowTrack;
            this.txtTrackX.Text = settings.TrackPostion.X.ToString();
            this.txtTrackY.Text = settings.TrackPostion.Y.ToString();
            this.txtTrackHeight.Text = settings.TrackHeight.ToString();
            this.cpTrackTravelledColor.SelectedColor = Color.FromArgb(settings.TraveledTrackColor.A, settings.TraveledTrackColor.R, settings.TraveledTrackColor.G, settings.TraveledTrackColor.B);
            this.txtTrackTravelledWidth.Text = settings.TraveledTrackLineWidth.ToString();
            this.cpTrackWholeColor.SelectedColor = Color.FromArgb(settings.WholeTrackColor.A, settings.WholeTrackColor.R, settings.WholeTrackColor.G, settings.WholeTrackColor.B);
            this.txtTrackWholeWidth.Text = settings.WholeTrackLineWidth.ToString();
            this.cbShowTravelledTrack.IsChecked = settings.ShowTraveledTrack;
            //Position marker
            this.cbEnablePM.IsChecked = settings.ShowPositionMarker;
            this.txtPMSize.Text = settings.PositionMarkerSize.ToString();
            this.cpPMColor.SelectedColor = Color.FromArgb(settings.PositionMarkerColor.A, settings.PositionMarkerColor.R, settings.PositionMarkerColor.G, settings.PositionMarkerColor.B);
            //Overlay Image
            this.cbEnableOverlayImage.IsChecked = settings.ShowOverlayImage;
            this.txtOIX.Text = settings.overlayImagePosition.X.ToString();
            this.txtOIY.Text = settings.overlayImagePosition.Y.ToString();
            //Map
            this.cbEnableMap.IsChecked = settings.ShowMap;
            this.txtMapHeight.Text = settings.MapHeight.ToString();
            this.dudMapOpacity.Value = settings.MapOpacity;
            //Distance
            this.cbEnableDistance.IsChecked = settings.ShowDistanceWidget;
            this.txtDistanceX.Text = settings.DistanceWidgetPosition.X.ToString();
            this.txtDistanceY.Text = settings.DistanceWidgetPosition.Y.ToString();
            this.cbDistanceFont.SelectedValue = new FontFamily(settings.PDistanceWidgetFont.FontFamily);
            this.txtDistanceFontSize.Value = settings.PDistanceWidgetFont.Size;
            this.cpDistanceColor.SelectedColor = Color.FromArgb(settings.DistanceWidgetColor.A, settings.DistanceWidgetColor.R, settings.DistanceWidgetColor.G, settings.DistanceWidgetColor.B);
            //Speed
            this.cbEnableSpeed.IsChecked = settings.ShowSpeedWidget;
            this.txtSpeedX.Text = settings.SpeedWidgetPosition.X.ToString();
            this.txtSpeedY.Text = settings.SpeedWidgetPosition.Y.ToString();
            this.cbSpeedFont.SelectedValue = new FontFamily(settings.PSpeedWidgetFont.FontFamily);
            this.txtSpeedFontSize.Value = (int)settings.PSpeedWidgetFont.Size;
            this.cpSpeedColor.SelectedColor = Color.FromArgb(settings.SpeedWidgetColor.A, settings.SpeedWidgetColor.R, settings.SpeedWidgetColor.G, settings.SpeedWidgetColor.B);
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
            else if (settings.Format == null)
            {
                return false;
            }
            return true;
        }
        private void InitializeSyncronization()
        {
            //Syncronization
            this.udTrackHours.Value = (int)settings.TrackStart / 3600;
            this.udTrackMinutes.Value = (int)(settings.TrackStart - this.udTrackHours.Value * 3600) / 60;
            this.udTrackSeconds.Value = (int)(settings.TrackStart - this.udTrackHours.Value * 3600 - this.udTrackMinutes.Value * 60);
            this.udTrackEndHours.Value = (int)settings.TrackEnd / 3600;
            this.udTrackEndMinutes.Value = (int)(settings.TrackEnd - this.udTrackEndHours.Value * 3600) / 60;
            this.udTrackEndSeconds.Value = (int)(settings.TrackEnd - this.udTrackEndHours.Value * 3600 - this.udTrackEndMinutes.Value * 60);
            this.udVideoStHours.Value = (int)settings.VideoStart / 3600;
            this.udVideoStMinutes.Value = (int)(settings.VideoStart - this.udVideoStHours.Value * 3600) / 60;
            this.udVideoStSeconds.Value = (int)(settings.VideoStart - this.udVideoStHours.Value * 3600 - this.udVideoStMinutes.Value * 60);
            this.udVideoEndHours.Value = (int)settings.VideoEnd / 3600;
            this.udVideoEndMinutes.Value = (int)(settings.VideoEnd - this.udVideoEndHours.Value * 3600) / 60;
            this.udVideoEndSeconds.Value = (int)(settings.VideoEnd - this.udVideoEndHours.Value * 3600 - this.udVideoEndMinutes.Value * 60);
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
                txtTrackDir.Text = loadFileDialog.FileName;
                ProjectSettings.GetSettings().GPXPath = loadFileDialog.FileName;
                svTrackDir.Visibility = Visibility.Visible;
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
                txtInputVideoDir.Text = loadFileDialog.FileName;
                ProjectSettings.GetSettings().VideoInputPath = loadFileDialog.FileName;
                svInputVideoDir.Visibility = Visibility.Visible;
                loadFileDialog.FileName = null;
            }
        }
        BackgroundWorker worker;
        public ProgressWindow pd;
        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            //TODO:add validations
            //settings.Serialize();
            //settings.VideoEnd = 300;
            //settings = settings.Deserialize();
            PassSettingsDown();
            try
            {
                //pd.ShowDialog();
                VideoCompositor.RenderVideo();
                MessageBox.Show("Success!");
                //comment this if you don't want to start the video immediately after the rendering
                Process.Start(settings.VideoOutputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PassSettingsDown()
        {
            //TODO:add validations
            //settings.Serialize();
            //settings.VideoEnd = 300;
            //settings = settings.Deserialize();
            settings.VideoOutputPath = this.txtOutputDir.Text;
            settings.TrackStart = Convert.ToInt32(udTrackHours.Value) * 3600f + Convert.ToInt32(udTrackMinutes.Value) * 60 + Convert.ToInt32(udTrackSeconds.Value);
            settings.TrackEnd = Convert.ToInt32(udTrackEndHours.Value) * 3600f + Convert.ToInt32(udTrackEndMinutes.Value) * 60 + Convert.ToInt32(udTrackEndSeconds.Value);
            settings.VideoStart = Convert.ToInt32(udVideoStHours.Value) * 3600f + Convert.ToInt32(udVideoStMinutes.Value) * 60 + Convert.ToInt32(udVideoStSeconds.Value);
            settings.VideoEnd = Convert.ToInt32(udVideoEndHours.Value) * 3600f + Convert.ToInt32(udVideoEndMinutes.Value) * 60 + Convert.ToInt32(udVideoEndSeconds.Value);
            settings.VideoSpeed = this.cmbSpeed.SelectedIndex + 1;
            settings.VideoQuality = this.cmbOutputQuality.SelectedIndex + 1;
            //settings.Format = (VideoFormats)this.cmbEncoding.SelectedIndex; //not tested
            //settings.BitRate = 20000000;
            //Track
            settings.ShowTrack = (bool)this.cbEnableTrack.IsChecked;
            if ((bool)this.cbEnableTrack.IsChecked)
            {
                settings.TrackPostion = new System.Drawing.Point(Convert.ToInt32(this.txtTrackX.Text), Convert.ToInt32(this.txtTrackY.Text));
                settings.TrackHeight = Convert.ToInt32(this.txtTrackHeight.Text);
                settings.TraveledTrackColor = System.Drawing.Color.FromArgb(this.cpTrackTravelledColor.SelectedColor.A, this.cpTrackTravelledColor.SelectedColor.R, this.cpTrackTravelledColor.SelectedColor.G, this.cpTrackTravelledColor.SelectedColor.B);
                settings.TraveledTrackLineWidth = Convert.ToInt32(this.txtTrackTravelledWidth.Text);
                settings.WholeTrackColor = System.Drawing.Color.FromArgb(this.cpTrackWholeColor.SelectedColor.A, this.cpTrackWholeColor.SelectedColor.R, this.cpTrackWholeColor.SelectedColor.G, this.cpTrackWholeColor.SelectedColor.B);
                settings.WholeTrackLineWidth = Convert.ToInt32(this.txtTrackWholeWidth.Text);
                settings.ShowTraveledTrack = (bool)this.cbShowTravelledTrack.IsChecked;
            }
            //Position marker
            settings.ShowPositionMarker = (bool)this.cbEnablePM.IsChecked;
            if ((bool)this.cbEnablePM.IsChecked)
            {
                settings.PositionMarkerSize = Convert.ToInt32(this.txtPMSize.Text);
                settings.PositionMarkerColor = System.Drawing.Color.FromArgb(this.cpPMColor.SelectedColor.A, this.cpPMColor.SelectedColor.R, this.cpPMColor.SelectedColor.G, this.cpPMColor.SelectedColor.B);
            }
            //Overlay Image
            settings.ShowOverlayImage = (bool)this.cbEnableOverlayImage.IsChecked;
            if ((bool)this.cbEnableOverlayImage.IsChecked)
            {
                settings.overlayImagePosition = new System.Drawing.Point(Convert.ToInt32(this.txtOIX.Text), Convert.ToInt32(this.txtOIY.Text));
            }
            //Elevation widget - not included in the UI so giving it the hardcoded data
            //settings.ShowElevationWidget = false;
            settings.ElevationWidgetPosition = new System.Drawing.Point(300, 0);
            settings.ElevationWidgetFont = new System.Drawing.Font("Arial", 28);
            settings.ElevationWidgetColor = System.Drawing.Color.White;
            //Map
            settings.ShowMap = (bool)this.cbEnableMap.IsChecked;
            if ((bool)this.cbEnableMap.IsChecked)
            {
                settings.MapHeight = Convert.ToInt32(this.txtMapHeight.Text);
                settings.MapOpacity = (float)this.dudMapOpacity.Value;
            }
            //Distance
            settings.ShowDistanceWidget = (bool)this.cbEnableDistance.IsChecked;
            if ((bool)this.cbEnableDistance.IsChecked)
            {
                settings.DistanceWidgetPosition = new System.Drawing.Point(Convert.ToInt32(this.txtDistanceX.Text), Convert.ToInt32(this.txtDistanceY.Text));
                settings.DistanceWidgetFont = new System.Drawing.Font(this.cbDistanceFont.SelectedValue.ToString(), Convert.ToInt32(this.txtDistanceFontSize.Text));
                settings.DistanceWidgetColor = System.Drawing.Color.FromArgb(this.cpDistanceColor.SelectedColor.A, this.cpDistanceColor.SelectedColor.R, this.cpDistanceColor.SelectedColor.G, this.cpDistanceColor.SelectedColor.B);
            }
            //Speed
            if ((bool)this.cbEnableSpeed.IsChecked)
            {
                settings.ShowSpeedWidget = (bool)this.cbEnableSpeed.IsChecked;
                settings.SpeedWidgetPosition = new System.Drawing.Point(Convert.ToInt32(this.txtSpeedX.Text), Convert.ToInt32(this.txtSpeedY.Text));
                settings.SpeedWidgetFont = new System.Drawing.Font(this.cbSpeedFont.SelectedValue.ToString(), Convert.ToInt32(this.txtSpeedFontSize.Text));
                settings.SpeedWidgetColor = System.Drawing.Color.FromArgb(this.cpSpeedColor.SelectedColor.A, this.cpSpeedColor.SelectedColor.R, this.cpSpeedColor.SelectedColor.G, this.cpSpeedColor.SelectedColor.B);
            }
            }

        #endregion

        #region HoverEffects
        public void btnWidgets_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Widgets_black.png", btnWidgets);
        }
        public void btnWidgets_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Widgets_white.png", btnWidgets);
        }
        public void btnFiles_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Files_black.png", btnFiles);
        }
        public void btnFiles_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Files_white.png", btnFiles);
        }
        private void btnTrack_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Track_black.png", btnTrack);
        }
        private void btnTrack_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Track_white.png", btnTrack);
        }
        private void btnSpeed_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Speed_black.png", btnSpeed);
        }
        private void btnSpeed_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Speed_white.png", btnSpeed);
        }
        private void btnDistance_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Distance_black.png", btnDistance);
        }
        private void btnDistance_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Distance_white.png", btnDistance);
        }
        private void btnPositionMarker_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Position-Marker_black.png", btnPositionMarker);
        }
        private void btnPositionMarker_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Position-Marker_white.png", btnPositionMarker);
        }
        private void btnOverlayImage_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Overlay_Image_black.png", btnOverlayImage);
        }
        private void btnOverlayImage_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Overlay_Image_white.png", btnOverlayImage);
        }
        private void btnMap_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Map_black.png", btnMap);
        }
        private void btnMap_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Map_white.png", btnMap);
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
            loadFileDialog.Filter = "JPG Files (.jpg)|*.jpg|(.png)|*.png";
            loadFileDialog.FilterIndex = 1;
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            if (loadFileDialog != null)
            {
                ProjectSettings.GetSettings().overlayImageFile = loadFileDialog.FileName;
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
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AVI Files (.avi)|*.avi";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.ShowDialog();
            if (saveFileDialog != null)
            {
                svOutputDir.Visibility = Visibility.Visible;
                txtOutputDir.Text = saveFileDialog.FileName;
            }
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML (.XML)|*.xml";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != String.Empty && saveFileDialog.FileName != null)
                PassSettingsDown();//TODO check weird color names
                settings.Serialize(saveFileDialog.FileName);
            }

        private void btnLoadSettings_Click(object sender, RoutedEventArgs e)
        {
            loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "XML (.XMl)|*.xml";
            loadFileDialog.Multiselect = false;
            loadFileDialog.ShowDialog();
            if (loadFileDialog.FileName != String.Empty && loadFileDialog.FileName != null)
            {
                settings = settings.Deserialize(loadFileDialog.FileName);
                InitializeSyncronization();
                InitializeContent();
            }
        }
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            PassSettingsDown();
            this.prevTimeWind = new SetPreviewTime();
            this.prevTimeWind.Show();
        }
  
    }
        #endregion
}