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
            InitializeComponent();
        }
        public string ProgressText
        {
            set
            {
                this.lblProgress.Content = value;
            }
        }

        public event EventHandler Cancel = delegate { };

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel(sender, e);
        }
    }
}
