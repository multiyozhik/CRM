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

        public async Task Add([FromForm] string name, string descriptor, string photo)
        {
            await context.Blogs.AddAsync(new Blog(Guid.NewGuid(), name, descriptor, photo, 
                DateTime.Today));
            context.SaveChanges();
        }

        public async Task Update(Blog blog)
        {
            var updatingBlog = await GetBlogById(blog.Id);
            updatingBlog = updatingBlog with
            {
                Name = blog.Name,
                Description = blog.Description,
                Photo = blog.Photo
            };
            context.Update(updatingBlog);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var blog = await GetBlogById(id);
            if (blog is not null) context.Blogs.Remove(blog);
            await context.SaveChangesAsync();
        }
    }
}
