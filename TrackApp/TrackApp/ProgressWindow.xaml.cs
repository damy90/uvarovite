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

namespace TrackApp
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
