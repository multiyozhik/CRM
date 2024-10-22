using CRMClientApp.ViewModels;
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
    /// <summary>
    /// Логика взаимодействия для LinkFormWindow.xaml
    /// </summary>
    public partial class LinkFormWindow : Window
    {
        public LinkFormWindow()
        {
            InitializeComponent();
        }

        private void AddIconButton_Click(object sender, RoutedEventArgs e)
        {
            var addIconButton = (Button)sender;
            var openFileDialog = new OpenFileDialog();
            bool? openFileDialogResult = openFileDialog.ShowDialog();
            if (openFileDialogResult == true)
            {
                ((SocialMediaLinkVM)addIconButton.DataContext).Icon = openFileDialog.FileName;
            }
        }

        private void SaveLinkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
