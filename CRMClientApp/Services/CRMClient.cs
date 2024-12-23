﻿using CRMClientApp.Models;
using CRMClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

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


        //метод для формирования Url-адреса картинки для клиента

        public string GetUrlFileName(string fileName)
            => string.Concat(baseAddress.ToString(), "img/", fileName);

        public async Task<FieldValuesViewModel?> GetFieldValues()
        {
            return await httpClient.GetFromJsonAsync<FieldValuesViewModel>(
                new Uri(baseAddress, "api/ApiGeneralInfo/GetFieldValues"));
        }

        //загрузка файла на сервер и удаление файла на сервере

        public async Task<string> UploadFile(string filePath, string mediaType)
        {
            using var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(File.OpenRead(filePath));

            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);   //image/png, image/svg+xml
            multipartFormContent.Add(fileStreamContent, "file", Path.GetFileName(filePath));
            var httpResponse = await httpClient
                .PostAsync(new Uri(baseAddress, "api/ApiGeneralInfo/UploadFile"), multipartFormContent);
            var res = await httpResponse.Content.ReadAsStringAsync();
            return res;
        }

        public async Task DeleteFile(string fileName)
        {
            await httpClient.DeleteAsync(
                new Uri(baseAddress, $"api/ApiGeneralInfo/DeleteFile/{fileName}"));
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
                    IconPath = new Uri(baseAddress, link.Key.TrimStart('/')).ToString(),
                    HyperlinkUri = link.Value
                })
                .ToList();
            return linkList;
        }

        public async Task AddLink(SocialMediaLinkVM newLink)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiContacts/SaveNewLink"), 
                JsonContent.Create(newLink));
        }

        public async Task DeleteLink(string iconFileName)
        {
            await httpClient.DeleteAsync(
                new Uri(baseAddress, $"api/ApiContacts/Delete/{iconFileName}"));
        }


        //заявки

        public async Task AddOrder(OrderVM orderVM)
        {
            await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiHome/AddOrder"), JsonContent.Create(orderVM));
            MessageBox.Show("Заявка отправлена");
        }

        public async Task EditOrderControl(FieldValuesViewModel fieldValuesViewModel)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiHome/Edit"),
                JsonContent.Create(fieldValuesViewModel));
        }

        public async Task<List<OrderVM>?> FilterOrdersByPeriod(string period)
        {
            var response = await httpClient.GetAsync(
                new Uri(baseAddress, $"api/ApiHome/FilterByPeriod/{period}"));
            var ordersList = await response.Content.ReadFromJsonAsync<List<OrderVM>>();
            return ordersList;
        }

        //рабочий стол

        public async Task<List<OrderVM>?> GetOrdersList()
        {
            var response = await httpClient.GetAsync(new Uri(baseAddress, "api/ApiHome/GetOrders"));
            return await response.Content.ReadFromJsonAsync<List<OrderVM>>();
        }

        public async Task<int> GetTotalOrdersListCount()
        {
            var ordersList = await GetOrdersList();
            return ordersList?.Count ?? 0;
        }

        public async Task<List<OrderVM>> FilterOrdersByDateRange(DateTime dateStart, DateTime dateEnd)
        {
            var response = await httpClient.PostAsync(new Uri(baseAddress, "api/ApiHome/FilterByDateRange"), 
                JsonContent.Create(new { DateStart = dateStart, DateEnd = dateEnd}));
            var ordersList = await response.Content.ReadFromJsonAsync<List<OrderVM>>();
            return ordersList;
        }

        public async Task ChangeStatus(OrderStatus status, Guid id)
        {
            await httpClient.PostAsync(new Uri(baseAddress, $"api/ApiHome/ChangeStatus/{id}"),
                JsonContent.Create(status));
        }

        //проекты

        public async Task<List<Project>?> GetProjectsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiProjects/GetProjects"));
            var projectsList = await httpResponse.Content.ReadFromJsonAsync<List<Project>>();
            return projectsList?.Select(project => {
                project.Photo = GetUrlFileName(project.Photo);
                return project; 
            }).ToList();
        }
           
        public async Task<Guid> AddProject(Project project)
        {
            var response = await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiProjects/Add"), 
                JsonContent.Create(project));
            var projectId = await response.Content.ReadFromJsonAsync<Guid>();
            return projectId;
        }

        public async Task EditProject(Project project)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiProjects/Update"), 
                JsonContent.Create(project));
        }

        public async Task DeleteProject(Guid id)
        {
            await httpClient.DeleteAsync(
                new Uri(baseAddress, $"api/ApiProjects/Delete/{id}"));
        }

        //услуги

        public async Task<List<Service>?> GetServicesList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiServices/GetServices"));
            return await httpResponse.Content.ReadFromJsonAsync<List<Service>>();
        }

        public async Task<Guid> AddService(Service service)
        {
            var response = await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiServices/Add"), 
                JsonContent.Create(service));
            var serviceId = await response.Content.ReadFromJsonAsync<Guid>();
            return serviceId;
        }

        public async Task EditService(Service service)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiServices/Update"),
                JsonContent.Create(service));
        }

        public async Task DeleteService(Guid id)
        {
            await httpClient.DeleteAsync(
                new Uri(baseAddress, $"api/ApiServices/Delete/{id}"));
        }


        //блог

        public async Task<List<Blog>?> GetBlogsList()
        {
            var httpResponse = await httpClient
                .GetAsync(new Uri(baseAddress, "api/ApiBlogs/GetBlogs"));
            var blogsList = await httpResponse.Content.ReadFromJsonAsync<List<Blog>>();
            blogsList = blogsList?.Select(
                blog => { 
                    blog.Photo = string.Concat(baseAddress, "img/", blog.Photo);
                    blog.CreateAt = new DateTime(blog.CreateAt.Year, blog.CreateAt.Month, blog.CreateAt.Day);
                    return blog;
                }).ToList();
            return blogsList;
        }

        public async Task<Guid> AddBlog(Blog blog)
        {
            var response = await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiBlogs/Add"),
                JsonContent.Create(blog));
            var blogId = await response.Content.ReadFromJsonAsync<Guid>();
            return blogId;
        }

        public async Task EditBlog(Blog blog)
        {
            await httpClient.PutAsync(
                new Uri(baseAddress, "api/ApiBlogs/Update"),
                JsonContent.Create(blog));
        }

        public async Task DeleteBlog(Guid id)
        {
            await httpClient.DeleteAsync(
                new Uri(baseAddress, $"api/ApiBlogs/Delete/{id}"));
        }


        //аутентификация

        public async Task<bool> Login(LoginViewModel loginVM)
        {
            var httpResponse = await httpClient.PostAsync(
                new Uri(baseAddress, "api/ApiAccount/LogIn"),
                JsonContent.Create(loginVM));
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task Logout()
        {            
            await httpClient.GetAsync(
                new Uri(baseAddress, "api/ApiAccount/LogOut"));
        }
    }
}
