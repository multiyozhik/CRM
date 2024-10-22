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
using System.Windows.Shapes;

namespace CRMClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EditContactsWindow.xaml
    /// </summary>
    public partial class EditContactsWindow : Window
    {
        public EditContactsWindow()
        {
            InitializeComponent();
        }

        private void AddLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var linkFormWindow = new LinkFormWindow()
            {
                DataContext = new SocialMediaLinkVM()
            };
            var linkFormWindowResult = linkFormWindow.ShowDialog();
            if (linkFormWindowResult is true)
            {
                var newLink = (SocialMediaLinkVM)linkFormWindow.DataContext;
                var crmViewModel = (CRMViewModel)DataContext;
                var links = crmViewModel.SocialMediaLinksList;

                //crmViewModel.CrmClient.AddLink(newLink);
                //links.Add(newLink);
            }
        }
        private void DeleteLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (SocialMediaLinkVM)button.DataContext;
            var crmViewModel = (CRMViewModel)DataContext;
            var links = crmViewModel.SocialMediaLinksList;
            crmViewModel.CrmClient.DeleteLink(link.Icon);
            links.Remove(link);
        }
    }
}
