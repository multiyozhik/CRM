using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ApiServicesController : ControllerBase
    {
        private readonly ServicesModel model;
        public ApiServicesController(ServicesModel servicesModel)
        {
            model = servicesModel;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Service>> GetServices()
            => await model.GetServicesList();

        [HttpPost]
        public async Task Add([FromBody] ServiceDataFromRequest serviceData)
        => await model.Add(serviceData.Name, serviceData.Description);

        [HttpGet("{id}")]
        public async Task<Service?> GetServiceById([FromRoute] Guid id)
        => await model.GetServiceById(id);

        [HttpPut]
        public async Task Update([FromBody] Service service)
        => await model.Update(service);

        [HttpPost("{id}")]
        public async Task Delete([FromRoute] Guid id)
        => await model.Delete(id);
    }
    public record ServiceDataFromRequest(string Name, string Description);
}
