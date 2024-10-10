using CRMClientApp.Commands;
using CRMClientApp.Models;
using CRMClientApp.Services;
using CRMClientApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Windows.Controls;
using System.Windows;

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

        //IsAdmin для авторизации

        private string? currentUserName;
        public string? CurrentUserName
        {
            get => currentUserName;
            set
            {
                currentUserName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentUserName)));
            }
        }

        private bool isAdmin = false;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isAdmin)));
                EnableLoginCommand = false;
            }
        }

        private bool enableLoginCommand = true;
        public bool EnableLoginCommand
        {
            get => enableLoginCommand;
            set
            {
                enableLoginCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(enableLoginCommand)));
            }
        }

        private AsyncRelayCommand loginCommand;
        public AsyncRelayCommand LoginCommand
        {
            get => loginCommand ?? new AsyncRelayCommand(async obj =>
            {

                var loginWindow = new LoginWindow()
                {
                    DataContext = new LoginViewModel()
                };
                var loginWindowResult = loginWindow.ShowDialog();

                if (loginWindowResult is true)
                {
                    var loginVM = (LoginViewModel)loginWindow.DataContext;
                    if (await crmClient.Login(loginVM))
                    {
                        IsAdmin = true;
                        CurrentUserName = loginVM.UserName;
                    }
                    else
                        MessageBox.Show("Пользователь не найден");
                    return;
                }
                else
                    MessageBox.Show("Ошибка ввода");
            });
        }

        private AsyncRelayCommand logoutCommand;
        public AsyncRelayCommand LogoutCommand
        {
            get => logoutCommand ?? new AsyncRelayCommand(async obj =>
            {
                await crmClient.Logout();
                IsAdmin = false;
                CurrentUserName = default;
                EnableLoginCommand = true;
            });
        }

        //названия вкладок

        private FieldValuesViewModel fieldValues;
        public FieldValuesViewModel FieldValues
        {
            get => fieldValues;
            set
            {
                fieldValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(fieldValues)));
            }
        }

        //контакты и соц. сети

        private ContactsValuesViewModel contactsValues;
        public ContactsValuesViewModel ContactsValues
        {
            get => contactsValues;
            set
            {
                contactsValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(contactsValues)));
            }
        }

        private ObservableCollection<SocialMediaLinkVM> socialMediaLinksList;
        public ObservableCollection<SocialMediaLinkVM> SocialMediaLinksList
        {
            get => socialMediaLinksList;
            set
            {
                socialMediaLinksList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(socialMediaLinksList)));
            }
        }

        //заявки
        public OrderVM AddingOrder { get; } = new OrderVM();

        private AsyncRelayCommand? addOrderCommand; 
        public AsyncRelayCommand? AddOrderCommand
        {
            get => addOrderCommand ??= new AsyncRelayCommand(
                async obj => await crmClient.AddOrder(AddingOrder));
        }

        //проекты

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

        //сервисы

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
        
        //блог

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
