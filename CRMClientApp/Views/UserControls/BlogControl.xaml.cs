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

        private void OpenBlogButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var openBlogButton = (Button)sender;
            var selectedBlog = (Blog)openBlogButton.DataContext;
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
                newBlog.Photo = await crmViewModel.CrmClient.UploadFile(newBlog.Photo, "image/png");
                newBlog.Id = await crmViewModel.CrmClient.AddBlog(newBlog);
                newBlog.Photo = crmViewModel.CrmClient.GetUrlFileName(newBlog.Photo);
                crmViewModel.BlogsList.Add(newBlog);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var blog = (Blog)button.DataContext;
            var blogFormWindow = new BlogFormWindow(blog);
            var blogFormWindowResult = blogFormWindow.ShowDialog();
            if (blogFormWindowResult == true)
            {
                var crmViewModel = (CRMViewModel)DataContext;
                blog.Photo = await crmViewModel.CrmClient.UploadFile(blog.Photo, "image/png");
                await crmViewModel.CrmClient.EditBlog(blog);
                var displayedBlog = blog;
                displayedBlog.Photo = crmViewModel.CrmClient.GetUrlFileName(blog.Photo);
                int indexOfBlog = crmViewModel.BlogsList.IndexOf(blog);
                crmViewModel.BlogsList.Remove(blog);
                crmViewModel.BlogsList.Insert(indexOfBlog, displayedBlog);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var blog = (Blog)button.DataContext;
            var crmViewModel = (CRMViewModel)DataContext;
            await crmViewModel.CrmClient.DeleteBlog(blog.Id);
            await crmViewModel.CrmClient.DeleteFile(System.IO.Path.GetFileName(blog.Photo));
            crmViewModel.BlogsList.Remove(blog);
        }
    }
}
