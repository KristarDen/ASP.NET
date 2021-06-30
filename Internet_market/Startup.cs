using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_market.Models;

namespace Internet_market
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public List<Product> Products = new List<Product>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            Product P1 = new Product();
            P1.id = 1;
            P1.Name = "Футболка Powerwolf \"Metal Is Religion\"";
            P1.Price = 450;
            P1.Type = "T-Short";
            P1.Description = "Футболка с полной запечаткой, детализированым рисунком и слоганом группы Powerwolf - \"Metal is religion\" ";
            P1.Color = "Серый";
            P1.Size = "XL";
            P1.Material = "100% хлопок";
            P1.Photos.Add("PW_t-short1.jpg");
            P1.Photos.Add("PW_t-short2.jpg");
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
            P2.Photos.Add("PW_hat1.jpg");
            P2.Photos.Add("PW_hat2.jpg");
            P2.Photos.Add("PW_hat3.jpg");
            Products.Add(P2);

        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
