namespace CRMSystem.Models
{
    public record Order(
        Guid Id, DateTime TimeStamp, string Name, string Email, string Text, OrderStatus Status)
    { };

    public enum OrderStatus
    {
        IsReceived,   //"Получена"
        InWork,       //"В работе"
        IsDone,      //"Выполнена"
        IsRejected,  //"Отклонена"
        IsCanceled   //"Отменена"
    }
}