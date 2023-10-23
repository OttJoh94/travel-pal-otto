using System.Windows;
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
    }
}
