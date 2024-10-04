using CRMClientApp.Models;
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
    }
}
