using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC.Areas.Basket.Services;
using MVC.Areas.Catalog;
using MVC.Areas.Catalog.Data;
using MVC.Areas.Catalog.Data.Repositories;
using MVC.Areas.Notification.Services;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            //SetupAutoMapper(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDistributedMemoryCache();
            services.AddSession();

            ConfigureContext<CatalogDbContext>(services, "CatalogContextConnection");

            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            var userCounterServiceInstance = new UserCounterService();
            services.AddSingleton<IUserCounterService>(userCounterServiceInstance);

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("CatalogContextConnection"));
            });
        }

        private void ConfigureContext<T>(IServiceCollection services, string connectionName) where T : DbContext
        {
            string connectionString = Configuration.GetConnectionString(connectionName);

            services.AddDbContext<T>(options =>
                options.UseSqlServer(connectionString)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MigrateDatabase<CatalogDbContext>(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: "AreaCatalog",
                    areaName: "Catalog",
                    template: "Catalog/{controller=Product}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "AreaBasket",
                    areaName: "Basket",
                    template: "Basket/{controller=Basket}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "AreaRegistration",
                    areaName: "Registration",
                    template: "Registration/{controller=Registration}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "AreaCheckout",
                    areaName: "Checkout",
                    template: "Checkout/{controller=Checkout}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "AreaNotification",
                    areaName: "Notification",
                    template: "Notification/{controller=Notification}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "default",
                   template: "Catalog/{controller=Product}/{action=Index}/{id?}");
            });
        }

        private static void MigrateDatabase<T>(IApplicationBuilder app) where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<T>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
