using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace TrackApp.Wpf
{ 
    //render video-commented out-->will be in main window
    //TODO collect data from user
    public partial class Settings : Window
    {
        const int spaceBetweenGrids = 5;
        const int rowHeight = 40;
        const int colWidth = 150;
        float videoStart;
        float trackStart;
        bool enableMap;
        bool enableDistance;
        bool enableSpeed;
        VideoFormats videoFormat;
        string outputName;
        float fontSize;
        Color fontColor;
        Color outlineColor;

        public Settings()
        {
            InitializeComponent();
        }

        #region helpingMethods
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
            ta.To = new Thickness(grd.Margin.Left, (3*rowHeight+spaceBetweenGrids), 0, 0);
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
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Font_enter.png", btnFont);
        }
        private void btnFont_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Font_leave.png", btnFont);
        }

        private void btnVideo_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Video_enter.png", btnVideo);
        }

        private void btnVideo_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Video_leave.png", btnVideo);
        }

        private void btnSync_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_enter.png", btnSync);
        }

        private void btnSync_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Syncronization_leave.png", btnSync);
        }

        private void btnAdvanced_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Advanced_enter.png", btnAdvanced);
        }

        private void btnAdvanced_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeIconBackground(@"pack://application:,,,/Resources/SB_Advanced_leave.png", btnAdvanced);
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
