using System;
using System.ComponentModel;

namespace CRMClientApp.ViewModels
{
    public class OrderVM : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }        
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Text { get; set; }

        private OrderStatus status;
        public OrderStatus Status
        {
            get => status;
            set
            {
                status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public enum OrderStatus
    {
        IsReceived,   //"Получена"
        InWork,       //"В работе"
        IsDone,      //"Выполнена"
        IsRejected,  //"Отклонена"
        IsCanceled   //"Отменена"
    }
}
