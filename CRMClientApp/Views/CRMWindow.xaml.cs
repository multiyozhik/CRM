﻿using CRMClientApp.Models;
using CRMClientApp.Services;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace CRMClientApp.Views
{
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

            crmViewModel.ContactsValues = await crmClient.GetContactsValues()?? new ContactsValuesViewModel();
            crmViewModel.FieldValues = await crmClient.GetFieldValues();
            crmViewModel.TotalOrdersCount = await crmClient.GetTotalOrdersListCount();
            var ordersList = await crmClient.GetOrdersList() ?? new List<OrderVM>(); 
            foreach (var order in ordersList)
            {
                crmViewModel.OrdersList.Add(order);
            }  
            crmViewModel.SocialMediaLinksList = new ObservableCollection<SocialMediaLinkVM>(
                await crmClient.GetSocialMediaLinks());
            crmViewModel.ProjectsList = new ObservableCollection<Project>(
                await crmClient.GetProjectsList()?? new List<Project>());
            crmViewModel.ServicesList = new ObservableCollection<Service>(
                await crmClient.GetServicesList() ?? new List<Service>());
            crmViewModel.BlogsList = new ObservableCollection<Blog>(
                await crmClient.GetBlogsList() ?? new List<Blog>());
        }
    }
}
