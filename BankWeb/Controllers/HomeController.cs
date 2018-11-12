using BankWeb.Models;
using BankWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBankService _service;

        public HomeController(IBankService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(_service.GetCustomers());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
