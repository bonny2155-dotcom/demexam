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
    public partial class MainWindow : Window
    {
        List<User> users;
        List<String> f;

        public MainWindow()
        {
            InitializeComponent();
            Roleperm();
            loadUsers();
            f = new List<String>() { "Логин", "Роль" };
            cmbf.ItemsSource = f;
            getCountRoles();
        }

        private void Roleperm()
        {
            switch (CurrentUser.IdRole)
            {
                case 1:
                    admperm();
                    break;
                case 2:
                    modperm();
                    break;
                case 3:
                    useperm();
                    break;
            }
        }

        private void admperm()
        {
            butadclc.Visibility = Visibility.Visible;
        }

        private void modperm()
        {
            butadclc.Visibility = Visibility.Visible;
        }

        private void useperm()
        {
            butadclc.Visibility = Visibility.Collapsed;
            filtal.Visibility = Visibility.Collapsed;
            stvis.Visibility = Visibility.Collapsed;
        }

        public void loadUsers()
        {
            switch (CurrentUser.IdRole)
            {
                case 1:
                    users = DB.Context.User.ToList();
                    break;
                case 2:
                    users = DB.Context.User.Where(u => u.IdRole == 3 || u.Id == CurrentUser.Id).ToList();
                    break;
                case 3:
                    users = DB.Context.User.Where(u => u.Id == CurrentUser.Id).ToList();
                    break;
            }
            use.ItemsSource = users;
        }

        private void Button_ClickCh(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as User;

            if (!CanModifyUser(user))
            {
                MessageBox.Show("У вас недостаточно прав для редактирования этого пользователя",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new UserAddEdit(user);
            editWindow.Closed += (s, args) =>
            {
                loadUsers();
                getCountRoles();
            };
            editWindow.ShowDialog();
        }

        private void Button_ClickDel(object sender, RoutedEventArgs e)
        {
            var selectedUser = use.SelectedItem as User;
            if (selectedUser?.Id == CurrentUser.Id)
            {
                MessageBox.Show("Вы не можете удалить свой собственный аккаунт",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!CanModifyUser(selectedUser))
            {
                MessageBox.Show("нет прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.Login}?",
                "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var userToDelete = DB.Context.User.Find(selectedUser.Id);
                    if (userToDelete != null)
                    {
                        DB.Context.User.Remove(userToDelete);
                        DB.Context.SaveChanges();
                        loadUsers();
                        getCountRoles();
                        MessageBox.Show("Пользователь удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanModifyUser(User targetUser)
        {
            if (targetUser == null) return false;

            switch (CurrentUser.IdRole)
            {
                case 1:
                    return true;
                case 2:
                    return targetUser.IdRole == 3 || targetUser.Id == CurrentUser.Id;
                case 3:
                    return targetUser.Id == CurrentUser.Id;
                default:
                    return false;
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new UserAddEdit(null);
            addWindow.Closed += (s, args) =>
            {
                loadUsers();
                getCountRoles();
            };
            addWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbf.SelectedIndex == 0)
            {
                users = rbu.IsChecked == true ? users.OrderBy(u => u.Login).ToList() :
                    users.OrderByDescending(u => u.Login).ToList();
            }
            else if (cmbf.SelectedIndex == 1)
            {
                users = rbu.IsChecked == true ? users.OrderBy(u => u.IdRole).ToList() :
                    users.OrderByDescending(u => u.IdRole).ToList();
            }
            use.ItemsSource = users;
        }

        private void filtwch(object sender, RoutedEventArgs e)
        {
            var filteredUsers = users.Where(u => u.Login.StartsWith(filtw.Text)).ToList();
            use.ItemsSource = filteredUsers;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (chf.IsChecked == true)
                stvis.Visibility = Visibility.Visible;
            else
            {
                stvis.Visibility = Visibility.Collapsed;
                loadUsers();
            }
        }

        private void getCountRoles()
        {
            if (CurrentUser.IdRole == 3)
            {
                lblCountOfUsers.Text = $"Ваша роль: {CurrentUser.RoleTitle}";
                return;
            }

            List<Counting_users_role_Result> counts = DB.Context.Counting_users_role().ToList();
            string str = "Количество пользователей\n";
            foreach (var item in counts)
            {
                str += item.Hasвание_pоли + " : " + item.Kоличество_пользователей + "\n";
            }
            lblCountOfUsers.Text = str;
        }
    }
}