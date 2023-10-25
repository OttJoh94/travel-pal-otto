using System.Windows;
using System.Windows.Input;

namespace TravelPal.Windows
{
    /// <summary>
    /// Interaction logic for LogInWindowNewUI.xaml
    /// </summary>
    public partial class LogInWindowNewUI : Window
    {
        public LogInWindowNewUI()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
