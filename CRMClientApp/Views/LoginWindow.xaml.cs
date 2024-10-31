using CRMClientApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginVM = ((Button)sender).DataContext as LoginViewModel;
            loginVM.Password = PasswordBox.Password;   //остальные поля формы просто биндятся, а пароль скрываем
            DialogResult = true;
            Close();
        }
    }
}
