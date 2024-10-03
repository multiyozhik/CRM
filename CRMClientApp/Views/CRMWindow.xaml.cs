using CRMClientApp.Services;
using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CRMClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class CRMWindow : Window
    {
        private readonly Uri baseAddress = new("https://localhost:7033");
        private readonly CRMClient crmClient;
        public CRMWindow()
        {
            InitializeComponent();
            crmClient = new CRMClient(baseAddress);
            DataContext = new CRMViewModel(crmClient);
            Loaded += CRMWindow_Loaded;
        }

        private async void CRMWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var crmWindow = (Window)sender;
            var crmViewModel = (CRMViewModel)crmWindow.DataContext;
            crmViewModel.ProjectsList = new ObservableCollection<Project>(
                await crmClient.GetProjectsList()?? new List<Project>());
        }
    }
}
