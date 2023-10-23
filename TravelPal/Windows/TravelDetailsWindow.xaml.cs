using System;
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


            txtDestination.Text = TravelManager.SelectedTravel.Destination;
            txtTravellers.Text = TravelManager.SelectedTravel.Travellers.ToString();
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            cbCountry.SelectedItem = TravelManager.SelectedTravel.Country;
            dateStartDate.SelectedDate = TravelManager.SelectedTravel.StartDate;
            dateEndDate.SelectedDate = TravelManager.SelectedTravel.EndDate;
            txtDaysOfTravelling.Text = TravelManager.SelectedTravel.TravelDays.ToString();

            foreach (PackingListItem item in TravelManager.SelectedTravel.PackingList)
            {
                lstPackingList.Items.Add(item);
            }

            if (TravelManager.SelectedTravel.GetType() == typeof(WorkTrip))
            {
                //Kod Välj Work trip i cbReason
                //Enable meeting details
            }
            else
            {
                //Välj vacation i cbReason
                //Enable AllInclusive
            }





        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();
            Close();
        }
    }
}
