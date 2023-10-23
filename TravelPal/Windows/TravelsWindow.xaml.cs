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
            foreach (Travel travel in TravelManager.Travels)
            {
                ListViewItem item = new();
                item.Content = travel.GetInfo();
                lstTravels.Items.Add(item);

            }
        }
    }
}
