using CRMClientApp.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
