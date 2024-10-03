using CRMSystem.Models;
using CRMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CRMSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private HomeModel Model { get; }

        public HomeController(HomeModel model)
        {
            Model = model;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View(); 
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddOrder(string name, string email, string message)
        {
            await Model.Add(name, email, message);
            return View("OrderAdded");
        }

        [HttpGet]
        public async Task<IActionResult> Desktop() => View(await Model.GetOrdersList());

        [HttpGet]
        public IActionResult ChangeStatus(Guid id)
        {
            ViewBag.OrderId = id;
            return View("ChangeStatus");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(OrderStatus status, Guid id)
        {
            await Model.UpdateOrderStatus(status, id);
            return RedirectToAction("Desktop");
        }

        [HttpPost]
        //AddDays(1), чтобы заявки последнего дня входили включительно, т.к. TimeStamp содержит время
        public async Task<IActionResult> FilterByDateRange([FromForm]DateTime dateStart, DateTime dateEnd)
        {
            var ordersByPeriod = await Model.FilterOrdersByDateRange(dateStart, dateEnd.AddDays(1));
            ViewBag.OrdersByPeriodCount = ordersByPeriod.Count;
            return View("Desktop", ordersByPeriod);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByPeriod([FromQuery]string period)
        {
            //иначе заявки последнего дня не войдут, т.к. TimeStamp содержит время
            var endDate = period =="yesterday"? DateTime.Today : DateTime.Today.AddDays(1);
            var ordersByPeriod = await Model.FilterOrdersByDateRange(
                period switch
                {
                    "today" => DateTime.Today,
                    "yesterday" => DateTime.Today.AddDays(-1),
                    "week" => DateTime.Today.AddDays(-7),
                    "month" => DateTime.Today.AddMonths(-1),
                    _ => DateTime.MinValue
                },
                endDate);
            ViewBag.OrdersByPeriodCount = ordersByPeriod.Count;
            return View("Desktop", ordersByPeriod);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] FieldValuesViewModel homeInterfaceVM)
        {
            using var fs = new FileStream("./wwwroot/files/default.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, homeInterfaceVM, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            });
            return RedirectToAction("Index");
        }
    }
}