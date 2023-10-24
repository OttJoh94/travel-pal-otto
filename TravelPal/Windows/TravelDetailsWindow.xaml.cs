using System;
using System.Collections.Generic;
using System.Windows;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        public TravelDetailsWindow()
        {
            InitializeComponent();

            cbReason.Items.Add("Work trip");
            cbReason.Items.Add("Vacation");

            UpdateUI();

        }

        private void UpdateUI()
        {
            Travel selectedTravel = TravelManager.SelectedTravel;

            txtDestination.Text = selectedTravel.Destination;
            txtTravellers.Text = selectedTravel.Travellers.ToString();
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            cbCountry.SelectedItem = selectedTravel.Country;
            dateStartDate.SelectedDate = selectedTravel.StartDate;
            dateEndDate.SelectedDate = selectedTravel.EndDate;
            txtDaysOfTravelling.Text = selectedTravel.TravelDays.ToString();

            foreach (PackingListItem item in selectedTravel.PackingList)
            {
                lstPackingList.Items.Add(item);
            }

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
            txtTravellers.IsReadOnly = false;
            cbReason.IsEnabled = true;
            txtMeetingDetails.IsReadOnly = false;
            xbAllInclusive.IsEnabled = true;
            dateStartDate.IsEnabled = true;
            dateEndDate.IsEnabled = true;
        }

        private void TurnOffUI()
        {
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;

            txtDestination.IsReadOnly = true;
            cbCountry.IsEnabled = false;
            txtTravellers.IsReadOnly = true;
            cbReason.IsEnabled = false;
            txtMeetingDetails.IsReadOnly = true;
            xbAllInclusive.IsEnabled = false;
            dateStartDate.IsEnabled = false;
            dateEndDate.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            string destination = txtDestination.Text;
            Country country = (Country)cbCountry.SelectedItem;
            int.TryParse(txtTravellers.Text, out int travellers);
            DateTime startDate = (DateTime)dateStartDate.SelectedDate;
            DateTime endDate = (DateTime)dateEndDate.SelectedDate;

            if (destination == "" || travellers == 0 || country == null || startDate == null || endDate == null)
            {
                MessageBox.Show("Invalid info, check inputs", "Warning");
                return;
            }

            try
            {
                if ((string)cbReason.SelectedItem == "Work trip")
                {
                    //TODO: Något måste göras om sortens resa ändras
                    string meetingDetails = txtMeetingDetails.Text;
                    WorkTrip updatedTravel = new(destination, country, travellers, new List<PackingListItem>(), TravelManager.SelectedTravel.User, startDate, endDate, meetingDetails);
                    TravelManager.UpdateTravel(updatedTravel);
                }
                else if ((string)cbReason.SelectedItem == "Vacation")
                {
                    bool allInclusive = false;
                    if (xbAllInclusive.IsChecked == true)
                    {
                        allInclusive = true;
                    }
                    Vacation updatedTravel = new(destination, country, travellers, new List<PackingListItem>(), TravelManager.SelectedTravel.User, startDate, endDate, allInclusive);
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
            if (cbReason.SelectedItem == "Work Trip")
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
    }
}
