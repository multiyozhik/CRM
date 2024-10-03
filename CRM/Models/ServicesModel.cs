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

        public async Task Add([FromForm] string name, string descriptor)
        {
            await context.Services.AddAsync(new Service(Guid.NewGuid(), name, descriptor));
            context.SaveChanges();
        }

        public async Task Update(Service service)
        {
            var updatingService = await GetServiceById(service.Id);
            updatingService = updatingService with
            {
                Name = service.Name,
                Description = service.Description
            };
            context.Update(updatingService);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var service = await GetServiceById(id);
            if (service is not null) context.Services.Remove(service);
            await context.SaveChangesAsync();
        }
    }
}
