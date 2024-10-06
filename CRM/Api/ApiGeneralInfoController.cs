using CRMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
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
            var s =  await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(linksStream);
            return s;
        }
    }
}
