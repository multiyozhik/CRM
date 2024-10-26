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
    public partial class EditContactsWindow : Window
    {
        public EditContactsWindow()
        {
            InitializeComponent();
        }

        private async void AddLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var newLink = new SocialMediaLinkVM();
            var linkFormWindow = new LinkFormWindow(newLink);
            var linkFormWindowResult = linkFormWindow.ShowDialog();
            if (linkFormWindowResult is true)
            {
                newLink = (SocialMediaLinkVM)linkFormWindow.DataContext;
                var crmViewModel = (CRMViewModel)DataContext;
                var serverIconPath = await crmViewModel.CrmClient.UploadFile(
                    System.IO.Path.GetFileName(newLink.Icon), 
                    "image/svg+xml"); 
                newLink.Icon = System.IO.Path.GetFileName(serverIconPath);   
                await crmViewModel.CrmClient.AddLink(newLink);
                newLink.Icon = crmViewModel.CrmClient.GetUrlFileName(newLink.Icon);
                crmViewModel.SocialMediaLinksList.Add(newLink);

                //т.е. из datacontext получаем путь к файлу иконки в клиенте, отсекаем само имя файла,
                //загружаем файл иконки на сервер и возвр. сформир. путь к иконке на сервере,
                //отсекаем только само серверное имя файла иконки и добавляем эту соц.ссылку в БД, 
                //для отображения добавления - формируем Url-адрес к файлу иконки и добавляем соц.ссылку в observable-коллекцию
            }
        }
        private async void DeleteLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (SocialMediaLinkVM)button.DataContext;
            var crmViewModel = (CRMViewModel)DataContext;
            await crmViewModel.CrmClient.DeleteLink(link.Icon);
            await crmViewModel.CrmClient.DeleteFile(link.Icon);
            crmViewModel.SocialMediaLinksList.Remove(link); 
        }
    }
}
