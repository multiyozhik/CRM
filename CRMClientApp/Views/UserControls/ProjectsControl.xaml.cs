using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CRMClientApp.Views.UserControls
{
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newProject = new Project();
            var projectFormWindow = new ProjectFormWindow(newProject);
            var projectFormWindowResult = projectFormWindow.ShowDialog();
            if (projectFormWindowResult == true)
            {
                var crmViewModel = (CRMViewModel)((Button)sender).DataContext;
                await crmViewModel.CrmClient.UploadFile(newProject.Photo);
                await crmViewModel.CrmClient.AddProject(newProject);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var editedProject = (Project)((Button)sender).DataContext;
            var projectFormWindow = new ProjectFormWindow(editedProject);
            var projectFormWindowResult = projectFormWindow.ShowDialog();
            if (projectFormWindowResult == true)
            {
                await crmViewModel.CrmClient.EditProject(editedProject);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var crmViewModel = (CRMViewModel)DataContext;
            var deletedProject = (Project)((Button)sender).DataContext;
            crmViewModel.CrmClient.DeleteProject(deletedProject);
        }
    }
}
