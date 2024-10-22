using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для BlogControl.xaml
    /// </summary>
    public partial class BlogControl : UserControl
    {
        public BlogControl()
        {
            InitializeComponent();
        }

        private void BlogCardButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var blogCardButton = (Button)sender;
            var selectedBlog = (Blog)blogCardButton.DataContext;
            var blogWindow = new BlogWindow() 
            { 
                DataContext = selectedBlog 
            };
            blogWindow.ShowDialog();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newBlog = new Blog();
            var blogFormWindow = new BlogFormWindow(newBlog);
            var blogFormWindowResult = blogFormWindow.ShowDialog();
            if (blogFormWindowResult == true)
            {
                var crmViewModel = (CRMViewModel)((Button)sender).DataContext;
                newBlog.Photo = await crmViewModel.CrmClient.UploadFile(newBlog.Photo);
                await crmViewModel.CrmClient.AddBlog(newBlog);
                newBlog.Photo = crmViewModel.CrmClient.GetPhotoUrl(newBlog.Photo);
                crmViewModel.BlogsList.Add(newBlog);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var editedBlog = (Blog)((Button)sender).DataContext;
            var blogFormWindow = new BlogFormWindow(editedBlog);
            var blogFormWindowResult = blogFormWindow.ShowDialog();
            if (blogFormWindowResult == true)
            {
                await crmViewModel.CrmClient.EditBlog(editedBlog);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var deletedBlog = (Blog)((Button)sender).DataContext;
            crmViewModel.CrmClient.DeleteBlog(deletedBlog);
        }
    }
}
