﻿using CRMClientApp.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace CRMClientApp.Views.UserControls
{
    public partial class ContactsControl : UserControl
    {
        public ContactsControl()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var link = (SocialMediaLinkVM)btn.DataContext;
            var uri = link.HyperlinkUri;
            var startProcess = new ProcessStartInfo() { 
                FileName = "explorer.exe",
                Arguments = uri };
            Process.Start(startProcess);
        }

        private void EditContactsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var editContactsWindow = new EditContactsWindow()
            {
                DataContext = crmViewModel
            };
            editContactsWindow.ShowDialog();
        }
    }
}
