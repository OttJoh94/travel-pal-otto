using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        public TravelsWindow()
        {
            InitializeComponent();
            UpdateUI();
        }


        private void UpdateUI()
        {
            lblWelcome.Content = $"Welcome {UserManager.SignedInUser.Username}!";

            lstUserTravels.Items.Clear();
            lstAdminTravels.Items.Clear();

            //Admin loggar in

            if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                lstAdminTravels.Visibility = Visibility.Visible;

                foreach (Travel travel in TravelManager.Travels)
                {
                    ListViewItem item = new();
                    if (travel.GetType() == typeof(WorkTrip))
                    {
                        WorkTrip workTrip = (WorkTrip)travel;
                        item.Content = workTrip.GetLstInfo();
                        item.Tag = workTrip;
                        lstAdminTravels.Items.Add(item);
                    }
                    else
                    {
                        Vacation vacation = (Vacation)travel;
                        item.Content = vacation.GetLstInfo();
                        item.Tag = vacation;
                        lstAdminTravels.Items.Add(item);
                    }
                }

                btnUserInfo.IsEnabled = false;
            }
            else
            {
                //User loggar in
                lstUserTravels.Visibility = Visibility.Visible;

                //Filtrerar alla travels för att hitta de som tillhör User
                List<Travel> myTravels = new();
                foreach (Travel travel in TravelManager.Travels)
                {
                    if (travel.User.Username == UserManager.SignedInUser.Username)
                    {
                        myTravels.Add(travel);
                    }
                }

                //Lägger till i ListView
                foreach (Travel travel in myTravels)
                {
                    ListViewItem item = new();
                    if (travel.GetType() == typeof(WorkTrip))
                    {
                        WorkTrip workTrip = (WorkTrip)travel;
                        item.Content = workTrip.GetLstInfo();
                        item.Tag = workTrip;
                        lstUserTravels.Items.Add(item);
                    }
                    else
                    {
                        Vacation vacation = (Vacation)travel;
                        item.Content = vacation.GetLstInfo();
                        item.Tag = vacation;
                        lstUserTravels.Items.Add(item);
                    }
                }

            }

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            UserManager.SignedInUser = null;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

        private void lstTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDetails.IsEnabled = true;
            btnRemove.IsEnabled = true;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedItem = new();
            //Väljer rätt lista beroende på userType
            if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                selectedItem = (ListViewItem)lstAdminTravels.SelectedItem;
            }
            else
            {
                selectedItem = (ListViewItem)lstUserTravels.SelectedItem;
            }
            Travel selectedTravel = (Travel)selectedItem.Tag;
            TravelManager.RemoveTravel(selectedTravel);
            MessageBox.Show("Travel removed");

            UpdateUI();
        }
    }
}
