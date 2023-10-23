using System.Windows;
using TravelPal.Managers;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            cbCountry.Items.Add("Hej");
        }

        private void xbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            string currentPwPassword = pwPassword.Password.ToString();

            pwPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Text = currentPwPassword;

        }

        private void xbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            string currentTxtPassword = txtPassword.Text;

            txtPassword.Visibility = Visibility.Hidden;
            pwPassword.Visibility = Visibility.Visible;
            pwPassword.Password = currentTxtPassword;
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string username = txtUsername.Text;
            if (!UserManager.ValidateUsername(username))
            {
                warnUsernameTaken.Visibility = Visibility.Visible;
            }
            else
            {
                warnUsernameTaken.Visibility = Visibility.Hidden;
            }

            EnableRegisterButton();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EnableRegisterButton()
        {
            string username = txtUsername.Text;
            string password;
            if (xbShowPassword.IsChecked == true)
            {
                password = txtPassword.Text;
            }
            else
            {
                password = pwPassword.Password.ToString();
            }

            if (username != "" && password != "" && cbCountry.SelectedIndex != -1 && warnUsernameTaken.Visibility != Visibility.Visible)
            {
                btnRegister.IsEnabled = true;
            }
            else
            {
                btnRegister.IsEnabled = false;
            }
        }

        private void txtPassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            EnableRegisterButton();
        }

        private void pwPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableRegisterButton();
        }

        private void cbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            EnableRegisterButton();
        }
    }
}
