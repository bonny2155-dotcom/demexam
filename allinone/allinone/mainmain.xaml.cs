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
    /// Логика взаимодействия для mainmain.xaml
    /// </summary>
    public partial class mainmain : Window
    {
     
        public mainmain()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            autorise autorise = new autorise();
            autorise.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UserAddEdit userAddEdit = new UserAddEdit(null);
            userAddEdit.Show();
         
        }
    }
}
