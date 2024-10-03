using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    { 
        private readonly ServicesModel model;
        public ServicesController(ServicesModel servicesModel)
        {
            model = servicesModel; 
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await model.GetServicesList());
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();        
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string description)
        {
            await model.Add(name, description);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var service = await model.GetServiceById(id);
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Service service)
        {
            await model.Update(service);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await model.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
