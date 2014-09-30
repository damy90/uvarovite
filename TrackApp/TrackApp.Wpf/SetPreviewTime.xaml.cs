using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TrackApp.Logic;

namespace TrackApp.Wpf
{
    /// <summary>
    /// Interaction logic for SetPreviewTime.xaml
    /// </summary>
    public partial class SetPreviewTime : Window
    {
        private float time;
        public float Time
        {
            get
            {
                return this.time;
            }
        }

        public SetPreviewTime()
        {
            InitializeComponent();
        }

        private void btnPreview_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnPreview.Background = new SolidColorBrush(Colors.White);
            this.btnPreview.Foreground = new SolidColorBrush(Color.FromRgb(30,60,255));
        }
        private void btnPreview_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnPreview.Background = new SolidColorBrush(Color.FromRgb(30, 60, 255));
            this.btnPreview.Foreground = new SolidColorBrush(Colors.White);
        }
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (udHours.Value != null && udMinutes.Value != null && udSeconds.Value != null)
            {
                this.time = Convert.ToInt32(udHours.Value) * 3600f + Convert.ToInt32(udMinutes.Value) * 60 + Convert.ToInt32(udSeconds.Value);
            }
            try
            {
                Preview prevWindow = new Preview(VideoCompositor.Preview(this.time));
                prevWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
