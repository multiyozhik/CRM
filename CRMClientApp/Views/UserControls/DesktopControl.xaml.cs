using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMClientApp.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DesktopControl.xaml
    /// </summary>
    public partial class DesktopControl : UserControl
    {
        public DesktopControl()
        {
            InitializeComponent();
        }

        private async void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("today");
            crmViewModel.OrdersList = new ObservableCollection<Order>(filteredOrdersList);
        }

        private async void YesterdayButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("yesterday");
            crmViewModel.OrdersList = new ObservableCollection<Order>(filteredOrdersList);
        }

        private async void WeekButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("week");
            crmViewModel.OrdersList = new ObservableCollection<Order>(filteredOrdersList);
        }

        private async void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("month");
            crmViewModel.OrdersList = new ObservableCollection<Order>(filteredOrdersList);
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var dateStart = (DateTime)crmViewModel.DateStart;
            var dateEnd = (DateTime)crmViewModel.DateEnd;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByDateRange(dateStart, dateEnd);
            crmViewModel.OrdersList = new ObservableCollection<Order>(filteredOrdersList);
        }
    }
}
