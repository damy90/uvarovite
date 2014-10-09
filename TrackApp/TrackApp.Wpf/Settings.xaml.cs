using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TrackApp.Logic;

namespace TrackApp.Wpf
{
    //render video-commented out-->will be in main window
    //TODO collect data from user
    public partial class Settings : Window
    {
        private const int SpaceBetweenGrids = 5;
        private const int RowHeight = 40;
        private const int ColWidth = 150;
        //float videoStart;
        //float trackStart;
        //bool enableMap;
        //bool enableDistance;
        //bool enableSpeed;
        //VideoFormats videoFormat;
        //string outputName;
        //float fontSize;
        //Color fontColor;
        //Color outlineColor;

        public Settings()
        {
            this.InitializeComponent();
        }

        #region helpingMethods

        private void ResizeGrid(Grid grid)
        {
            if (grid.Height == RowHeight)
            {
                grid.Height = grid.RowDefinitions.Count * RowHeight;
            }
            else
            {
                grid.Height = RowHeight;
            }
        }

        private void AnimationDown(Grid grd)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = grd.Margin;
            ta.To = new Thickness(grd.Margin.Left, 3 * RowHeight + SpaceBetweenGrids, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            grd.BeginAnimation(Grid.MarginProperty, ta);
        }

        private void AnimationUp(Grid grid)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = grid.Margin;
            ta.To = new Thickness(grid.Margin.Left, RowHeight + SpaceBetweenGrids, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            grid.BeginAnimation(Grid.MarginProperty, ta);
        }
        #endregion

        private void btnFont_Click(object sender, RoutedEventArgs e)
        {
            this.grdFont.Visibility = Visibility.Visible;
            this.grdSync.Visibility = Visibility.Hidden;
            this.grdVideo.Visibility = Visibility.Hidden;
            this.grdAdvanced.Visibility = Visibility.Hidden;
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            this.grdVideo.Visibility = Visibility.Visible;
            this.grdSync.Visibility = Visibility.Hidden;
            this.grdFont.Visibility = Visibility.Hidden;
            this.grdAdvanced.Visibility = Visibility.Hidden;
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            this.grdSync.Visibility = Visibility.Visible;
            this.grdVideo.Visibility = Visibility.Hidden;
            this.grdFont.Visibility = Visibility.Hidden;
            this.grdAdvanced.Visibility = Visibility.Hidden;
        }

        private void btnAdvanced_Click(object sender, RoutedEventArgs e)
        {
            this.grdAdvanced.Visibility = Visibility.Visible;
            this.grdVideo.Visibility = Visibility.Hidden;
            this.grdFont.Visibility = Visibility.Hidden;
            this.grdSync.Visibility = Visibility.Hidden;
        }

        private void btnFontForeground_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnFont_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Font_enter.png", this.btnFont);
        }

        private void btnFont_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Font_leave.png", this.btnFont);
        }

        private void btnVideo_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Video_enter.png", this.btnVideo);
        }

        private void btnVideo_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Video_leave.png", this.btnVideo);
        }

        private void btnSync_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_enter.png", this.btnSync);
        }

        private void btnSync_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_leave.png", this.btnSync);
        }

        private void btnAdvanced_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Advanced_enter.png", this.btnAdvanced);
        }

        private void btnAdvanced_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ChangeIconBackground(@"pack://application:,,,/Resources/SB_Advanced_leave.png", this.btnAdvanced);
        }

        private void ChangeIconBackground(string path, System.Windows.Controls.Button btn)
        {
            var imgBrush = new ImageBrush();
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            imgBrush.ImageSource = new BitmapImage(uri);
            btn.Background = imgBrush;
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            //TODO:implement checks for necessary data and collect data
            //VideoCompositor.RenderVideo();
            this.Hide();
        }
    }
}
