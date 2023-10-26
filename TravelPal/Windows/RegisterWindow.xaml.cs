using System;
using System.Windows;
using System.Windows.Input;
using TravelPal.Managers;
using TravelPal.Models;

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
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
        }

        //Byta mellan PasswordBox och TextBox
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
            //Warns if the username is taken
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
            try
            {
                string username = txtUsername.Text;

                string password = GetPassword();

                Country country = (Country)cbCountry.SelectedItem;

                User newUser = new(username, password, country);
                UserManager.AddUser(newUser);
                MessageBox.Show("Registered! You can now log in", "Success");
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
            //Inte fått fram för allt valideras innan jag kan klicka på registrera, men för att vara på säkra sidan.
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetPassword()
        {
            string password;
            if (xbShowPassword.IsChecked == true)
            {
                password = txtPassword.Text;
            }
            else
            {
                password = pwPassword.Password.ToString();
            }
            return password;
        }

        private void EnableRegisterButton()
        {
            string username = txtUsername.Text;
            string password = GetPassword();



            //Kollar så att allt är ifyllt korrekt för att kunna klicka på register
            if (username != "" && password != "" && cbCountry.SelectedIndex != -1 && UserManager.ValidateUsername(username) && UserManager.ValidatePassword(password))
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
            WarnPassword();
        }

        private void pwPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableRegisterButton();
            WarnPassword();
        }

        private void cbCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            EnableRegisterButton();
        }

        private void WarnPassword()
        {
            string password = GetPassword();


            if (!UserManager.ValidatePassword(password))
            {
                warnPassword.Visibility = Visibility.Visible;
            }
            else
            {
                warnPassword.Visibility = Visibility.Hidden;
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
