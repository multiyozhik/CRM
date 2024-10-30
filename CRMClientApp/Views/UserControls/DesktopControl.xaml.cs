using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
            crmViewModel.OrdersList.Clear();
            foreach (var order in filteredOrdersList)
                crmViewModel.OrdersList.Add(order);            
        }

        private async void YesterdayButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("yesterday");
            crmViewModel.OrdersList.Clear();
            foreach (var order in filteredOrdersList)
                crmViewModel.OrdersList.Add(order);
        }

        private async void WeekButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("week");
            crmViewModel.OrdersList.Clear();
            foreach (var order in filteredOrdersList)
                crmViewModel.OrdersList.Add(order);
        }

        private async void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByPeriod("month");
            crmViewModel.OrdersList.Clear();
            foreach (var order in filteredOrdersList)
                crmViewModel.OrdersList.Add(order);
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var dateStart = (DateTime)crmViewModel.DateStart;
            var dateEnd = (DateTime)crmViewModel.DateEnd;
            var filteredOrdersList = await crmViewModel.CrmClient.FilterOrdersByDateRange(dateStart, dateEnd);
            crmViewModel.OrdersList.Clear();
            foreach (var order in filteredOrdersList)
                crmViewModel.OrdersList.Add(order);
        }
    }
}
