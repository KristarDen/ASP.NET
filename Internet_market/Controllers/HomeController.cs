using Internet_market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Internet_market.Models;

namespace Internet_market.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public List<Product> Products = new List<Product>();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product(int id)
        {
            Product P1 = new Product();
            P1.id = 1;
            P1.Name = "Футболка Powerwolf \"Metal Is Religion\"";
            P1.Price = 450;
            P1.Type = "T-Short";
            P1.Description = "Футболка с полной запечаткой, детализированым рисунком и слоганом группы Powerwolf - \"Metal is religion\" ";
            P1.Color = "Серый";
            P1.Size = "XL";
            P1.Material = "100% хлопок";
            P1.Photos.Add(new string("/Images/PW_t-short1.jpg"));
            P1.Photos.Add(new string("/Images/PW_t-short2.jpg"));
            Products.Add(P1);


            Product P2 = new Product();
            P2.id = 1;
            P2.Name = "Шапка Powerwolf";
            P2.Price = 170;
            P2.Type = "Hat";
            P2.Description = "Шапка группы Powerwolf с вышивкой";
            P2.Color = "Чёреный";
            P2.Size = "Безразмерная";
            P2.Material = "Полиакрил";
            P2.Photos.Add("/Images/PW_hat1.jpg");
            P2.Photos.Add("/Images/PW_hat2.jpg");
            P2.Photos.Add("/Images/PW_hat3.jpg");
            Products.Add(P2);


            return View("Product", Products[id-1]);
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
