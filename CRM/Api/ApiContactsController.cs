using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ApiContactsController : Controller
    {
        [HttpPost("{logo}")]
        public async Task Delete([FromRoute] string logo)
        {
            var linksDict = await Deserialize("./wwwroot/files/social-media-links.json");
            linksDict?.Remove(logo);
            await Serialize("./wwwroot/files/social-media-links.json", linksDict);
        }

        [HttpPost]
        public async Task SaveNewLink(SocialMediaLinkDataFromRequest link)
        {
            var linksDict = await Deserialize("./wwwroot/files/social-media-links.json");
            linksDict?.Add($"/img/{link.IconPath}", link.HyperlinkUri);
            await Serialize("./wwwroot/files/social-media-links.json", linksDict);
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

    public record SocialMediaLinkDataFromRequest(string IconPath, string HyperlinkUri);
}
