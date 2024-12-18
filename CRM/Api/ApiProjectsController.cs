﻿using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ApiProjectsController : ControllerBase
    {
        private readonly ProjectsModel model;
        public ApiProjectsController(ProjectsModel projectsModel)
        {
            model = projectsModel;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Project>> GetProjects()
            => await model.GetProjectsList();

        [HttpPost]
        public async Task<Guid> Add([FromBody] ProjectDataFromRequest projectData)
        {
            var id = await model.Add(projectData.Name, projectData.Description, projectData.Photo);
            return id;
        }

        [HttpPut]
        public async Task Update([FromBody] Project project)
            => await model.Update(project);


        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] Guid id)
            => await model.Delete(id);
    }

    public record ProjectDataFromRequest(string Name, string Description, string Photo);
}
