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

namespace allinone
{
    /// <summary>
    /// Логика взаимодействия для autorise2.xaml
    /// </summary>
    public partial class autorise2 : Window
    {
        private Random random;

        public autorise2()
        {
            InitializeComponent();
            random = new Random();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rndnum();
        }

        private void rndnum()
        {
            string randomText = random.Next(1000, 10000).ToString();
            paswrd.Text = randomText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Молодец", "Вы зашли", MessageBoxButton.OK);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}