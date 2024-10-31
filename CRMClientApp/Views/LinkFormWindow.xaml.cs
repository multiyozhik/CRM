using CRMClientApp.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace CRMClientApp.Views
{
    public partial class LinkFormWindow : Window
    {
        public LinkFormWindow(SocialMediaLinkVM link)
        {
            InitializeComponent();
            DataContext = link;
        }

        private void AddIconButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            bool? openFileDialogResult = openFileDialog.ShowDialog();
            if (openFileDialogResult == true)
            {
                ((SocialMediaLinkVM)DataContext).IconPath = openFileDialog.FileName;                
            }
        }

        private void SaveLinkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
