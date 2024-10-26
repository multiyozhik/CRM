using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Models
{
    public class BlogsModel
    {
        private readonly OrdersDbContext context;
        public BlogsModel(OrdersDbContext ordersDbContext)
        {
            context = ordersDbContext;
            ordersDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<List<Blog>> GetBlogsList() => await context.Blogs.ToListAsync();

        public async Task<Blog?> GetBlogById(Guid id)
            => await context.Blogs.FirstOrDefaultAsync(blog => blog.Id == id);

        public async Task<Guid> Add(string name, string descriptor, string photo)
        {
            var blogId = Guid.NewGuid();
            await context.Blogs.AddAsync(new Blog(blogId, name, descriptor, photo, 
                DateTime.Today));
            context.SaveChanges();
            return blogId;
        }

        public async Task Update(Blog blog)
        {
            var count = await context.Blogs.Where(b => b.Id == blog.Id).ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(b => b.Name, blog.Name)
                    .SetProperty(b => b.Description, blog.Description)
                    .SetProperty(b => b.Photo, blog.Photo));
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var count = await context.Blogs.Where(b => b.Id == id).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }
    }
}
