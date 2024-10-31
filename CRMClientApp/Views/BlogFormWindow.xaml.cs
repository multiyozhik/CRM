using CRMClientApp.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views
{
    public partial class BlogFormWindow : Window
    {
        public BlogFormWindow(Blog blog)
        {
            InitializeComponent();
            DataContext = blog;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private async void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var addPhotoButton = (Button)sender;
            var openFileDialog = new OpenFileDialog();
            bool? openFileDialogResult = openFileDialog.ShowDialog();
            if (openFileDialogResult == true)
            {
                ((Blog)addPhotoButton.DataContext).Photo = openFileDialog.FileName;
            }
        }
    }
}
