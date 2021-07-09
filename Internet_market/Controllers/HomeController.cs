﻿using Internet_market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Internet_market.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public List<Product> Products = new List<Product>();

        private ApplicationContext db_context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db_context = context;

           // Product P1 = new Product();
           ////P1.id = 1;
           // P1.Name = "Футболка Powerwolf \"Metal Is Religion\"";
           // P1.Price = 450;
           // P1.Type = "Футболка";
           // P1.Description = "Футболка с полной запечаткой, детализированым рисунком и слоганом группы Powerwolf - \"Metal is religion\" ";
           // P1.Color = "Серый";
           // P1.Size = "XL";
           // P1.Material = "100% хлопок";
           // db_context.Products.Add(P1);         


           // Product P2 = new Product();
           //// P2.id = 2;
           // P2.Name = "Шапка Powerwolf";
           // P2.Price = 170;
           // P2.Type = "Шапка";
           // P2.Description = "Шапка группы Powerwolf с вышивкой";
           // P2.Color = "Чёреный";
           // P2.Size = "Безразмерная";
           // P2.Material = "Полиакрил";
           // db_context.Products.Add(P2);

           // db_context.SaveChanges();

           // db_context.Photos.Add(new Photo { source = "/Images/PW_t-short1.jpg", ProductId = 1 });
           // db_context.Photos.Add(new Photo { source = "/Images/PW_t-short2.jpg", ProductId = 1 });

           // db_context.Photos.Add(new Photo {source = "/Images/PW_hat1.jpg", ProductId = 2 });
           // db_context.Photos.Add(new Photo { source = "/Images/PW_hat2.jpg", ProductId = 2 });
           // db_context.Photos.Add(new Photo { source = "/Images/PW_hat3.jpg", ProductId = 2 });

           // db_context.SaveChanges();
        }

        

        public async Task<IActionResult> Index()
        {

            var prod_n_photo = await db_context.Products.Include(p => p.Photos).ToListAsync();            

            return View("Index", prod_n_photo);
           
            
        }

        public IActionResult Product(int id)
        {
            Product product = db_context.Products.Find(id);
            var photos = db_context.Photos.Where(photo => photo.ProductId == id).ToList();

            ViewData["Product"] = product;
            ViewData["Photos"] = photos;

            return View("Product", ViewData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cart()
        {
           
            if(HttpContext.Request.Cookies.Keys.Contains("p1"))
            {
                List<Product> products = new List<Product>();
                List<Photo> photos = new List<Photo>();
                int i = 1;
                while(true)
                {
                    if(HttpContext.Request.Cookies.Keys.Contains($"p{i}"))
                    {
                        int id = Convert.ToInt32(HttpContext.Request.Cookies[$"p{i}"]);
                        Product product = db_context.Products.Find(id);
                        var photoOfProd = db_context.Photos.Where(photo => photo.ProductId == id).ToList();

                        products.Add(product);
                        photos.Add(photoOfProd[0]);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
                ViewData["Products"] = products;
                ViewData["Photos"] = photos;
            }
            return View("Cart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
