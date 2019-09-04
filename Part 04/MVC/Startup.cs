using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC.Areas.Basket.Data;
using MVC.Areas.Catalog.Data;
using MVC.Areas.Catalog.Data.Repositories;
using MVC.Areas.Checkout.Data;
using MVC.Areas.Notification.Services;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

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

            services.AddMvc(options =>
                options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(a => a.SerializerSettings.ContractResolver 
                    = new DefaultContractResolver());

            services.AddDistributedMemoryCache();
            services.AddSession();

            ConfigureContext<CatalogDbContext>(services, "CatalogContextConnection");
            ConfigureContext<CheckoutDbContext>(services, "CheckoutContextConnection");

            ConfigureRedis(services);
            ConfigureDI(services);
        }

        private void ConfigureRedis(IServiceCollection services)
        {
            //By connecting here we are making sure that our service
            //cannot start until redis is ready. This might slow down startup,
            //but given that there is a delay on resolving the ip address
            //and then creating the connection it seems reasonable to move
            //that cost to startup instead of having the first request pay the
            //penalty.
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("RedisConnectionString"), true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            services.AddTransient<IBasketRepository, RedisBasketRepository>();
            services.AddTransient<ICheckoutRepository, CheckoutRepository>();
            services.AddTransient<IProductService, ProductService>();
            //services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddSingleton<IProductRepository, ElasticProductRepository>();
            var userCounterServiceInstance = new UserCounterService();
            services.AddSingleton<IUserCounterService>(userCounterServiceInstance);
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
            InitializeProductRepository<IProductRepository>(app);
            //MigrateDatabase<CatalogDbContext>(app);
            MigrateDatabase<CheckoutDbContext>(app);

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
                    template: "Catalog/{controller=Home}/{action=Index}/{searchText?}");

                routes.MapAreaRoute(
                    name: "AreaBasket",
                    areaName: "Basket",
                    template: "Basket/{controller=Home}/{action=Index}/{code?}");

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
                   template: "{controller=Home}/{action=Index}/{id?}");
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

        private static void InitializeProductRepository<T>(IApplicationBuilder app) where T : IProductRepository
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var repository = serviceScope.ServiceProvider.GetService<T>();
                repository.Initialize();
            }
        }
    }
}
