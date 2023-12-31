﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                    AddToListView(travel, lstAdminTravels);

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
                    AddToListView(travel, lstUserTravels);
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

        private void btnDetails_Click(object sender, RoutedEventArgs e)
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
            TravelManager.SelectedTravel = selectedTravel;

            TravelDetailsWindow detailsWindow = new();
            detailsWindow.Show();
            Close();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" 'Add travel' to add a new travel to your list\n 'Details' to show details of a selected travel \n 'Remove' to remove a travel \n 'User' to manage your account \n 'Sign out' to sign out", "Info");
        }

        private void btnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow detailsWindow = new();
            detailsWindow.Show();
            Close();
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new();
            addTravelWindow.Show();
            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void AddToListView(Travel travel, ListView listview)
        {
            ListViewItem item = new();
            if (travel.GetType() == typeof(WorkTrip))
            {
                WorkTrip workTrip = (WorkTrip)travel;
                item.Content = workTrip.GetLstInfo();
                item.Tag = workTrip;
                listview.Items.Add(item);
            }
            else
            {
                Vacation vacation = (Vacation)travel;
                item.Content = vacation.GetLstInfo();
                item.Tag = vacation;
                listview.Items.Add(item);
            }
        }
    }
}
