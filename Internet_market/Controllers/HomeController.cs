using Internet_market.Models;
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
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;

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
        }

        

        public async Task<IActionResult> Index()
        {

            var ProdWithphoto = await db_context.Products.Include(p => p.Photos).ToListAsync();            

            return View("Index", ProdWithphoto);
           
            
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

        public IActionResult AddProduct(int id)
        {
            var option = new CookieOptions
            {
                HttpOnly = false,
                Path = "/"
            };

            //перебор имеющихся cookies и добавление в конец
            if (HttpContext.Request.Cookies.Keys.Contains($"p{1}"))
            {
                int i = 1;
                while(true)
                {
                    if (!HttpContext.Request.Cookies.Keys.Contains($"p{i}"))
                    {
                        HttpContext.Response.Cookies.Append($"p{i}", $"{id}", option);
                        break;
                    }
                    else i++;
                }

            }

            else
            {
                HttpContext.Response.Cookies.Append($"p1", $"{id}", option);
            }

            if(HttpContext.User.Identity.IsAuthenticated == true)
            {
                int UserId;
                string[] userInfo = HttpContext.User.Identity.Name.Split('|');
                UserId = Convert.ToInt32(userInfo[0]);

                Order ord = new Order
                {
                    User = db_context.Users.Find(UserId),
                    UserId = UserId,
                    dateTime = DateTime.Now,
                    ProductId = id,
                    Product = db_context.Products.Find(id)
                };

                db_context.Orders.Add(ord);

                db_context.SaveChanges();
            }
            return Redirect($"~/Home/Product/{id}");
        }

        public IActionResult OrderForm()
        {
            return View();
        }

        public async Task <IActionResult> Order(string phone)
        {
            FillCartFromCookies();
            DelAllItemsFromCookies();

            int UserId;
            string[] userInfo = HttpContext.User.Identity.Name.Split('|');
            UserId = Convert.ToInt32(userInfo[0]);

            var user = db_context.Users.Find(UserId);

            string message = "Заказ\n Информация о заказщике :\n Имя" + user.FirstName +"\n"+ "Фамилия" + user.LastName + "\n"
                + "Email: " + user.Email;

            var Orders = db_context.Orders.Where(order => order.UserId == UserId);

            decimal sum = 0;

            int i = 1;
            foreach(var order in Orders)
            {
                var prod = db_context.Products.Find(order.ProductId);
                message += $"№{i} #id {prod.Id} | {prod.Name} | размер {prod.Size} | цвет {prod.Color} | {prod.Type} | {prod.Price}\n";
                sum += prod.Price;
                i++;

                db_context.Orders.Remove(order);
            }
            message += $"Всего {sum} ₴";

            string email = user.Email;

            await SendEmailAsync(email, "Новый заказ", message);

            ViewData.Add("Name", "" + userInfo[1]);
            return View("OrderResponce");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Rock shop new order", "demirov_denis@ukr.net"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.ukr.net", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("demirov_denis@ukr.net", "zPvIcGIgtCZUcNfJ");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        private void FillCartFromCookies()
        {
            int userId;

            string[] userInfo;
            userInfo = HttpContext.User.Identity.Name.Split('|');
            userId = Convert.ToInt32(userInfo[0]);

            //Если хотябы первый товар в cookies
            if (HttpContext.Request.Cookies.Keys.Contains($"p{1}"))
            {
                string pr_id;
                int i = 1;
                var Orders = db_context.Orders.Where(itm => itm.UserId == userId);

                //перебор всех возможных товаров
                while (true)
                {
                    if (HttpContext.Request.Cookies.Keys.Contains($"p{i}"))
                    {
                        //если товара из cookies нет в базе, то он туда добавляется
                        HttpContext.Request.Cookies.TryGetValue($"p{i}", out pr_id);
                        if (Orders.Where(itm => itm.ProductId == Convert.ToInt32(pr_id)).FirstOrDefault() == null)
                        {

                            Order ord = new Order
                            {
                                User = db_context.Users.Find(userId),
                                UserId = userId,
                                dateTime = DateTime.Now,
                                ProductId = Convert.ToInt32(pr_id),
                                Product = db_context.Products.Find(Convert.ToInt32(pr_id))
                            };
                            db_context.Orders.Add(ord);
                            db_context.Users.Find(userId).Orders.Add(ord);
                            db_context.SaveChanges();

                            
                        }
                        i++;
                    }
                    else break;
                }

            }

        }

        private void DelAllItemsFromCookies()
        {
            if (HttpContext.Request.Cookies.Keys.Contains($"p{1}"))
            {
                int i = 1;
                while (true)
                {
                    if (HttpContext.Request.Cookies.Keys.Contains($"p{i}"))
                    {
                        HttpContext.Response.Cookies.Delete($"p{i}");
                        i++;
                    }
                    else break;
                }

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
