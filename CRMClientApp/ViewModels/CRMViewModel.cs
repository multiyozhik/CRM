﻿using CRMClientApp.Commands;
using CRMClientApp.Models;
using CRMClientApp.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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

        private Service? selectedService;
        public Service? SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedService)));
            }
        }

        private ObservableCollection<Service>? servicesList;
        public ObservableCollection<Service>? ServicesList
        {
            get => servicesList;
            set
            {
                servicesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServicesList)));
            }
        }

        private Blog? selectedBlog;
        public Blog? SelectedBlog
        {
            get => selectedBlog;
            set
            {
                selectedBlog = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBlog)));
            }
        }
        private ObservableCollection<Blog>? blogsList;
        public ObservableCollection<Blog>? BlogsList
        {
            get => blogsList;
            set
            {
                blogsList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlogsList)));
            }
        }
    }
}
