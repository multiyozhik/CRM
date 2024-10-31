using CRMClientApp.Commands;
using CRMClientApp.Models;
using CRMClientApp.Services;
using CRMClientApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace CRMClientApp.ViewModels
{
    public class CRMViewModel : INotifyPropertyChanged
    {
        public CRMClient CrmClient { get; }
        public CRMViewModel(CRMClient crmClient)
        {
            CrmClient = crmClient;
            DateStart = DateTime.Now;   
            DateEnd = DateTime.Now;
            OrdersList.CollectionChanged += OrdersList_CollectionChanged;
        }

        private void OrdersList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is null) return;
            foreach (OrderVM order in e.NewItems)
            {
                order.PropertyChanged += Order_PropertyChanged;
            }
        }

        private async void Order_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Status") return;
            var orderVM = (OrderVM)sender;            
            await CrmClient.ChangeStatus(orderVM.Status, orderVM.Id);
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
                VisibleInitialVisibility = Visibility.Visible;
            }
        }

        private Visibility visibleInitialVisibility = Visibility.Visible;
        public Visibility VisibleInitialVisibility
        {
            get => visibleInitialVisibility;
            set
            {
                visibleInitialVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(visibleInitialVisibility)));
            }
        }

        private Visibility collapsedInitialVisibility = Visibility.Collapsed;
        public Visibility CollapsedInitialVisibility
        {
            get => collapsedInitialVisibility;
            set
            {
                collapsedInitialVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(collapsedInitialVisibility)));
            }
        }

        private readonly AsyncRelayCommand loginCommand;
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
                    if (await CrmClient.Login(loginVM))
                    {
                        IsAdmin = true;
                        CurrentUserName = loginVM.UserName;
                        VisibleInitialVisibility = Visibility.Collapsed;
                        CollapsedInitialVisibility = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("Пользователь не найден");
                    return;
                }
                else
                    MessageBox.Show("Ошибка ввода");
            });
        }

        private readonly AsyncRelayCommand logoutCommand;
        public AsyncRelayCommand LogoutCommand
        {
            get => logoutCommand ?? new AsyncRelayCommand(async obj =>
            {
                await CrmClient.Logout();
                IsAdmin = false;
                CurrentUserName = default;
                VisibleInitialVisibility = Visibility.Visible;
                CollapsedInitialVisibility = Visibility.Collapsed;
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

        public List<string> OrderStatusList { get; } = new List<string>() {
            "Получена", "В работе", "Выполнена", "Отклонена", "Отменена"};

        public OrderVM AddingOrder { get; } = new OrderVM();

        private AsyncRelayCommand? addOrderCommand; 
        public AsyncRelayCommand? AddOrderCommand
        {
            get => addOrderCommand ??= new AsyncRelayCommand(
                async obj =>
                {
                await CrmClient.AddOrder(AddingOrder);
                TotalOrdersCount += 1;
                });
        }

        private readonly AsyncRelayCommand editOrderControlCommand;
        public AsyncRelayCommand EditOrderControlCommand
        {
            get => editOrderControlCommand ?? new AsyncRelayCommand(async obj =>
            {
                FieldValues = await CrmClient.GetFieldValues();
                var editOrderControlWindow = new EditOrderControlWindow()
                {
                    DataContext = FieldValues
                };
                var editOrderControlResult = editOrderControlWindow.ShowDialog();
                if (editOrderControlResult is true)
                {
                    var fieldValuesViewModel = (FieldValuesViewModel)editOrderControlWindow.DataContext;
                    await CrmClient.EditOrderControl(fieldValuesViewModel);
                    FieldValues = await CrmClient.GetFieldValues();
                }
                else
                {
                    MessageBox.Show("Ошибка ввода. Необходимо было корректно ввести все поля");
                    return;
                }
            });
        }

        //рабочий стол

        private OrderVM? selectedOrder;
        public OrderVM? SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOrder)));
            }
        }
        public ObservableCollection<OrderVM> OrdersList { get; } = new ObservableCollection<OrderVM>();

        private int totalOrdersCount;
        public int TotalOrdersCount
        {
            get => totalOrdersCount;
            set
            {
                totalOrdersCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalOrdersCount)));
            }
        }

        private DateTime dateStart;
        public DateTime DateStart
        {
            get => dateStart;
            set
            {
                dateStart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateStart)));
            }
        }

        private DateTime dateEnd;
        public DateTime DateEnd
        {
            get => dateEnd;
            set
            {
                dateEnd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateEnd)));
            }
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
