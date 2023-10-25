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

            if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                lblUser.Visibility = Visibility.Visible;
                cbUser.Visibility = Visibility.Visible;
            }


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


            foreach (IUser user in UserManager.Users)
            {
                ComboBoxItem item = new();
                item.Tag = user;
                item.Content = user.Username;
                cbUser.Items.Add(item);

                if (user == UserManager.SignedInUser)
                {
                    cbUser.SelectedItem = item;
                }
            }

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

            if (dateEndDate.SelectedDate == null)
            {
                dateEndDate.SelectedDate = dateStartDate.SelectedDate;
            }
        }

        private void dateEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDateSelected = true;

            if (dateEndDate.SelectedDate < dateStartDate.SelectedDate)
            {
                MessageBox.Show("End date must be after start date", "Error");
                dateEndDate.SelectedDate = dateStartDate.SelectedDate;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Kolla så alla inputs stämmer

            if (txtDestination.Text == "" || cbCountry.SelectedItem == null || cbTravellers.SelectedItem == null || cbPurpose.SelectedItem == null || !startDateSelected || !endDateSelected)
            {
                TurnOffAllWarnings();
                //Visa var det saknas
                ShowWarningsWhereNeeded();
                return;
            }

            try
            {
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

                ComboBoxItem selectedUser = (ComboBoxItem)cbUser.SelectedItem;
                IUser user = (IUser)selectedUser.Tag;


                if (_purpose == "Work trip")
                {
                    WorkTrip workTrip = new(destination, country, travellers, _packingList, user, startDate, endDate, meetingDetails);
                    TravelManager.Travels.Add(workTrip);
                }
                else if (_purpose == "Vacation")
                {
                    Vacation vacation = new(destination, country, travellers, _packingList, user, startDate, endDate, allInclusive);
                    TravelManager.Travels.Add(vacation);
                }

                MessageBox.Show("Travel added successfully!");
                TravelsWindow travels = new();
                travels.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ShowWarningsWhereNeeded()
        {
            if (txtDestination.Text == "")
            {
                warnDestination.Visibility = Visibility.Visible;
            }
            if (cbCountry.SelectedItem == null)
            {
                warnCountry.Visibility = Visibility.Visible;
            }
            if (cbTravellers.SelectedItem == null)
            {
                warnTravellers.Visibility = Visibility.Visible;
            }
            if (cbPurpose.SelectedItem == null)
            {
                warnPurpose.Visibility = Visibility.Visible;
            }
            if (!startDateSelected)
            {
                warnStartDate.Visibility = Visibility.Visible;
            }
            if (!endDateSelected)
            {
                warnEndDate.Visibility = Visibility.Visible;
            }
        }

        private void TurnOffAllWarnings()
        {
            warnDestination.Visibility = Visibility.Hidden;
            warnCountry.Visibility = Visibility.Hidden;
            warnTravellers.Visibility = Visibility.Hidden;
            warnPurpose.Visibility = Visibility.Hidden;
            warnStartDate.Visibility = Visibility.Hidden;
            warnEndDate.Visibility = Visibility.Hidden;
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

            ComboBoxItem selectedUser = (ComboBoxItem)cbUser.SelectedItem;
            IUser user = (IUser)selectedUser.Tag;


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

            UpdatePackListUI();
        }

        private void cbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddPassportToItems();
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
