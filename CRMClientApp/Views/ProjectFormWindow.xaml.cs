using CRMClientApp.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views
{
    public partial class ProjectFormWindow : Window
    {
        public ProjectFormWindow(Project project)
        {
            InitializeComponent();
            DataContext = project;
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
                ((Project)addPhotoButton.DataContext).Photo = openFileDialog.FileName;
            }
        }
    }
}
