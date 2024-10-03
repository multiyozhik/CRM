using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Models
{
    public class ProjectsModel
    {
        private readonly OrdersDbContext context;
        public ProjectsModel(OrdersDbContext ordersDbContext) 
        {
            context = ordersDbContext;
            ordersDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //отслеживание отключаем, иначе для record проекта конфликт по Id
        }

        public async Task<List<Project>> GetProjectsList() => await context.Projects.ToListAsync();

        public async Task<Project?> GetProjectById(Guid id)
            => await context.Projects.FirstOrDefaultAsync(project => project.Id == id);

        public async Task Add([FromForm] string name, string descriptor, string photo)
        {
            await context.Projects.AddAsync(new Project(Guid.NewGuid(), name, descriptor, photo));
            context.SaveChanges();
        }

        public async Task Update(Project project)
        {
            var updatingProject = await GetProjectById(project.Id);
            updatingProject = updatingProject with {
                Name = project.Name, Description = project.Description, Photo = project.Photo };
            context.Update(updatingProject);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var project = await GetProjectById(id);
            if (project is not null) context.Projects.Remove(project);
            await context.SaveChangesAsync();
        }
    }
}
