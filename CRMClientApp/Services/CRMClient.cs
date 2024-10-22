using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRMClientApp.Services
{
    public class CRMClient
    {
        private readonly Uri baseAddress;
        private static readonly HttpClient httpClient = new();
        public CRMClient(Uri baseAddress)
        {
            this.baseAddress = baseAddress;
            httpClient.BaseAddress = baseAddress;
        }


        //формирование адреса картинки для клиента

        public string GetPhotoUrl(string photoName)
            => string.Concat(baseAddress.ToString(), "img/", photoName);

        public async Task<FieldValuesViewModel?> GetFieldValues()
        {
            return await httpClient.GetFromJsonAsync<FieldValuesViewModel>(
                new Uri(baseAddress, "api/ApiGeneralInfo/GetFieldValues"));
        }

        //контакты
        public async Task<ContactsValuesViewModel?> GetContactsValues()
        {
            var contactsVM = await httpClient.GetFromJsonAsync<ContactsValuesViewModel>(
                new Uri(baseAddress, "api/ApiGeneralInfo/GetContactsValues"));
            contactsVM.MapPath = new Uri(baseAddress, contactsVM.MapPath).ToString();
            return contactsVM;
        }

        public async Task<List<SocialMediaLinkVM>> GetSocialMediaLinks()
        {
            var linksDict = await httpClient.GetFromJsonAsync<Dictionary<string, string>>(
                        new Uri(baseAddress, "api/ApiGeneralInfo/GetSocialMediaLinks"));
            var linkList = linksDict.Select(link =>
                new SocialMediaLinkVM()
                {
                    Icon = new Uri(baseAddress, link.Key.TrimStart('/')).ToString(),
                    HyperlinkUri = link.Value
                })
                .ToList();
            return linkList;
        }

        public async Task AddLink(SocialMediaLinkVM newLink)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiContacts/SaveNewLink"), 
                new StringContent(
                    JsonSerializer.Serialize(newLink),
                    Encoding.UTF8,
                    "application/json"));
        }

        public async Task DeleteLink(string iconPath)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiContacts/Delete"),
                new StringContent(iconPath));
        }

        //заявки

        public async Task AddOrder(OrderVM orderVM)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiHome/AddOrder"),
                new StringContent(
                    JsonSerializer.Serialize(orderVM), 
                    Encoding.UTF8, 
                    "application/json"));
        }

        public async Task EditOrderControl(FieldValuesViewModel fieldValuesViewModel)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiHome/Edit"),
                new StringContent(
                    JsonSerializer.Serialize(fieldValuesViewModel),
                    Encoding.UTF8,
                    "application/json"));
        }

        //проекты

        public async Task<List<Project>?> GetProjectsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiProjects/GetProjects"));
            var projectsList = await httpResponse.Content.ReadFromJsonAsync<List<Project>>();
            projectsList = projectsList?.Select(project => {
                project.Photo = GetPhotoUrl(project.Photo);
                return project; 
            }).ToList();
            return projectsList;
        }

        public async Task<string> UploadFile(string filePath)
        {
            using var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(File.OpenRead(filePath));

            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(fileStreamContent, "file", Path.GetFileName(filePath));
            var httpResponse = await httpClient
                .PostAsync(new Uri(baseAddress, "api/ApiGeneralInfo/UploadFile"), multipartFormContent);
            return await httpResponse.Content.ReadAsStringAsync();
        }
    
        public async Task AddProject(Project project)
        {
            var content = JsonContent.Create(project);
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiProjects/Add"), content);
        }

        public async Task EditProject(Project project)
        {
            var content = JsonContent.Create(project);
            await httpClient.PutAsync(new Uri(baseAddress, "api/ApiProjects/Update"), content);
        }

        public async Task DeleteProject(Project project)
        {
            var content = JsonContent.Create(project);
            await httpClient.PostAsync(new Uri(baseAddress, "api/ApiProjects/Delete"), content);
        }

        //услуги

        public async Task<List<Service>?> GetServicesList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiServices/GetServices"));
            var servicesList = await httpResponse.Content.ReadFromJsonAsync<List<Service>>();
            return servicesList;
        }

        public async Task AddService(Service service)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiServices/Add"),
                new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
        }

        public async Task EditService(Service service)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiServices/Update"),
                new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
        }

        public async Task DeleteService(Service service)
        {
            await httpClient.PostAsync(new Uri(baseAddress, "api/ApiServices/Delete"),
                new StringContent(service.Id.ToString()));
        }


        //блог

        public async Task<List<Blog>?> GetBlogsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiBlogs/GetBlogs"));
            var blogsList = await httpResponse.Content.ReadFromJsonAsync<List<Blog>>();
            blogsList = blogsList?.Select(
                blog => { blog.Photo = string.Concat(baseAddress, "img/", blog.Photo);
                    return blog;
                }).ToList();
            return blogsList;
        }

        public async Task AddBlog(Blog blog)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiBlogs/Add"),
                new StringContent(JsonSerializer.Serialize(blog), Encoding.UTF8, "application/json"));
        }

        public async Task EditBlog(Blog blog)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiBlogs/Update"),
                new StringContent(JsonSerializer.Serialize(blog), Encoding.UTF8, "application/json"));
        }

        public async Task DeleteBlog(Blog blog)
        {
            await httpClient.PostAsync(new Uri(baseAddress, "api/ApiBlogs/Delete"),
                new StringContent(blog.Id.ToString()));
        }


        //аутентификация

        public async Task<bool> Login(LoginViewModel loginVM)
        {
            var httpResponse = await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiAccount/LogIn"),
                new StringContent(
                    JsonSerializer.Serialize(loginVM),
                    Encoding.UTF8,
                    "application/json"));
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task Logout()
        {            
            await httpClient.GetAsync(
                new Uri(baseAddress, "api/ApiAccount/LogOut"));
        }
    }
}
