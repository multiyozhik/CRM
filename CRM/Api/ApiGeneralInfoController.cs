using CRMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class ApiGeneralInfoController : Controller
    {
        [HttpGet]
        public async Task<FieldValuesViewModel?> GetFieldValues()
        {
            using var fs = new FileStream("./wwwroot/files/default.json", FileMode.Open);
            return await JsonSerializer.DeserializeAsync<FieldValuesViewModel>(fs);
        }

        [HttpGet]
        public async Task<ContactsValuesViewModel?> GetContactsValues()
        {
            using var fs = new FileStream("./wwwroot/files/contacts.json", FileMode.Open);
            return await JsonSerializer.DeserializeAsync<ContactsValuesViewModel>(fs);
        }

        [HttpGet]
        public async Task<Dictionary<string, string>?> GetSocialMediaLinks()
        {
            using var linksStream = new FileStream("./wwwroot/files/social-media-links.json", FileMode.Open);
            return  await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(linksStream);
        }

        [Authorize]
        [HttpPost]
        public async Task<string> UploadFile([FromForm] IFormFile file) 
        {
            var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/img/";
            Directory.CreateDirectory(uploadPath);
            var fileGuidName = Guid.NewGuid().ToString();
            string fullPath = $"{uploadPath}/{fileGuidName}_{Path.GetFileName(file.FileName)}";
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return Path.GetFileName(fullPath);
        }

        [Authorize]
        [HttpDelete("{fileName}")]
        public async Task DeleteFile([FromRoute] string fileName)     
        {            
            var directoryPath = $"{Directory.GetCurrentDirectory()}/wwwroot/img/";
            var filePath = Path.Combine(directoryPath, Path.GetFileName(fileName));
            System.IO.File.Delete(filePath);          
        }
    }
}
