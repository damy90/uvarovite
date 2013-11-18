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
    /// Interaction logic for Settings.xaml
    /// </summary>
    /// TODO:  1. Инициализираме в класа всички полета, които ще трябва да предадем надолу по бекенда(на бекенда трябва да е създаден преди това widget)
    ///        2. Вмъкване на всички контроли
    ///        3. ОК бутон - при него трябва да има валидация дали всички полета са попълнени (например txtMyTextBlock.Text == (null||String.Empty))
    ///        4. След попълване на тези контроли в зависимост от резултатът им ще променим и вида на MainWindow
    ///        
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        public void Enable_Distance_Widget(object sender, RoutedEventArgs e)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
