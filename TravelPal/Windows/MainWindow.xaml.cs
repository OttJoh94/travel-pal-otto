using System.Windows;
using System.Windows.Input;
using TravelPal.Managers;
using TravelPal.Windows;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new();
            registerWindow.Show();
            Close();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();

            if (UserManager.SignInUser(username, password))
            {
                TravelsWindow travelsWindow = new();
                travelsWindow.Show();
                Close();
            }
            else
            {
                warnInvalidUser.Visibility = Visibility.Visible;
            }
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            EnableSignInButton();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableSignInButton();
        }

        private void EnableSignInButton()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();

            if (username != "" && password != "")
            {
                btnSignIn.IsEnabled = true;
            }
            else
            {
                btnSignIn.IsEnabled = false;
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
