using System;
using System.Windows;

namespace TrackApp.Wpf
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            this.InitializeComponent();
        }

        public event EventHandler Cancel = delegate { };

        public string ProgressText
        {
            set
            {
                this.lblProgress.Content = value;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cancel(sender, e);
        }
    }
}
