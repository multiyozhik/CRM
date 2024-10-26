using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ApiBlogsController : ControllerBase
    {
        private readonly BlogsModel model;
        public ApiBlogsController(BlogsModel blogsModel)
        {
            model = blogsModel;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Blog>> GetBlogs()
            => await model.GetBlogsList();

        [HttpPost]
        public async Task<Guid> Add([FromBody] ArticleDataFromRequest articleData)
        {
            var id = await model.Add(articleData.Name, articleData.Description, articleData.Photo);
            return id;
        }

        [HttpPut]
        public async Task Update([FromBody] Blog blog)
            => await model.Update(blog);

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] Guid id)
            => await model.Delete(id);
    }

    public record ArticleDataFromRequest(string Name, string Description, string Photo);
}
