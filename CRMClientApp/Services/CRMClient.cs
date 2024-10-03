using CRMClientApp.ViewModels;
using CRMClientApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;
using CRMClientApp.Models;
using System.IO;
using System.Net.Http.Json;

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
            projectsList = projectsList
                ?.Select(
                    project => project with { Photo =
                        string.Concat(baseAddress, "img/", project.Photo)
                    }
                )
                ?.ToList();
            return projectsList;
        }

        public async Task<Project?> GetProject(Guid projectId)
        { 
            var httpResponse = await httpClient.GetAsync(
                new Uri(baseAddress, $"api/ApiProjects/GetProjectById/{projectId}"));
            return await httpResponse.Content.ReadFromJsonAsync<Project>();
        }
    }
}
