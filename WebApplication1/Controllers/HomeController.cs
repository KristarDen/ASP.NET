using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
        [HttpPost]
        public string Calc(int First_Num,  string operation_type, int Second_Num)
        {
            double result = 0;
            string symbol = "";
            double a = First_Num;
            double b = Second_Num;

            switch (int.Parse(operation_type))
            {
                case 1:
                    result = a + b;
                    symbol = "+";
                    break;

                case 2:
                    result = a - b;
                    symbol = "-";
                    break;

                case 3:
                    if (b == 0) return $"You cant divide by zerro";
                    result = a / b;
                    symbol = "/";
                    break;

                case 4:
                    result = a * b;
                    symbol = "*";
                    break;

                default:
                    break;
            }

            return $"{a}{symbol}{b} = {result}";
        }
    }
}
