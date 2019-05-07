### CAROUSEL - PUT THIS COMMIT FIRST!

Part 03/MVC/wwwroot/css/site.css

ADD

.carousel-control-prev
,.carousel-control-next {
    width: 32px;
}

.carousel-control-prev {
    margin-left: -32px;
}

.carousel-control-next {
    margin-right: -32px;
}

REMOVE

    margin-left: -200px;
.
.
.
    margin-right: -200px;


### Identity added

At the end of part 2 of this sequence of courses, we had an e-commerce application in which the user chose products from a catalog (carousel of products), placed them in his shopping cart, and filled out a register with his address and personal data for delivery. However, access to all screens is open to any anonymous user who has access to the website, which causes us to have some problems:

- The sensitive points in our application (such as the cart and checkout screen) are vulnerable to hackers, robots and other types of malicious software that can forge false requests or steal information
- Our application does not know who is currently accessing, so it is not possible to provide certain conveniences, such as automatically filling the registration for a user who has previously purchased the website.
- Requests currently have the user's cadastral data, but nothing prevents a user from making purchases by filling in someone else's data. Thus, the application does not know if the data is being filled by an appropriate person.

In this part 3 of the course, we will use a login system and ensure that our application is accessed only by authenticated users. This will allow you to protect sensitive points of the application from accessing anonymous users. With authentication, we will ensure that the user entered the system through a secure identification service. This will also enable the application to further track user access, identify usage patterns, automatically fill out registration forms, view customer order history, and other conveniences that enhance the user experience in the application.

If you only need a user table with password login features and a user profile, then ASP.NET Core Identity is the best option for you.
If you only need a user table with login and password features and a user profile for your application, then ASP.NET Core Identity is the best option for you.
In this chapter we will learn how to install ASP.NET Core Identity in our e-commerce solution and take advantage of the security, login / logout, authentication, and user profile features provided by this framework.
The installation of ASP.NET Core Identity in a preexisting project that did not have Identity since the beginning is different from the installation in which a new ASP.NET Core project already comes with Identity since its inception. To install in our project, which does not have Identity, we will follow a process in which the installation adds to our project a group of ready-made files (similar to prefabricated modules that are seen in construction), and this process called "assembly scaffolding "or" Scaffolding "in English.
But before we install the ASP.NET Core Identity packages themselves, it is necessary to make some temporary adaptations in our project, otherwise the installation of ASP.NET Core Identity packages will not be successful.

Below, we will install the NuGet package for the SQLite database management engine.

Right-click the MVC project name, choose the Add NuGet Package submenu, and when you open the package installation page, enter the package name: Microsoft.EntityFrameworkCore.SQLite

Microsoft.EntityFrameworkCore.Sqlite

![Add Nuget Package](add_nuget_package.png)

![Sqlite](Microsoft.EntityFrameworkCore.Sqlite.png)

Now click the "Install" button and wait for the package to install.

Okay, now the project is ready to receive the Scaffold of ASP.NET Core Identity.

### Applying the ASP.NET Core Identity Scaffold

Manually create login / logout, authentication, etc. features. in our application would require a lot of effort, both for the development of views and for creating business logic, model entities, data access, security, etc., in addition to many hours of unit testing, functional tests, integrated testing, etc.

Fortunately, we can use the benefits of authentication and authorization features in our application without much effort. Because this type of functional requirement is virtually universal in web applications, Microsoft provides a package of files that can be transparently installed in ASP.NET Core projects that do not yet have authentication / authorization. It's called ASP.NET Core Identity.

To apply ASP.NET Core Identity in our solution, we need to right-click on the name of the MVC project, and choose the Add Scaffolded Item menu option and then choose Add, which will open a new window dialog called "Add Scaffold".

![New Scaffolding Item](new_scaffolding_item.png)

![Add Scaffold](add_scaffold.png)

Here, we will choose the Installed> Identity> Identity option.

AppIdentityContext, AppIdentityUser

The ASP.NET Core Identity Scaffold will open a new dialog window, containing a series of configuration parameters, which allow you to define the layout of the pages, what features are included, data and user context classes, and also which type of database (SQL Server or SQLite).

![Add Identity](add_identity.png)

Here we choose:

- Layout: The _Layout.cshtml file already exists in our project. This will cause the new login, logout, etc. pages. have the same standardized "face" of our MVC application.
- Identity pages: Login, Logout, Register, ExternalLogin. These pages are sufficient for our application. However, note how much more functionality can be added
- context class: AppIdentityContext: This class will be created in the confirmation window.
- user class: AppIdentityUser. This class will be created in the confirmation window.

Once we confirm these parameters for ASP.NET Core Identity scaffold, our project is modified, and the most notable change is the creation of a folder and file structure under the Areas / Identity folder of our project.

![Identity Area](identity_area.png)

What can we identify in these files / folders above?

- The new AppIdentityContext class: It defines the context with the entities and relationships required to configure the Entity Framework Core.
- The new AppIdentityUser class: it contains the user profile data used in the ASPNET Core Identity authentication and authorization process.
- The pages below Pages / Account: are pages containing the markup (HTML) for login, logout, etc. of Identity. They are "Razor Pages", that is, an MVC structure type where the view is in the .cshtml file and the actions of the controller and the template are in a single file .cshtml.cs
- Partial Auxiliary Views
- IdentityHostingStartup class: This class is executed by the WebHost of our application, which is declared in the Program.cs file. The IdentityHostingStartup class configures the database and other service types that ASP.NET Identity Core needs to work.

### Adding ASP.NET Core Identity Model Migration

Do not just add the ASP.NET Identity Core files in our project. We will still have to generate the database schema, so that the tables and initial data for ASP.NET Identity Core required configuration are also created.

When we did the scaffolding of ASP.NET Identity Core, we added a new Identity data model to our project, as we can see in the IdentityHostingStartup.cs file class:

```csharp
public void Configure(IWebHostBuilder builder)
{
    builder.ConfigureServices((context, services) => {
        services.AddDbContext<AppIdentityContext>(options =>
            options.UseSqlite(
                context.Configuration.GetConnectionString("AppIdentityContextConnection")));

        services.AddDefaultIdentity<AppIdentityUser>()
            .AddEntityFrameworkStores<AppIdentityContext>();
    });
}
```

appsettings.json

    "AllowedHosts": "*",
    "ConnectionStrings": {
    "AppIdentityContextConnection": "DataSource=MVC.db"
    }

PM> Add-Migration Identity

![Add Migration](add_migration.png)

PM> Update-Database

Startup.cs

    app.UseStaticFiles();
    app.UseAuthentication();

![File Loginpartial](file_loginpartial.png)

_Layout.cshtml

    <div class="navbar-collapse collapse justify-content-end">
        <partial name="_LoginPartial" />
        <ul class="nav navbar-nav">

![Register Link](register_link.png)

![Identity Account Register](identity_account_register.png)

![Login Link](login_link.png)

![Identity Account Login](identity_account_login.png)

![Register Login And Icons](register_login_and_icons.png)

_Layout.cshtml

    @if (User.Identity.IsAuthenticated)
    {
        <ul class="nav navbar-nav">
            <li>
                <vc:notification-counter title="Notifications"...

                <vc:basket-counter title="Basket"...

            </li>
        </ul>
    }

![Register Login No Icons](register_login_no_icons.png)

![Click User Name](click_user_name.png)

![Account Manage](account_manage.png)

![Change Password](change_password.png)

![Account Manage Twofactorauthentication](account_manage_twofactorauthentication.png)

![Account Manage Personaldata](account_manage_personaldata.png)



![Mvc Db](mvc_db.png)





### Identity Core

![Basket Nonauthorized](basket_nonauthorized.png)







Part 03/MVC/Controllers/BasketController.cs

REMOVE

using Microsoft.AspNetCore.Mvc;

ADD

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

 [Authorize]


Part 03/MVC/Controllers/CheckoutController.cs

REMOVE

using Microsoft.AspNetCore.Mvc;

ADD

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

 [Authorize]

Part 03/MVC/Controllers/NotificationsController.cs

REMOVE

using Microsoft.AspNetCore.Mvc;

ADD

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

  [Authorize]

Part 03/MVC/Controllers/RegistrationController.cs

ADD 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Identity.Data;
using MVC.Models.ViewModels;
using MVC.Services;

[Authorize]

![Add To Basket Nonauthorized](add_to_basket_nonauthorized.png)

https://localhost:44340/Identity/Account/Login?ReturnUrl=%2FBasket

![Returnurl](returnurl.png)

Part 03/MVC/MVC.csproj

<PackageReference Include="IdentityModel" Version="3.10.10" />

CREATED

Part 03/MVC/MVC.db
Part 03/MVC/Migrations/20190503043129_UserProfileData.cs
Part 03/MVC/Migrations/AppIdentityContextModelSnapshot.cs

ADDED

Part 03/MVC/Models/ViewModels/RegistrationViewModel.cs

namespace MVC.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AdditionalAddress { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}

Part 03/MVC/Services/IIdentityParser.cs

using System.Security.Principal;

namespace MVC.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}

Part 03/MVC/Services/IdentityParser.cs

using System.Security.Principal;

namespace MVC.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}

Part 03/MVC/Services/IdentityParser.cs

using MVC.Areas.Identity.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MVC.Services
{
    public class IdentityParser : IIdentityParser<AppIdentityUser>
    {
        public AppIdentityUser Parse(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                return new AppIdentityUser
                {
                    Name = claims.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "",
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "",
                    Phone = claims.Claims.FirstOrDefault(x => x.Type == "phone")?.Value ?? "",
                    Address = claims.Claims.FirstOrDefault(x => x.Type == "address")?.Value ?? "",
                    AdditionalAddress = claims.Claims.FirstOrDefault(x => x.Type == "address_details")?.Value ?? "",
                    District = claims.Claims.FirstOrDefault(x => x.Type == "district")?.Value ?? "",
                    City = claims.Claims.FirstOrDefault(x => x.Type == "city")?.Value ?? "",
                    State = claims.Claims.FirstOrDefault(x => x.Type == "state")?.Value ?? "",
                    ZipCode = claims.Claims.FirstOrDefault(x => x.Type == "zip_code")?.Value ?? ""
                };
            }
            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}

Part 03/MVC/Startup.cs

using IdentityModel;


using Microsoft.AspNetCore.Identity;

using MVC.Areas.Identity.Data;


services.AddTransient<IIdentityParser<AppIdentityUser>, IdentityParser>();

services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
    options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
});


Part 03/MVC/Views/Registration/Index.cshtml

@using MVC.Models.ViewModels
@model RegistrationViewModel
@{


        //<input type="text" class="form-control" />
        <input type="text" class="form-control" value="@Model.Name" />

Part 03/MVC/Areas/Identity/Data/AppIdentityUser.cs

ADD

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AdditionalAddress { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }


Part 03/MVC/Controllers/BaseController.cs

ADD

        protected string GetUserId()
        {
            return @User.FindFirst("sub")?.Value;
        }

### AddMicrosoftAccount

Part 03/MVC/Startup.cs

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using IdentityModel;
using IdentityModel;

//using Microsoft.AspNetCore.HttpsPolicy;

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication_Microsoft_ApplicationId"];
                microsoftOptions.ClientSecret = Configuration["Authentication_Microsoft_Password"];
            });


Part 03/MVC/appsettings.json

//  }
  },
  "Authentication_Microsoft_ApplicationId": "365218f7-1110-4d12-ad00-2472000b3219",
  "Authentication_Microsoft_Password": "wutBVKOC4[lgpwLC4147$-("

### Persisting User Data to Session

Part 03/MVC/Controllers/BasketController.cs

//var clientId = GetUserId();

Part 03/MVC/Controllers/CheckoutController.cs

    public class CheckoutController : BaseController
    {
        //public IActionResult Index()
        private readonly IHttpHelper httpHelper;

        public CheckoutController(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistrationViewModel registration)
        {
            httpHelper.SetRegistration(GetUserId(), registration);
            return View();
        }



Part 03/MVC/Controllers/HttpHelper.cs

using Microsoft.AspNetCore.Http;
using MVC.Models.ViewModels;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpContextAccessor contextAccessor;

        public HttpHelper(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public void SetRegistration(string clientId, RegistrationViewModel registrationViewModel)
        {
            string json = JsonConvert.SerializeObject(registrationViewModel.GetClone());
            contextAccessor.HttpContext.Session.SetString($"registration_{clientId}", json);
        }

        public RegistrationViewModel GetRegistration(string clientId)
        {
            string json = contextAccessor.HttpContext.Session.GetString($"registration_{clientId}");
            if (string.IsNullOrWhiteSpace(json))
                return new RegistrationViewModel();

            return JsonConvert.DeserializeObject<RegistrationViewModel>(json);
        }
    }
}

Part 03/MVC/Controllers/IHttpHelper.cs

using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public interface IHttpHelper
    {
        RegistrationViewModel GetRegistration(string clientId);
        void SetRegistration(string clientId, RegistrationViewModel registrationViewModel);
    }
} 

Part 03/MVC/Controllers/RegistrationController.cs

    public class RegistrationController : BaseController
    {
        //private readonly IIdentityParser<AppIdentityUser> appUserParser;
        private readonly IHttpHelper httpHelper;

        //public RegistrationController(IIdentityParser<AppIdentityUser> appUserParser)
        public RegistrationController(IHttpHelper httpHelper)
        {
            //this.appUserParser = appUserParser;
            this.httpHelper = httpHelper;
        }

        public IActionResult Index()
        {
 //           var usuario = appUserParser.Parse(HttpContext.User);
 //           var viewModel = new RegistrationViewModel
 //           {
 //               District = usuario.District,
 //               ZipCode = usuario.ZipCode,
 //               AdditionalAddress = usuario.AdditionalAddress,
 //               Email = usuario.Email,
 //               Address = usuario.Address,
 //               City = usuario.City,
 //               Name = usuario.Name,
 //               Phone = usuario.Phone,
 //               State = usuario.State
 //           };
            var viewModel = httpHelper.GetRegistration(GetUserId());

Part 03/MVC/Models/ViewModels/RegistrationViewModel.cs

        public RegistrationViewModel()
        {

        }

        public RegistrationViewModel(string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            AdditionalAddress = additionalAddress;
            District = district;
            City = city;
            State = state;
            ZipCode = zipCode;
        }


        public RegistrationViewModel GetClone()
        {
            return new RegistrationViewModel(this.Name, this.Email, this.Phone, this.Address, this.AdditionalAddress, this.District, this.City, this.State, this.ZipCode);
        }


Part 03/MVC/Startup.cs

            services.AddDistributedMemoryCache();
            services.AddSession();
.
.
.

            services.AddTransient<IHttpHelper, HttpHelper>();
.
.
.
            app.UseSession();

Part 03/MVC/Views/Registration/Index.cshtml

//<form method="post" action="checkout">
<form method="post" asp-controller="checkout" asp-action="index">

//    <input type="text" class="form-control" value="@Model.Name" />
    <input class="form-control" asp-for="@Model.Name" />

    //<label class="control-label">Email</label>
    //<input type="email" class="form-control" />
    <label class="control-label" for="email">Email</label>
    <input type="email" class="form-control" id="email" asp-for="@Model.Email">
    <span asp-validation-for="@Model.Email" class="text-danger"></span>

//<input type="text" class="form-control" />
<input class="form-control" asp-for="@Model.Phone" />

    //<input type="text" class="form-control" />
    <input class="form-control" asp-for="@Model.Address" />

    //<input type="text" class="form-control" />
    <input class="form-control" asp-for="@Model.AdditionalAddress" />

    //<input type="text" class="form-control" />
    <input class="form-control" asp-for="@Model.District" />

    //<input type="text" class="form-control" />
    <input class="form-control" asp-for="@Model.City" />

    //<input type="text" class="form-control" />
    <input class="form-control" asp-for="@Model.State" />

    //<input type="text" class="form-control" />
    <input  class="form-control" asp-for="@Model.ZipCode" />

### Form validation

Part 03/MVC/Controllers/CheckoutController.cs

//    httpHelper.SetRegistration(GetUserId(), registration);
//    return View();
    if (ModelState.IsValid)
    {
        httpHelper.SetRegistration(GetUserId(), registration);
        return View(registration);
    }
    return RedirectToAction("Index", "Registration");


Part 03/MVC/Controllers/HttpHelper.cs

//    return new RegistrationViewModel();
    return new RegistrationViewModel(clientId);

Part 03/MVC/Models/ViewModels/RegistrationViewModel.cs

//    public RegistrationViewModel(string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
    public RegistrationViewModel(string userId)
    {
        UserId = userId;
    }

    public RegistrationViewModel(string userId, string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
    {
        UserId = userId;


[Required]
[Required]
[Required]
[Required]...


Part 03/MVC/Views/Checkout/Index.cshtml

//@model string
@model RegistrationViewModel

    <p>Thank you very much, <b>@Model.Name</b>!</p>
    <p>Your order has been placed.</p>
//    <p>Soon you will receive an e-mail at <b>@email</b> including all order details.</p>
    <p>Soon you will receive an e-mail at <b>@Model.Email</b> including all order details.</p>


Part 03/MVC/Views/Registration/Index.cshtml

<input type="hidden" asp-for="@Model.UserId" />

//                        <label class="control-label">Customer Name</label>
//                        <input class="form-control" asp-for="@Model.Name" />
                        <label class="control-label" for="name">Customer Name</label>
                        <input type="text" class="form-control" id="name" asp-for="@Model.Name" />

//                        <label class="control-label">Phone</label>
//                        <input class="form-control" asp-for="@Model.Phone" />
                        <label class="control-label" for="phone">Phone</label>
                        <input type="text" class="form-control" id="phone" asp-for="@Model.Phone" />
                        <span asp-validation-for="@Model.Phone" class="text-danger"></span>
                        <span asp-validation-for="@Model.Name" class="text-danger"></span>

@section Scripts
{
    <partial name="~/Views/Shared/_ValidationScriptsPartial.cshtml"/>
}

### Obtaining email from identity

Part 03/MVC/Controllers/BaseController.cs

        protected string GetUserId()
        {
            //return @User.FindFirst("sub")?.Value;
            return @User.FindFirst(JwtClaimTypes.Subject)?.Value;
        }

        protected string GetUserEmail()
        {
            return @User.FindFirst(JwtClaimTypes.Name)?.Value;
        }


Part 03/MVC/Controllers/HttpHelper.cs

        //public RegistrationViewModel GetRegistration(string clientId)
        public RegistrationViewModel GetRegistration(string clientId, string email)
        {
            string json = contextAccessor.HttpContext.Session.GetString($"registration_{clientId}");
            if (string.IsNullOrWhiteSpace(json))
                //return new RegistrationViewModel(clientId);
                return new RegistrationViewModel(clientId, email);

Part 03/MVC/Controllers/IHttpHelper.cs

        //RegistrationViewModel GetRegistration(string clientId);
        RegistrationViewModel GetRegistration(string clientId, string email);

Part 03/MVC/Controllers/RegistrationController.cs

            //var viewModel = httpHelper.GetRegistration(GetUserId());
            var viewModel = httpHelper.GetRegistration(GetUserId(), GetUserEmail());

Part 03/MVC/Models/ViewModels/RegistrationViewModel.cs

        public RegistrationViewModel(string userId, string email) : this(userId)
        {
            Email = email;
        }

        public RegistrationViewModel(string userId, string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
            : this(userId, email)
        {
            //UserId = userId;
            Name = name;
            //Email = email;

