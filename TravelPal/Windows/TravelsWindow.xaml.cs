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
    }
}
