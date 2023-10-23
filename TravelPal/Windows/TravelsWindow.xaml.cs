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
            lblWelcome.Content = $"Welcome {UserManager.SignedInUser}!";


            foreach (Travel travel in TravelManager.Travels)
            {
                ListViewItem item = new();
                if (travel.GetType() == typeof(WorkTrip))
                {
                    WorkTrip workTrip = (WorkTrip)travel;
                    item.Content = workTrip.GetLstInfo();
                    item.Tag = workTrip;
                    lstTravels.Items.Add(item);
                }
                else
                {
                    Vacation vacation = (Vacation)travel;
                    item.Content = vacation.GetLstInfo();
                    item.Tag = vacation;
                    lstTravels.Items.Add(item);
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
