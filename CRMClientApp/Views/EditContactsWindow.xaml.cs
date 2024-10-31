using CRMClientApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
                var serverIconPath = await crmViewModel.CrmClient.UploadFile(newLink.IconPath, "image/svg+xml");
                newLink.IconPath = System.IO.Path.GetFileName(serverIconPath);
                await crmViewModel.CrmClient.AddLink(newLink);
                newLink.IconPath = crmViewModel.CrmClient.GetUrlFileName(newLink.IconPath);
                crmViewModel.SocialMediaLinksList.Add(newLink);

                //т.е. из datacontext получаем путь к файлу иконки в клиенте, 
                //загружаем по этому клиентскому пути файл иконки на сервер и возвр. сформир. путь к иконке на сервере,
                //отсекаем только само серверное имя файла иконки и добавляем эту соц.ссылку в БД, 
                //для отображения добавления - формируем Url-адрес к файлу иконки и добавляем соц.ссылку в observable-коллекцию
            }
        }
        private async void DeleteLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (SocialMediaLinkVM)button.DataContext;
            link.IconPath = System.IO.Path.GetFileName(link.IconPath);
            var crmViewModel = (CRMViewModel)DataContext;
            await crmViewModel.CrmClient.DeleteLink(link.IconPath);
            await crmViewModel.CrmClient.DeleteFile(link.IconPath);
            crmViewModel.SocialMediaLinksList.Remove(link); 
        }
    }
}
