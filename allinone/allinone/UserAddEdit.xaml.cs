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
    public partial class UserAddEdit : Window
    {
        bool isAdd = false;
        User user;
        string currentUserRole;
        int currentUserId;

        public UserAddEdit(User user)
        {
            InitializeComponent();
            currentUserRole = CurrentUser.RoleTitle;
            currentUserId = CurrentUser.Id;

            if (user == null)
            {
                title.Text = "Добавить пользователя";
                this.user = new User();
                isAdd = true;
            }
            else
            {
                title.Text = "Изменить пользователя";
                this.user = user;
                isAdd = false;
            }

            LoadUsers();

            if (!isAdd)
            {
                loginTextBox.Text = this.user.Login;
                passTextBox.Text = this.user.Password;
                pass2TextBox.Text = this.user.Password;
                ccc.SelectedValue = this.user.IdRole;

                if (currentUserRole == "Модератор" && user.Id == currentUserId)
                {
                    loginTextBox.IsEnabled = false;
                }
            }
        }

        public void LoadUsers()
        {
            var allRoles = DB.Context.Role.ToList();

            if (currentUserRole == "Модератор" || string.IsNullOrEmpty(currentUserRole))
            {
                var userRole = allRoles.FirstOrDefault(r => r.Title == "Пользователь");
                ccc.ItemsSource = userRole != null ? new List<Role> { userRole } : new List<Role>();
                ccc.SelectedValue = userRole?.Id;
                ccc.IsEnabled = false;
            }
            else if (currentUserRole == "Пользователь")
            {
                if (!isAdd && user.Id == currentUserId)
                {
                    var currentRole = allRoles.FirstOrDefault(r => r.Id == user.IdRole);
                    ccc.ItemsSource = currentRole != null ? new List<Role> { currentRole } : new List<Role>();
                    ccc.SelectedValue = currentRole?.Id;
                    ccc.IsEnabled = false;
                }
                else if (isAdd)
                {
                    var userRole = allRoles.FirstOrDefault(r => r.Title == "Пользователь");
                    ccc.ItemsSource = userRole != null ? new List<Role> { userRole } : new List<Role>();
                    ccc.SelectedValue = userRole?.Id;
                }
                else
                {
                    ccc.ItemsSource = allRoles;
                }
            }
            else
            {
                ccc.ItemsSource = allRoles;
            }

            ccc.DisplayMemberPath = "Title";
            ccc.SelectedValuePath = "Id";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginTextBox.Text))
                {
                    MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(passTextBox.Text))
                {
                    MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (passTextBox.Text != pass2TextBox.Text)
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (ccc.SelectedValue == null)
                {
                    MessageBox.Show("Выберите роль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                user.Login = loginTextBox.Text.Trim();
                user.Password = passTextBox.Text;

                bool canChangeRole = !(currentUserRole == "Модератор" && !isAdd) &&
                                   !(currentUserRole == "Пользователь" && !isAdd && user.Id == currentUserId);

                if (canChangeRole)
                {
                    user.IdRole = (int)ccc.SelectedValue;
                }
                if (isAdd)
                {
                    DB.Context.User.Add(user);
                }
                else
                {
                    var exuse = DB.Context.User.Find(user.Id);
                    if (exuse != null)
                    {
                        if (!(currentUserRole == "Модератор" && user.Id == currentUserId))
                        {
                            exuse.Login = user.Login;
                        }
                        exuse.Password = user.Password;

                        if (canChangeRole)
                        {
                            exuse.IdRole = user.IdRole;
                        }
                    }
                }

                DB.Context.SaveChanges();
                MessageBox.Show("Успешный успех", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}