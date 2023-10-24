﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>

    public partial class AddTravelWindow : Window
    {

        private List<PackingListItem> _packingList = new();
        private string _purpose;
        private bool startDateSelected = false;
        private bool endDateSelected = false;

        public AddTravelWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }

            for (int i = 1; i < 11; i++)
            {
                cbTravellers.Items.Add(i);
                cbQuantity.Items.Add(i);
            }

            cbPurpose.Items.Add("Work trip");
            cbPurpose.Items.Add("Vacation");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travels = new();
            travels.Show();
            Close();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

            string itemName = txtItemName.Text;

            if (xbTravelDocument.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(itemName))
                {
                    MessageBox.Show("Must enter item name", "Warning");
                    return;
                }
                //Travel document
                bool required = false;
                if (xbRequired.IsChecked == true)
                {
                    required = true;
                }
                TravelDocument travelDocument = new(itemName, required);

                _packingList.Add(travelDocument);

            }
            else
            {
                if (string.IsNullOrWhiteSpace(itemName) || cbQuantity.SelectedIndex == -1)
                {
                    MessageBox.Show("Must enter item name and quantity", "Warning");
                    return;
                }
                //Other item
                int quantity = (int)cbQuantity.SelectedItem;
                OtherItem otherItem = new(itemName, quantity);

                _packingList.Add(otherItem);
            }

            UpdatePackListUI();
        }

        private void UpdatePackListUI()
        {
            lstPackingList.Items.Clear();

            txtItemName.Text = "";
            cbQuantity.SelectedIndex = -1;




            foreach (var item in _packingList)
            {
                ListViewItem lstItem = new();
                lstItem.Tag = item;
                if (item.GetType() == typeof(TravelDocument))
                {
                    TravelDocument travelDocument = (TravelDocument)item;
                    lstItem.Content = travelDocument.GetInfo();
                }
                else
                {
                    OtherItem otherItem = (OtherItem)item;
                    lstItem.Content = otherItem.GetInfo();
                }
                lstPackingList.Items.Add(lstItem);
            }
        }

        private void xbTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            lblQuantity.Visibility = Visibility.Hidden;
            cbQuantity.Visibility = Visibility.Hidden;

            lblRequired.Visibility = Visibility.Visible;
            xbRequired.Visibility = Visibility.Visible;
        }

        private void xbTravelDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            lblQuantity.Visibility = Visibility.Visible;
            cbQuantity.Visibility = Visibility.Visible;

            lblRequired.Visibility = Visibility.Hidden;
            xbRequired.Visibility = Visibility.Hidden;
        }

        private void cbPurpose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cbPurpose.SelectedItem == "Work trip")
            {
                _purpose = "Work trip";
                lblMeetingDetails.Visibility = Visibility.Visible;
                txtMeetingDetails.Visibility = Visibility.Visible;

                lblAllInclusive.Visibility = Visibility.Hidden;
                xbAllinclusive.Visibility = Visibility.Hidden;
            }
            else if ((string)cbPurpose.SelectedItem == "Vacation")
            {
                _purpose = "Vacation";
                lblMeetingDetails.Visibility = Visibility.Hidden;
                txtMeetingDetails.Visibility = Visibility.Hidden;

                //lblAllInclusive.Visibility = Visibility.Visible;
                xbAllinclusive.Visibility = Visibility.Visible;
            }
        }

        private void lstPackingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemoveItem.IsEnabled = true;
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)lstPackingList.SelectedItem;
            PackingListItem selectedPackingListItem = (PackingListItem)selectedItem.Tag;
            _packingList.Remove(selectedPackingListItem);

            UpdatePackListUI();

            btnRemoveItem.IsEnabled = false;
        }

        private void dateStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            startDateSelected = true;
        }

        private void dateEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDateSelected = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Kolla så alla inputs stämmer

            string destination = txtDestination.Text;
            Country country = (Country)cbCountry.SelectedItem;
            int travellers = (int)cbTravellers.SelectedItem;
            string meetingDetails = txtMeetingDetails.Text;
            bool allInclusive = false;
            if (xbAllinclusive.IsChecked == true)
            {
                allInclusive = true;
            }
            DateTime startDate = (DateTime)dateStartDate.SelectedDate!;
            DateTime endDate = (DateTime)dateEndDate.SelectedDate!;

            if (_purpose == "Work trip")
            {
                WorkTrip workTrip = new(destination, country, travellers, _packingList, UserManager.SignedInUser, startDate, endDate, meetingDetails);
                TravelManager.Travels.Add(workTrip);
            }
            else if (_purpose == "Vacation")
            {
                Vacation vacation = new(destination, country, travellers, _packingList, UserManager.SignedInUser, startDate, endDate, allInclusive);
                TravelManager.Travels.Add(vacation);
            }

            MessageBox.Show("Travel added successfully!");
            TravelsWindow travels = new();
            travels.Show();
            Close();
        }
    }
}
