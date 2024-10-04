using CRMClientApp.Models;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CRMClientApp.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ProjectsControl.xaml
    /// </summary>
    public partial class ProjectsControl : UserControl
    {
        public ProjectsControl()
        {
            InitializeComponent();
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var openProjectButton = (Button)sender;
            var selectedProject = (Project)openProjectButton.DataContext;
            var projectWindow = new ProjectWindow()
            {
                DataContext = selectedProject
            };
            projectWindow.ShowDialog();
        }
    }
}
