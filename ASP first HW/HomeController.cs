using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_first_HW
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
                    if (b == 0) return $"<h2 style=\"color: red\">You cant divide by zerro</h2>";
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

            return $"<h3>{a}{symbol}{b}={result}<\\h3>";
        }
    }
}
