using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CRMSystem.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewLink(string logo, string linkPath)
        {
            var linksDict = await Deserialize("./wwwroot/files/social-media-links.json");
            linksDict?.Add($"/img/{logo}", linkPath);
            await Serialize("./wwwroot/files/social-media-links.json", linksDict);
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string logo)
        {
            var linksDict = await Deserialize("./wwwroot/files/social-media-links.json");
            linksDict?.Remove(logo);
            await Serialize("./wwwroot/files/social-media-links.json", linksDict);
            return RedirectToAction("Edit");
        }

        [NonAction]
        private static async Task Serialize(string path, Dictionary<string, string> dataDictionary)
        {
            using var fs = new FileStream(path, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, dataDictionary, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            });
        }

        [NonAction]
        private static async Task<Dictionary<string, string>> Deserialize(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
        }             
    }
}
