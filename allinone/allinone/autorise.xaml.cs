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
    /// Логика взаимодействия для autorise.xaml
    /// </summary>
    public partial class autorise : Window
    {
        public autorise()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = log.Text.Trim();
            string passw = pass.Text.Trim();
       
            User user = DB.Context.User
                .Where(u => u.Login == login && u.Password == passw)
                .FirstOrDefault();

            if (user != null)
            {
                CurrentUser.Id = user.Id;
                CurrentUser.Login = user.Login;
                CurrentUser.IdRole = user.IdRole ?? 3;
                CurrentUser.RoleTitle = user.Role?.Title ?? "Пользователь";

                MessageBox.Show($"Здраствуй\nТвоя роль: {CurrentUser.RoleTitle}",
                    "Успешный успех", MessageBoxButton.OK, MessageBoxImage.Information);

                autorise2 autorise2 = new autorise2();
                autorise2.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Логин или пароль не верны", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void log_GotFocus(object sender, RoutedEventArgs e)
        {
            if (log.Text == "Логин")
            {
                log.Text = "";
            }
        }

        private void pass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pass.Text == "Пароль")
            {
                pass.Text = "";
            }
        }

        private void pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pass.Text))
            {
                pass.Text = "Пароль";
            }
        }

        private void log_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(log.Text))
            {
                log.Text = "Логин";
            }
        }
    }
}