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
                newProject.Photo = await crmViewModel.CrmClient.UploadFile(newProject.Photo);
                await crmViewModel.CrmClient.AddProject(newProject);
                
                newProject.Photo = crmViewModel.CrmClient.GetPhotoUrl(newProject.Photo);
                crmViewModel.ProjectsList.Add(newProject);
                //т.е. вначале записываю в БД само имя файла (будет сам находить на сервере в wwwroot/img),
                //а потом формирую правильный адрес для клиента для отображения, т.е.
                //надо сделать localhost/img/filename, аналогично методу в CRMClient GetProjectsList()
                //и вручную доб. новый проект, чтоб Observable коллекция изменилась и ChangedProperty сработало
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        { 
            var button = (Button)sender;
            var project = (Project)button.DataContext;            

            var projectFormWindow = new ProjectFormWindow(project);
            var projectFormWindowResult = projectFormWindow.ShowDialog();
            if (projectFormWindowResult == true)
            {
                var crmViewModel = (CRMViewModel)DataContext;
                project.Photo = await crmViewModel.CrmClient.UploadFile(project.Photo);

                await crmViewModel.CrmClient.EditProject(project);
                var displayedProject = project;
                displayedProject.Photo = crmViewModel.CrmClient.GetPhotoUrl(project.Photo);
                int indexOfProject = crmViewModel.ProjectsList.IndexOf(project);
                //crmViewModel.ProjectsList[indexOfProject] = displayedProject;
                crmViewModel.ProjectsList.Remove(project);
                crmViewModel.ProjectsList.Insert(indexOfProject, displayedProject);
                
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var project = (Project)button.DataContext;
            var crmViewModel = (CRMViewModel)DataContext;
            crmViewModel.CrmClient.DeleteProject(project);
            crmViewModel.ProjectsList.Remove(project);
        }
    }
}
