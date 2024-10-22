using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            var count = await context.Projects.Where(p => p.Id == project.Id).ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(p => p.Name, project.Name)
                    .SetProperty(p => p.Description, project.Description)
                    .SetProperty(p => p.Photo, project.Photo));

            //var updatingProject = await GetProjectById(project.Id);
            //updatingProject = updatingProject with {
            //    Name = project.Name, Description = project.Description, Photo = project.Photo };
            //context.Update(updatingProject);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Project project)
        {
            var count = context.Projects
                .Where(p => p.Id == project.Id).ExecuteDelete(); //эффективнее, чем context.Projects.Remove(project);
            await context.SaveChangesAsync();
        }
    }
}
