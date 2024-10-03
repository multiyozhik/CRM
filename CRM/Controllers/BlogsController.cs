using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace CRMSystem.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly BlogsModel model;        
        public BlogsController(BlogsModel blogsModel)
        {
            model = blogsModel;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await model.GetBlogsList());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ArticleDescription()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string description, string photo)
        {
            await model.Add(name, description, photo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var blog = await model.GetBlogById(id);
            return View(blog);
        }

        [HttpPost]  
        public async Task<IActionResult> Update([FromForm] Blog blog)
        {
            await model.Update(blog);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await model.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
