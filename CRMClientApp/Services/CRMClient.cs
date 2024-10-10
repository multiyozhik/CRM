using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public async Task<FieldValuesViewModel?> GetFieldValues()
        {
            return await httpClient.GetFromJsonAsync<FieldValuesViewModel>(
                new Uri(baseAddress, "api/ApiGeneralInfo/GetFieldValues"));
        }

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

        public async Task AddOrder(OrderVM orderVM)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiHome/AddOrder"),
                new StringContent(
                    JsonSerializer.Serialize(orderVM), 
                    Encoding.UTF8, 
                    "application/json"));
        }

        public async Task<List<Project>?> GetProjectsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiProjects/GetProjects"));
            var projectsList = await httpResponse.Content.ReadFromJsonAsync<List<Project>>();
            projectsList = projectsList?.Select(
                project => project with { Photo = string.Concat(baseAddress, "img/", project.Photo) })
                ?.ToList();
            return projectsList;
        }

        public async Task<List<Service>?> GetServicesList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiServices/GetServices"));
            var servicesList = await httpResponse.Content.ReadFromJsonAsync<List<Service>>();
            return servicesList;
        }

        public async Task<List<Blog>?> GetBlogsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiBlogs/GetBlogs"));
            var blogsList = await httpResponse.Content.ReadFromJsonAsync<List<Blog>>();
            blogsList = blogsList?.Select(
                blog => blog with { Photo = string.Concat(baseAddress, "img/", blog.Photo) })
                ?.ToList();
            return blogsList;
        }

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
