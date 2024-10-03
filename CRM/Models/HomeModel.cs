using CRMSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace CRMSystem.Models
{
    public class HomeModel
    {
        private readonly OrdersDbContext context;
        public HomeModel(OrdersDbContext ordersDBContext)
        {
            context = ordersDBContext;
        }

        public async Task Add(string name, string email, string message)
        {
            await context.Orders.AddAsync(new Order(
                Guid.NewGuid(), DateTime.Now.ToLocalTime(), name, email, message, OrderStatus.IsReceived));
            context.SaveChanges();
        }

        public async Task<List<Order>> GetOrdersList()
            => await context.Orders.ToListAsync();

        public async Task UpdateOrderStatus(OrderStatus status, Guid id)
        {
            var order = await context.Orders.FirstOrDefaultAsync(order => order.Id == id);
            if (order != null)
            {
                var updatedOrder = order with { Status = status };
                context.Orders.Remove(order);
                await context.Orders.AddAsync(updatedOrder);
                context.SaveChanges();
            }
        }

        public async Task<List<Order>> FilterOrdersByDateRange(DateTime dateStart, DateTime dateEnd)
            => await context.Orders.Where(order => (order.TimeStamp >= dateStart && order.TimeStamp <= dateEnd))
            .ToListAsync();
    }
}
