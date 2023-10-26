using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        private List<PackingListItem> _packingList = new();

        private bool _isEditing = false;
        public TravelDetailsWindow()
        {
            InitializeComponent();

            //För att skapa en egen kopia av listan så det inte sparas förrän man klickar save
            foreach (var item in TravelManager.SelectedTravel.PackingList)
            {
                _packingList.Add(item);
            }
            cbReason.Items.Add("Work trip");
            cbReason.Items.Add("Vacation");

            for (int i = 1; i <= 10; i++)
            {
                cbTravellers.Items.Add(i);
                cbQuantity.Items.Add(i);
            }

            UpdateUI();
            UpdatePackingList();
            TurnOffUI();


        }

        private void UpdatePackingList()
        {
            lstPackingList.Items.Clear();

            foreach (PackingListItem item in _packingList)
            {
                ListViewItem listViewItem = new();
                if (item.GetType() == typeof(TravelDocument))
                {
                    TravelDocument document = (TravelDocument)item;
                    listViewItem.Tag = document;
                    listViewItem.Content = document.GetInfo();
                }
                else
                {
                    OtherItem otherItem = (OtherItem)item;
                    listViewItem.Tag = otherItem;
                    listViewItem.Content = otherItem.GetInfo();
                }
                lstPackingList.Items.Add(listViewItem);

            }
        }

        private void UpdateUI()
        {
            Travel selectedTravel = TravelManager.SelectedTravel;

            //Fyller i all info från resan.
            txtDestination.Text = selectedTravel.Destination;
            cbTravellers.SelectedItem = selectedTravel.Travellers;
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            cbCountry.SelectedItem = selectedTravel.Country;
            dateStartDate.SelectedDate = selectedTravel.StartDate;
            dateEndDate.SelectedDate = selectedTravel.EndDate;
            txtDaysOfTravelling.Text = selectedTravel.TravelDays.ToString();


            if (selectedTravel.GetType() == typeof(WorkTrip))
            {
                WorkTrip workTrip = (WorkTrip)selectedTravel;
                //Enable meeting details
                cbReason.SelectedItem = "Work trip";
                lblMeetingDetails.Visibility = Visibility.Visible;
                txtMeetingDetails.Visibility = Visibility.Visible;
                txtMeetingDetails.Text = workTrip.MeetingDetails;

            }
            else
            {
                Vacation vacation = (Vacation)selectedTravel;
                //Välj vacation i cbReason
                //Enable AllInclusive
                cbReason.SelectedItem = "Vacation";
                lblAllInclusive.Visibility = Visibility.Visible;
                xbAllInclusive.Visibility = Visibility.Visible;
                if (vacation.AllInclusive)
                {
                    xbAllInclusive.IsChecked = true;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();
            Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            TurnOnUI();

        }

        private void TurnOnUI()
        {
            //Byt knapp
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;

            //Gör så man kan ändra i rutorna
            txtDestination.IsReadOnly = false;
            cbCountry.IsEnabled = true;
            cbTravellers.IsEnabled = true;
            cbReason.IsEnabled = true;
            txtMeetingDetails.IsReadOnly = false;
            xbAllInclusive.IsEnabled = true;
            dateStartDate.IsEnabled = true;
            dateEndDate.IsEnabled = true;
            btnAddItem.IsEnabled = true;
            txtItemName.IsEnabled = true;
            cbQuantity.IsEnabled = true;
            xbTravelDocument.IsEnabled = true;

            _isEditing = true;
        }

        private void TurnOffUI()
        {
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;

            txtDestination.IsReadOnly = true;
            cbCountry.IsEnabled = false;
            cbTravellers.IsEnabled = false;
            cbReason.IsEnabled = false;
            txtMeetingDetails.IsReadOnly = true;
            xbAllInclusive.IsEnabled = false;
            dateStartDate.IsEnabled = false;
            dateEndDate.IsEnabled = false;
            txtDaysOfTravelling.IsReadOnly = true;
            btnAddItem.IsEnabled = false;
            txtItemName.IsEnabled = false;
            cbQuantity.IsEnabled = false;
            xbTravelDocument.IsEnabled = false;

            _isEditing = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            string destination = txtDestination.Text;
            Country country = (Country)cbCountry.SelectedItem;
            int travellers = (int)cbTravellers.SelectedItem;
            DateTime startDate = (DateTime)dateStartDate.SelectedDate;
            DateTime endDate = (DateTime)dateEndDate.SelectedDate;

            //nullCheck
            if (destination == "" || travellers == null || country == null || startDate == null || endDate == null)
            {
                MessageBox.Show("Invalid info, check inputs", "Warning");
                return;
            }

            try
            {
                if ((string)cbReason.SelectedItem == "Work trip")
                {

                    string meetingDetails = txtMeetingDetails.Text;
                    WorkTrip updatedTravel = new(destination, country, travellers, _packingList, TravelManager.SelectedTravel.User, startDate, endDate, meetingDetails);
                    TravelManager.UpdateTravel(updatedTravel);
                }
                else if ((string)cbReason.SelectedItem == "Vacation")
                {
                    bool allInclusive = false;
                    if (xbAllInclusive.IsChecked == true)
                    {
                        allInclusive = true;
                    }
                    Vacation updatedTravel = new(destination, country, travellers, _packingList, TravelManager.SelectedTravel.User, startDate, endDate, allInclusive);
                    TravelManager.UpdateTravel(updatedTravel);

                }


                MessageBox.Show("Travel updated!");

                UpdateUI();
                TurnOffUI();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}, try again", "Warning");
            }

        }

        private void cbReason_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbReason.SelectedItem == "Work trip")
            {
                lblMeetingDetails.Visibility = Visibility.Visible;
                lblAllInclusive.Visibility = Visibility.Hidden;

                txtMeetingDetails.Visibility = Visibility.Visible;
                xbAllInclusive.Visibility = Visibility.Hidden;
            }
            else if (cbReason.SelectedItem == "Vacation")
            {
                lblMeetingDetails.Visibility = Visibility.Hidden;
                lblAllInclusive.Visibility = Visibility.Visible;

                txtMeetingDetails.Visibility = Visibility.Hidden;
                xbAllInclusive.Visibility = Visibility.Visible;
            }
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

            UpdatePackingList();
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)lstPackingList.SelectedItem;
            PackingListItem selectedPackingListItem = (PackingListItem)selectedItem.Tag;
            if (TravelManager.TryingToRemovePassport(selectedPackingListItem))
            {
                return;
            }
            _packingList.Remove(selectedPackingListItem);

            UpdatePackingList();

            btnRemoveItem.IsEnabled = false;
        }

        private void lstPackingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isEditing)
            {
                btnRemoveItem.IsEnabled = true;
            }
        }

        private void xbTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            lblQuantity.Visibility = Visibility.Hidden;
            cbQuantity.Visibility = Visibility.Hidden;

            //lblRequired.Visibility = Visibility.Visible;
            xbRequired.Visibility = Visibility.Visible;
        }

        private void xbTravelDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            lblQuantity.Visibility = Visibility.Visible;
            cbQuantity.Visibility = Visibility.Visible;

            lblRequired.Visibility = Visibility.Hidden;
            xbRequired.Visibility = Visibility.Hidden;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Here you can view details about your travel\nPress 'Edit' to unlock the boxes and edit the info\nDon't forget to save!", "Help");
        }

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddPassportToItems();
        }

        private void AddPassportToItems()
        {
            //Remove previous passport
            PackingListItem? previousPassport = _packingList.FirstOrDefault(item => item.Name == "Passport");
            if (previousPassport != null)
            {
                _packingList.Remove(previousPassport);
            }

            //Nullcheck
            if (cbCountry.SelectedItem == null)
            {
                return;
            }

            IUser user = TravelManager.SelectedTravel.User!;


            if (TravelManager.PassportRequired(user, (Country)cbCountry.SelectedItem))
            {
                TravelDocument travelDocument = new("Passport", true);
                _packingList.Add(travelDocument);
            }
            else
            {
                TravelDocument travelDocument = new("Passport", false);
                _packingList.Add(travelDocument);
            }

            UpdatePackingList();
        }
    }
}
