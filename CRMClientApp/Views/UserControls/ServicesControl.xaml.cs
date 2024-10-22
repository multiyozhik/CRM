using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views.UserControls
{
    public partial class ServicesControl : UserControl
    {
        public ServicesControl()
        {
            InitializeComponent();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newService = new Service();
            var serviceFormWindow = new ServiceFormWindow(newService);
            var serviceFormWindowResult = serviceFormWindow.ShowDialog();
            if (serviceFormWindowResult == true)
            {
                var crmViewModel = (CRMViewModel)((Button)sender).DataContext;
                await crmViewModel.CrmClient.AddService(newService);
                crmViewModel.ServicesList.Add(newService);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var editedService = (Service)((Button)sender).DataContext;
            var serviceFormWindow = new ServiceFormWindow(editedService);
            var serviceFormWindowResult = serviceFormWindow.ShowDialog();
            if (serviceFormWindowResult == true)
            {
                await crmViewModel.CrmClient.EditService(editedService);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var deletedService = (Service)((Button)sender).DataContext;
            crmViewModel.CrmClient.DeleteService(deletedService);
        }
    }
}
