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
        [HttpDelete("{iconFileName}")]
        public async Task Delete([FromRoute] string iconFileName)
        {
            var directory = $"{Directory.GetCurrentDirectory()}/wwwroot/files/";
            var linksDict = await Deserialize($"{directory}/social-media-links.json");
            var b = linksDict?.Remove($"/img/{iconFileName}");
            await Serialize($"{directory}/social-media-links.json", linksDict);
        }

        [HttpPost]
        public async Task SaveNewLink([FromBody] SocialMediaLinkVM link)
        {
            var linksDict = await Deserialize("/files/social-media-links.json");
            linksDict?.Add($"/img/{link.IconPath}", link.HyperlinkUri);
            await Serialize("/files/social-media-links.json", linksDict);
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

    public record SocialMediaLinkVM(string IconPath, string HyperlinkUri);
}
