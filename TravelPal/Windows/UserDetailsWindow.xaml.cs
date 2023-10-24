using System;
using System.Windows;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private User _currentUser;
        public UserDetailsWindow()
        {
            InitializeComponent();
            _currentUser = (User)UserManager.SignedInUser;

            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            cbCountry.SelectedItem = _currentUser.Location;
            txtUsername.Text = _currentUser.Username;
        }

        private void xbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            string newPassword = pwNewPassword.Password.ToString();
            string confirmPassword = pwConfirmPassword.Password.ToString();

            txtNewPassword.Text = newPassword;
            txtConfirmPassword.Text = confirmPassword;

            pwNewPassword.Visibility = Visibility.Hidden;
            pwConfirmPassword.Visibility = Visibility.Hidden;

            txtNewPassword.Visibility = Visibility.Visible;
            txtConfirmPassword.Visibility = Visibility.Visible;
        }

        private void xbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            pwNewPassword.Password = newPassword;
            pwConfirmPassword.Password = confirmPassword;

            pwNewPassword.Visibility = Visibility.Visible;
            pwConfirmPassword.Visibility = Visibility.Visible;

            txtNewPassword.Visibility = Visibility.Hidden;
            txtConfirmPassword.Visibility = Visibility.Hidden;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = txtUsername.Text;
            string newPassword;
            Country newLocation = (Country)cbCountry.SelectedItem;
            //Get the correct password
            if (xbShowPassword.IsChecked == true)
            {
                newPassword = txtNewPassword.Text;
            }
            else
            {
                newPassword = pwNewPassword.Password.ToString();
            }


            bool usernameUpdated = false;
            bool passwordUpdated = false;
            bool locationUpdated = false;

            //Update username
            if (newUsername != _currentUser.Username)
            {
                if (UserManager.UpdateUsername(_currentUser, newUsername))
                {
                    usernameUpdated = true;
                    warnUsernameTaken.Visibility = Visibility.Hidden;
                }
                else
                {
                    warnUsernameTaken.Visibility = Visibility.Visible;
                }

            }

            //Update password
            if (_currentUser.Password != newPassword && CheckPasswordMatch() && UserManager.UpdatePassword(_currentUser, newPassword))
            {
                passwordUpdated = true;
                warnPasswordShort.Visibility = Visibility.Hidden;
            }
            else if (newPassword != "" && !UserManager.UpdatePassword(_currentUser, newPassword))
            {
                warnPasswordShort.Visibility = Visibility.Visible;
            }
            else
            {
                warnPasswordShort.Visibility = Visibility.Hidden;
            }

            //Update location
            if (_currentUser.Location != newLocation)
            {
                _currentUser.Location = newLocation;
                locationUpdated = true;
            }

            //Show the correct messageBox
            if (usernameUpdated && passwordUpdated && locationUpdated)
            {
                MessageBox.Show("Username, password and location updated!");
            }
            else if (usernameUpdated && passwordUpdated && !locationUpdated)
            {
                MessageBox.Show("Username and password updated!");
            }
            else if (!usernameUpdated && passwordUpdated && locationUpdated)
            {
                MessageBox.Show("Password and location updated!");
            }
            else if (usernameUpdated && !passwordUpdated && locationUpdated)
            {
                MessageBox.Show("Username and location updated!");
            }
            else if (usernameUpdated && !passwordUpdated && !locationUpdated)
            {
                MessageBox.Show("Username updated!");
            }
            else if (!usernameUpdated && passwordUpdated && !locationUpdated)
            {
                MessageBox.Show("Password updated!");
            }
            else if (!usernameUpdated && !passwordUpdated && locationUpdated)
            {
                MessageBox.Show("Location updated!");
            }

        }

        private void NewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswordMatch();
        }

        private bool CheckPasswordMatch()
        {
            if (pwNewPassword.Password.ToString() != pwConfirmPassword.Password.ToString() || txtNewPassword.Text != txtConfirmPassword.Text)
            {
                warnPasswordNotMatching.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                warnPasswordNotMatching.Visibility = Visibility.Hidden;
                return true;
            }
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswordMatch();
        }
    }
}
