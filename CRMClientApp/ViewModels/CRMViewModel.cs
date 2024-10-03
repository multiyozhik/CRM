using CRMClientApp.Commands;
using CRMClientApp.Models;
using CRMClientApp.Services;
using CRMClientApp.Views;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;

namespace CRMClientApp.ViewModels
{
    public class CRMViewModel : INotifyPropertyChanged
    {
        private readonly CRMClient crmClient;
        public CRMViewModel(CRMClient crmClient)
        {
            this.crmClient = crmClient;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public OrderVM AddingOrder { get; } = new OrderVM();

        private AsyncRelayCommand? addOrderCommand;     

        public AsyncRelayCommand? AddOrderCommand
        {
            get => addOrderCommand ??= new AsyncRelayCommand(
                async obj => await crmClient.AddOrder(AddingOrder));
        }

        private Project? selectedProject;
        public Project? SelectedProject 
        { 
            get => selectedProject;
            set
            {
                selectedProject = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProject)));
                if (selectedProject is not null)
                {
                    var projectWindow = new ProjectWindow()
                    {
                        DataContext = SelectedProject
                    };
                    Dispatcher.CurrentDispatcher.Invoke(projectWindow.ShowDialog);
                }
            }
        }

        private ObservableCollection<Project>? projectsList;
        public ObservableCollection<Project>? ProjectsList
        {
            get => projectsList;
            set
            {
                projectsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(projectsList)));
            }
        }
    }
}
