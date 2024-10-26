using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Models
{
    public class ServicesModel
    {
        private readonly OrdersDbContext context;
        public ServicesModel(OrdersDbContext ordersDbContext) 
        {
            context = ordersDbContext;
            ordersDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<List<Service>> GetServicesList() => await context.Services.ToListAsync();

        public async Task<Service?> GetServiceById(Guid id)
            => await context.Services.FirstOrDefaultAsync(service => service.Id == id);

        public async Task<Guid> Add(string name, string descriptor)
        {
            var serviceId = Guid.NewGuid();
            await context.Services.AddAsync(new Service(serviceId, name, descriptor));
            context.SaveChanges();
            return serviceId;
        }

        public async Task Update(Service service)
        {
            var count = await context.Services.Where(s => s.Id == service.Id).ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(s => s.Name, service.Name)
                    .SetProperty(s => s.Description, service.Description));
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var count = await context.Services.Where(s => s.Id == id).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }
    }
}
