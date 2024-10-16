using CRMClientApp.Models;
using System.Windows;

namespace CRMClientApp.Views
{
    public partial class ServiceFormWindow : Window
    {
        public ServiceFormWindow(Service service)
        {
            InitializeComponent();
            DataContext = service;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
