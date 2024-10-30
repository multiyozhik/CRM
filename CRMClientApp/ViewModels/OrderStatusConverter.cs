using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CRMClientApp.ViewModels
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var orderStatus = (OrderStatus)value;

            return orderStatus switch
            {
                OrderStatus.InWork => "В работе",
                OrderStatus.IsDone => "Выполнена",
                OrderStatus.IsRejected => "Отклонена",
                OrderStatus.IsCanceled => "Отменена",
                OrderStatus.IsReceived => "Получена",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var orderStatus = (string)value;

            return orderStatus switch
            {
                "В работе" => OrderStatus.InWork,
                "Выполнена" => OrderStatus.IsDone,
                "Отклонена" => OrderStatus.IsRejected,
                "Отменена" => OrderStatus.IsCanceled,
                "Получена" => OrderStatus.IsReceived,
                _ => null!
            };
        }
    }
}
