using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
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
    }
}
