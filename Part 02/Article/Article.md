#### Part02 - begin

#### Introduction

#### Partial View vs. View Component

#### Our First View Component: Refactoring the Basket View

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\ViewComponents\BasketListViewComponent.cs

```csharp
﻿using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Collections.Generic;

namespace MVC.ViewComponents
{
    public class BasketListViewComponent : ViewComponent
    {
        public BasketListViewComponent()
        {
        }

        public IViewComponentResult Invoke(List<BasketItem> items)
        {
            return View("Default", items);
        }
    }
}
```
**Listing** : ViewComponents\BasketListViewComponent.cs file

Default.cshtml

```razor
﻿@using MVC.Controllers
@model List<BasketItem>;

@{
    var items = Model;
}

<partial name="_BasketList" for="@items" />
```
**Listing**: Components\BasketList\Default.cshtml file

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Views\Basket\Index.cshtml


```razor
<!--<partial name="_BasketList" for="@items" />-->
```
**Listing**: removing PartialTagHelper for BasketList


```razor
﻿@using MVC.Controllers
@addTagHelper *, MVC
.
.
.
<vc:basket-list items="@items"></vc:basket-list>
```
**Listing**: Using the generated BasketListViewComponentTagHelper

#### Moving BasketItem to ViewModels

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Controllers\BasketController.cs

```csharp
﻿namespace MVC.Models.ViewModels
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
```
**Listing**: Moving to BasketItem.cs to Models\ViewModels

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\ViewComponents\BasketListViewComponent.cs

```csharp
using MVC.Models.ViewModels;
```

Default.cshtml

```csharp
@using MVC.Models.ViewModels
```

/Views/Basket/Index.cshtml

```csharp
@using MVC.Models.ViewModels
```
_BasketItem.cshtml

```csharp
@using MVC.Models.ViewModels
```

#### Unit Testing Our View Component

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC.Test\MVC.Test.csproj

@@ -0,0 +1,20 @@
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>

    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Mvc.ViewFeatures" Version="2.2.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.9.0" />
    <PackageReference Include="xunit" Version="2.4.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\MVC\MVC.csproj" />
  </ItemGroup>

</Project>

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\Part02.sln

+ Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "MVC.Test", "MVC.Test\MVC.Test.csproj", "{79A50851-9596-44A7-A4A9-89321097E856}"
+ EndProject
+ 
+ {79A50851-9596-44A7-A4A9-89321097E856}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
+ {79A50851-9596-44A7-A4A9-89321097E856}.Debug|Any CPU.Build.0 = Debug|Any CPU
+ {79A50851-9596-44A7-A4A9-89321097E856}.Release|Any CPU.ActiveCfg = Release|Any CPU
+ {79A50851-9596-44A7-A4A9-89321097E856}.Release|Any CPU.Build.0 = Release|Any CPU

#### Basket Component With Items Should Display Default View

BasketListViewComponentTest.cs

@@ -0,0 +1,34 @@
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MVC.Models.ViewModels;
using MVC.ViewComponents;
using System;
using System.Collections.Generic;
using Xunit;

namespace MVC.Test
{
    public class BasketListViewComponentTest
    {
        [Fact]
        public void Invoke_With_Items_Should_Display_Default_View()
        {
            //arrange 
            var vc = new BasketListViewComponent();
            List<BasketItem> items =
            new List<BasketItem>
            {
                new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
                new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
                new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
            };

            //act 
            var result = vc.Invoke(items);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Default", vvcResult.ViewName);
        }
    }
}

#### Basket Component Without Items Should Display Empty View

BasketListViewComponentTest.cs

+[Fact]
+public void Invoke_With_Items_Should_Display_Empty_View()
+{
+    //arrange 
+    var vc = new BasketListViewComponent();
+    //act 
+    var result = vc.Invoke(new List<BasketItem>());
+
+    //assert
+    ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
+    Assert.Equal("Empty", vvcResult.ViewName);
+}

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\ViewComponents\BasketListViewComponent.cs

public IViewComponentResult Invoke(List<BasketItem> items)
{
    +if (items.Count == 0)
    +{
    +    return View("Empty");
    +}
    return View("Default", items);
}

Empty.cshtml

@@ -0,0 +1,15 @@
﻿@using MVC.Models.ViewModels
@model List<BasketItem>;

@{
    var items = Model;
}

<div class="card">
    <div class="card-body">
        <!--https://getbootstrap.com/docs/4.0/components/alerts/-->
        <div class="alert alert-warning" role="alert">
            There are no items in your basket yet! Click <a asp-controller="catalog"><b>here</b></a> to start shopping!
        </div>
    </div>
</div>

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Views\Basket\Index.cshtml

-new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
-new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
-new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
+//new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
+//new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
+//new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }


#### Creating a ViewComponent for BasketList

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\ViewComponents\BasketItemViewComponent.cs

@@ -0,0 +1,17 @@
﻿using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;

namespace MVC.ViewComponents
{
    public class BasketItemViewComponent : ViewComponent
    {
        public BasketItemViewComponent()
        {
        }

        public IViewComponentResult Invoke(BasketItem item)
        {
            return View("Default", item);
        }
    }
}

_BasketItem.cshtml => Default.cshtml

Default.cshtml

+@addTagHelper *, MVC


-<partial name="_BasketList" for="@items" />
+<div class="card">
+    <div class="card-header">
+        <div class="row">
+            <div class="col-sm-6">
+                Item
+            </div>
+            <div class="col-sm-2 text-center">
+                Unit Price
+            </div>
+            <div class="col-sm-2 text-center">
+                Quantity
+            </div>
+            <div class="col-sm-2">
+                <span class="pull-right">
+                    Subtotal
+                </span>
+            </div>
+        </div>
+    </div>
+    <div class="card-body">
+        @foreach (var item in items)
+        {
+            <vc:basket-item item="@item"></vc:basket-item>
+        }
+    </div>
+    <div class="card-footer">
+        <div class="row">
+            <div class="col-sm-10">
+                <span numero-items>
+                    Total: @items.Count
+                    item@(items.Count > 1 ? "s" : "")
+                </span>
+            </div>
+            <div class="col-sm-2">
+                Total: <span class="pull-right" total>
+                    @(items.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
+                </span>
+            </div>
+        </div>
+    </div>
+</div>

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Views\Basket\Index.cshtml

-//new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
-//new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
-//new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
+new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
+new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
+new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }

DELETE _BasketList.cshtml

#### Unit Testing BasketItemViewComponent

ADD BasketItemViewComponentTest.cs

@@ -0,0 +1,29 @@
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MVC.Models.ViewModels;
using MVC.ViewComponents;
using System;
using System.Collections.Generic;
using Xunit;

namespace MVC.Test
{
    public class BasketItemViewComponentTest
    {
        [Fact]
        public void Invoke_Should_Display_Default_View()
        {
            //arrange 
            var vc = new BasketItemViewComponent();
            BasketItem item =
                new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 };

            //act 
            var result = vc.Invoke(item);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Default", vvcResult.ViewName);
        }
    }
}

#### Moving Components to Views/Shared Folder

Part 02/MVC.Test/BasketItemViewComponentTest.cs

-var result = vc.Invoke(item);
+var result = vc.Invoke(item, false);

+BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
+Assert.Equal(item.ProductId, resultModel.ProductId);

+[Fact]
+public void Invoke_Should_Display_SummaryItem_View()
+{
+    //arrange 
+    var vc = new BasketItemViewComponent();
+    BasketItem item =
+        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 };
+
+    //act 
+    var result = vc.Invoke(item, true);
+
+    //assert
+    ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
+    Assert.Equal("SummaryItem", vvcResult.ViewName);
+    BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
+    Assert.Equal(item.ProductId, resultModel.ProductId);

Part 02/MVC.Test/BasketListViewComponentTest.cs

-var result = vc.Invoke(items);
+var result = vc.Invoke(items, false);

-var result = vc.Invoke(new List<BasketItem>());
+var result = vc.Invoke(new List<BasketItem>(), false);

Part 02/MVC/Models/ViewModels/BasketItemList.cs

+using System.Collections.Generic;
+
+namespace MVC.Models.ViewModels
+{
+    public class BasketItemList
+    {
+        public List<BasketItem> List { get; set; }
+        public bool IsSummary { get; set; }
+    }
+}

Part 02/MVC/ViewComponents/BasketItemViewComponent.cs

-public IViewComponentResult Invoke(BasketItem item)
+public IViewComponentResult Invoke(BasketItem item, bool isSummary)

+if (isSummary == true)
+{
+    return View("SummaryItem", item);
+}

Part 02/MVC/ViewComponents/BasketListViewComponent.cs

-public IViewComponentResult Invoke(List<BasketItem> items)
+public IViewComponentResult Invoke(List<BasketItem> items, bool isSummary)

-return View("Default", items);
+return View("Default", new BasketItemList
+{
+    List = items,
+    IsSummary = isSummary
+});

Part 02/MVC/Views/Basket/Index.cshtml

-{
-    new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
-    new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
-    new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
-};
+{
+    new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
+    new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
+    new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
+};

-<vc:basket-list items="@items"></vc:basket-list>
+<vc:basket-list items="@items" is-summary="false"></vc:basket-list>

Part 02/MVC/Views/Checkout/Index.cshtml

- @model string
- 
- @{ 
+ @using MVC.Models.ViewModels
+ @model string
+ @addTagHelper *, MVC
+ @{

+    List<BasketItem> items = new List<BasketItem>
+    {
+        new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
+        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
+        new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
+    };


+<h4>Summary</h4>
+
+<vc:basket-list items="@items" is-summary="true"></vc:basket-list>

...sket/Components/BasketItem/Default.cshtml → ...ared/Components/BasketItem/Default.cshtml

Part 02/MVC/Views/Shared/Components/BasketItem/SummaryItem.cshtml

@using MVC.Models.ViewModels

@model BasketItem

@{
    var item = Model;
}

<div class="row row-center">
    <div class="col-sm-2">@item.ProductId</div>
    <input type="hidden" name="productId" value="012" />
    <div class="col-sm-4">@item.Name</div>
    <div class="col-sm-2 text-center">@item.UnitPrice.ToString("C")</div>
    <div class="col-sm-2 text-center">@item.Quantity</div>
    <div class="col-sm-2">
        <div class="pull-right">
            <span class="pull-right" subtotal>
                @((item.Quantity * item.UnitPrice).ToString("C"))
            </span>
        </div>
    </div>
</div>
<br />

...sket/Components/BasketList/Default.cshtml → ...ared/Components/BasketList/Default.cshtml

- @model List<BasketItem>;
- 
- @{
-     var items = Model;
- }

+ @model BasketItemList;

-  @foreach (var item in items)
+  @foreach (var item in Model.List)
   {
-      <vc:basket-item item="@item"></vc:basket-item>
+      <vc:basket-item item="@item" is-summary="Model.IsSummary"></vc:basket-item>
   }

- Total: @items.Count
- item@(items.Count > 1 ? "s" : "")
+ Total: @Model.List.Count
+ item@(Model.List.Count > 1 ? "s" : "")

- @(items.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
+ @(Model.List.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))

...Basket/Components/BasketList/Empty.cshtml → ...Shared/Components/BasketList/Empty.cshtml

#### Fixing All Tests for IBasketService

Part 02/MVC.Test/BasketListViewComponentTest.cs

+ using Moq;
+ using MVC.Services;

- //arrange 
- var vc = new BasketListViewComponent();
+ //arrange
+ Mock<IBasketService> basketServiceMock =
+     new Mock<IBasketService>();

+ basketServiceMock.Setup(m => m.GetBasketItems())
+     .Returns(items);
+ var vc = new BasketListViewComponent(basketServiceMock.Object);

- var result = vc.Invoke(items, false);
+ var result = vc.Invoke(false);

- var vc = new BasketListViewComponent();
+ Mock<IBasketService> basketServiceMock =
+     new Mock<IBasketService>();
+ 
+ basketServiceMock.Setup(m => m.GetBasketItems())
+     .Returns(new List<BasketItem>());
+ var vc = new BasketListViewComponent(basketServiceMock.Object);

- var result = vc.Invoke(new List<BasketItem>(), false);
+ var result = vc.Invoke(false);

Part 02/MVC.Test/MVC.Test.csproj

+ <PackageReference Include="Moq" Version="4.10.1" />

Part 02/MVC/Services/BasketService.cs

using MVC.Models.ViewModels;
using System.Collections.Generic;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        public List<BasketItem> GetBasketItems()
        {
            return new List<BasketItem>
            {
                new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
                new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
                new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
            };
        }
    }
}

Part 02/MVC/Services/IBasketService.cs

using System.Collections.Generic;
using MVC.Models.ViewModels;

namespace MVC.Services
{
    public interface IBasketService
    {
        List<BasketItem> GetBasketItems();
    }
} 

Part 02/MVC/Startup.cs

+ using MVC.Services; 

+ services.AddTransient<IBasketService, BasketService>();

Part 02/MVC/ViewComponents/BasketListViewComponent.cs

+ using MVC.Services;

- public BasketListViewComponent()

+    private readonly IBasketService basketService;
+
+    public BasketListViewComponent(IBasketService basketService)
+    {
+        this.basketService = basketService;
+    }
+
+    public IViewComponentResult Invoke(List<BasketItem> items, bool isSummary)
+    public IViewComponentResult Invoke(bool isSummary)
+    {
+        List<BasketItem> items = basketService.GetBasketItems();

Part 02/MVC/Views/Basket/Index.cshtml

+ List<BasketItem> items = new List<BasketItem>
+ {
+     new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
+     new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
+     new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
+ };

- <vc:basket-list items="@items" is-summary="false"></vc:basket-list>
+ <vc:basket-list is-summary="false"></vc:basket-list>

Part 02/MVC/Views/Checkout/Index.cshtml

-    List<BasketItem> items = new List<BasketItem>
-    {
-        new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
-        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
-        new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
-    };

- <vc:basket-list items="@items" is-summary="true"></vc:basket-list>
+ <vc:basket-list is-summary="true"></vc:basket-list>

#### Asserting Collections

...2/MVC.Test/BasketItemViewComponentTest.cs → ...Components/BasketItemViewComponentTest.cs

- namespace MVC.Test
+ namespace MVC.Test.ViewComponents

...2/MVC.Test/BasketListViewComponentTest.cs → ...Components/BasketListViewComponentTest.cs

- using Microsoft.AspNetCore.Mvc;
- namespace MVC.Test
+ namespace MVC.Test.ViewComponents

+    var model = Assert.IsAssignableFrom<BasketItemList>(vvcResult.ViewData.Model);
+    Assert.Collection<BasketItem>(model.List,
+        i => Assert.Equal(1, i.ProductId),
+        i => Assert.Equal(5, i.ProductId),
+        i => Assert.Equal(9, i.ProductId)
+    );

#### Creating ViewComponent for Categories

Part 02/MVC/ViewComponents/CategoriesViewComponent.cs

+ using Microsoft.AspNetCore.Mvc;
+ using MVC.Models;
+ using System.Collections.Generic;
+ 
+ namespace MVC.ViewComponents
+ {
+     public class CategoriesViewComponent : ViewComponent
+     {
+         public CategoriesViewComponent()
+         {
+         }
+ 
+         public IViewComponentResult Invoke(List<Product> products)
+         {
+             return View("Default", products);
+         }
+     }
+ }

Part 02/MVC/Views/Catalog/_Categories.cshtml → ...alog/Components/Categories/Default.cshtml


Part 02/MVC/Views/Catalog/Index.cshtml

- @model List<Product>;
+ @addTagHelper *, MVC
+ @model List<Product>;

- <partial name="_Categories" for="@Model" />
+ <vc:categories products="@Model"></vc:categories>

#### Creating ViewComponent for ProductCard

Part 02/MVC/ViewComponents/ProductCardViewComponent.cs

+ using Microsoft.AspNetCore.Mvc;
+ using MVC.Models;
+ using System;
+ using System.Collections.Generic;
+ using System.Linq;
+ using System.Threading.Tasks;
+ 
+ namespace MVC.ViewComponents
+ {
+     public class ProductCardViewComponent : ViewComponent
+     {
+         public ProductCardViewComponent()
+         {
+ 
+         }
+ 
+         public IViewComponentResult Invoke(Product product)
+         {
+             return View("Default", product);
+         }
+     }
+ }

Part 02/MVC/Views/Catalog/Components/Categories/Default.cshtml

- @model List<Product>;

+ @addTagHelper *, MVC
+ @model List<Product>;

-             foreach (var productIndex in productsInPage)
+             foreach (var product in productsInPage)
            {
-                 <partial name="_ProductCard" for="@productIndex" />
+                 <vc:product-card product="@product"></vc:product-card>
            }

... 02/MVC/Views/Catalog/_ProductCard.cshtml → ...log/Components/ProductCard/Default.cshtml

#### Creating ViewComponents for Catalog

Part 02/MVC/Models/ViewModels/CarouselPageViewModel.cs

using System.Collections.Generic;

namespace MVC.Models.ViewModels
{
    public class CarouselPageViewModel
    {
        public CarouselPageViewModel()
        {

        }

        public CarouselPageViewModel(List<Product> products, int pageIndex)
        {
            Products = products;
            PageIndex = pageIndex;
        }

        public List<Product> Products { get; set; }
        public int PageIndex { get; set; }
    }
}

Part 02/MVC/Models/ViewModels/CarouselViewModel.cs

using System.Collections.Generic;

namespace MVC.Models.ViewModels
{
    public class CarouselViewModel
    {
        public CarouselViewModel()
        {

        }

        public CarouselViewModel(Category category, List<Product> products, int pageCount, int pageSize)
        {
            Category = category;
            Products = products;
            PageCount = pageCount;
            PageSize = pageSize;
        }

        public Category Category { get; set; }
        public List<Product> Products { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}

Part 02/MVC/Models/ViewModels/CategoriesViewModel.cs

using System.Collections.Generic;

namespace MVC.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel()
        {

        }

        public CategoriesViewModel(List<Category> categories, List<Product> products, int pageSize)
        {
            Categories = categories;
            Products = products;
            PageSize = pageSize;
        }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public int PageSize { get; set; }
    }
}

Part 02/MVC/ViewComponents/CarouselPageViewComponent.cs

using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MVC.ViewComponents
{
    public class CarouselPageViewComponent : ViewComponent
    {
        public CarouselPageViewComponent()
        {

        }

        public IViewComponentResult Invoke(List<Product> productsInCategory, int pageIndex, int pageSize)
        {
            var productsInPage =
                productsInCategory
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            return View("Default", 
                new CarouselPageViewModel(productsInPage, pageIndex));
        }
    }
}

Part 02/MVC/ViewComponents/CarouselViewComponent.cs

using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.ViewComponents
{
    public class CarouselViewComponent : ViewComponent
    {
        public CarouselViewComponent()
        {

        }

        public IViewComponentResult Invoke(Category category, List<Product> products, int pageSize)
        {
            var productsInCategory = products
                .Where(p => p.Category.Id == category.Id)
                .ToList();
            int pageCount = (int)Math.Ceiling((double)productsInCategory.Count() / pageSize);

            return View("Default", 
                new CarouselViewModel(category, productsInCategory, pageCount, pageSize));
        }
    }
}

Part 02/MVC/ViewComponents/CategoriesViewComponent.cs

+ using MVC.Models.ViewModels;
+ using System.Linq;

+         const int PageSize = 4;   

-             return View("Default", products);
+             var categories = products
+                 .Select(p => p.Category)
+                 .Distinct()
+                 .ToList();
+             return View("Default", new CategoriesViewModel(categories, products, PageSize));

Part 02/MVC/Views/Catalog/Components/Carousel/Default.cshtml

@using MVC.Models.ViewModels
@addTagHelper *, MVC
@model CarouselViewModel

<h3>@Model.Category.Name</h3>

<div id="carouselExampleIndicators-@Model.Category.Id" class="carousel slide" data-ride="carousel">
    <div class="carousel-inner">
        @{
            for (int pageIndex = 0; pageIndex < Model.PageCount; pageIndex++)
            {
                <vc:carousel-page products-in-category="@Model.Products"
                                  page-index="@pageIndex"
                                  page-size="@Model.PageSize">
                </vc:carousel-page>
            }
        }
    </div>
    <a class="carousel-control-prev" href="#carouselExampleIndicators-@Model.Category.Id" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
    </a>
    <a class="carousel-control-next" href="#carouselExampleIndicators-@Model.Category.Id" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
    </a>
</div>

Part 02/MVC/Views/Catalog/Components/CarouselPage/Default.cshtml

@using MVC.Models.ViewModels
@addTagHelper *, MVC
@model CarouselPageViewModel

<div class="carousel-item @(Model.PageIndex == 0 ? "active" : "")">
    <div class="container">
        <div class="row">
            @{
                foreach (var product in Model.Products)
                {
                    <vc:product-card product="@product"></vc:product-card>
                }
            }
        </div>
    </div>
</div> 

Part 02/MVC/Views/Catalog/Components/Categories/Default.cshtml

- @addTagHelper *, MVC
- @model List<Product>;
- @{
-     var products = Model;
-     const int PageSize = 4;
-     var categories = products.Select(p => p.Category).Distinct();
- }
+ @using MVC.Models.ViewModels
+ @addTagHelper *, MVC
+ @model CategoriesViewModel

- @foreach (var category in categories)
+ @foreach (var category in Model.Categories)

-    <h3>@category.Name</h3>
-
-    <div id="carouselExampleIndicators-@category.Id" class="carousel slide" data-ride="carousel">
-        <div class="carousel-inner">
-            @{
-                var productsInCategory = products
-                    .Where(p => p.Category.Id == category.Id);
-                int pageCount = (int)Math.Ceiling((double)productsInCategory.Count() / PageSize);
-
-                for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
-                {
-                    <div class="carousel-item @(pageIndex == 0 ? "active" : "")">
-                        <div class="container">
-                            <div class="row">
-                                @{
-                                    var productsInPage =
-                                        productsInCategory
-                                        .Skip(pageIndex * PageSize)
-                                        .Take(PageSize);
-
-                                    foreach (var product in productsInPage)
-                                    {
-                                        <vc:product-card product="@product"></vc:product-card>
-                                    }
-                                }
-                            </div>
-                        </div>
-                    </div>
-                }
-            }
-        </div>
-        <a class="carousel-control-prev" href="#carouselExampleIndicators-@category.Id" role="button" data-slide="prev">
-            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
-            <span class="sr-only">Previous</span>
-        </a>
-        <a class="carousel-control-next" href="#carouselExampleIndicators-@category.Id" role="button" data-slide="next">
-            <span class="carousel-control-next-icon" aria-hidden="true"></span>
-            <span class="sr-only">Next</span>
-        </a>
-    </div>

+ <vc:carousel category="@category" products="@Model.Products" page-size="@Model.PageSize"></vc:carousel>

#### Creating Navigation Bar Notification Icons

Part 02/MVC/Views/Shared/_Layout.cshtml

+    <div class="navbar-collapse collapse justify-content-end">
+        <ul class="nav navbar-nav">
+            <li>
+                <div class="container-notification">
+                    <a asp-controller="notifications"
+                        title="Notifications">
+                        <div class="user-count notification show-count fa fa-bell" data-count="2">
+                        </div>
+                    </a>
+                </div>
+            </li>
+            <li>
+                <span>
+                    &nbsp;
+                    &nbsp;
+                </span>
+            </li>
+            <li>
+                <div class="container-notification">
+                    <a asp-action="index" asp-controller="basket"
+                        title="Basket">
+                        <div class="user-count userbasket show-count fa fa-shopping-cart" data-count="3">
+                        </div>
+                    </a>
+                </div>
+            </li>
+        </ul>
+    </div>

Part 02/MVC/wwwroot/css/site.css

}

.user-count {
    display: inline-block;
    position: relative;
    padding: 0.08em;
    font-size: 1.2em;
}

.user-count::before,
.user-count::after {
    color: #000;
    text-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
}

.user-count::after {
    font-family: Arial;
    font-size: 0.7em;
    font-weight: 700;
    position: absolute;
    top: -10px;
    right: -10px;
    padding: 4px 6px;
    line-height: 100%;
    border-radius: 60px;
    background: #ffcc00;
    opacity: 0;
    content: attr(data-count);
    opacity: 0;
    -webkit-transform: scale(0.5);
    transform: scale(0.5);
    transition: transform, opacity;
    transition-duration: 0.3s;
    transition-timing-function: ease-out;
}

.user-count.show-count::after {
    -webkit-transform: scale(1);
    transform: scale(1);
    opacity: 1;
}


#### Creating UserCounter ViewComponent

Part 02/MVC/Models/ViewModels/UserCountViewModel.cs

+ namespace MVC.Models.ViewModels
+ {
+     public class UserCountViewModel
+     {   
+         public UserCountViewModel(string title, string controllerName, string cssClass, string icon, string count)
+         {
+             Title = title;
+             ControllerName = controllerName;
+             CssClass = cssClass;
+             Icon = icon;
+             Count = count;
+         }
+ 
+         public string ControllerName { get; set; }
+         public string Title { get; set; }
+         public string CssClass { get; set; }
+         public string Icon { get; set; }
+         public string Count { get; set; }
+     }
+ }

Part 02/MVC/ViewComponents/UserCounterViewComponent.cs

using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;

namespace MVC.ViewComponents
{
    public class UserCounterViewComponent : ViewComponent
    {
        public UserCounterViewComponent()
        {

        }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, string count)
        {
            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
            return View("Default", model);
        }
    }
}

Part 02/MVC/Views/Shared/Components/UserCounter/Default.cshtml

@using MVC.Models.ViewModels
@addTagHelper *, MVC
@model UserCountViewModel;

<div class="container-notification">
    <a asp-controller="@Model.ControllerName"
       title="@Model.Title">
        <div class="user-count @(Model.CssClass) show-count fa fa-@(Model.Icon)" count="@(Model.Count)">
        </div>
    </a>
</div> 

Part 02/MVC/Views/Shared/_Layout.cshtml

+ @addTagHelper *, MVC

-        <div class="container-notification">
-            <a asp-controller="notifications"
-                title="Notifications">
-                <div class="user-count notification show-count fa fa-bell" data-count="2">
-                </div>
-            </a>
-        </div>
+        <vc:user-counter 
+            title="Notifications"
+            controller-name="notifications"
+            css-class="notification"
+            icon="bell"
+            count="2">
+        </vc:user-counter>

-         <div class="container-notification">
-             <a asp-action="index" asp-controller="basket"
-                 title="Basket">
-                 <div class="user-count userbasket show-count fa fa-shopping-cart" data-count="3">
-                 </div>
-             </a>
-         </div>
+        <vc:user-counter 
+            title="Basket"
+            controller-name="basket"
+            css-class="basket"
+            icon="shopping-cart"
+            count="3">
+        </vc:user-counter>

Part 02/MVC/wwwroot/css/site.css

-    content: attr(data-count);
+    content: attr(count);

#### Creating UserCounterService

Part 02/MVC/Models/ViewModels/UserCountViewModel.cs

- public UserCountViewModel(string title, string controllerName, string cssClass, string icon, string count)
+ public UserCountViewModel(string title, string controllerName, string cssClass, string icon, int count)

- public string Count { get; set; }
+ public int Count { get; set; }


Part 02/MVC/Services/IUserCounterService.cs

namespace MVC.Services
{
    public interface IUserCounterService
    {
        int GetBasketCount();
        int GetNotificationCount();
    }
}

Part 02/MVC/Services/UserCounterService.cs

namespace MVC.Services
{
    public class UserCounterService : IUserCounterService
    {
        public int GetNotificationCount()
        {
            return 7;
        }

        public int GetBasketCount()
        {
            return 9;
        }
    }
}

Part 02/MVC/Startup.cs

+    var userCounterServiceInstance = new UserCounterService();
+    services.AddSingleton<IUserCounterService>(userCounterServiceInstance);

Part 02/MVC/ViewComponents/UserCounterViewComponent.cs

+ using MVC.Services;

namespace MVC.ViewComponents
{
+    public enum UserCounterType
+    {
+        Notification = 1,
+        Basket = 2
+    }

-         public UserCounterViewComponent()
-         {
+         protected readonly IUserCounterService userCounterService;
+ 
+         public UserCounterViewComponent(IUserCounterService userCounterService)
+         {
+             this.userCounterService = userCounterService;
+         }

-        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, string count)
+        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, UserCounterType userCounterType)
+        {
+            int count = 0;
+
+            if (userCounterType == UserCounterType.Notification)
+            {
+                count = userCounterService.GetNotificationCount();
+            }
+            else if (userCounterType == UserCounterType.Basket)
+            {
+                count = userCounterService.GetBasketCount();
+            }

Part 02/MVC/Views/Shared/_Layout.cshtml

-         count="2">
+         user-counter-type="Notification">

-         count="3">
+         user-counter-type="Basket">

#### Creating NotificationCounter, BasketCounter Subclasses

Part 02/MVC/ViewComponents/UserCounterViewComponent.cs


-    public enum UserCounterType
-    {
-        Notification = 1,
-        Basket = 2
-    }
-
-    public class UserCounterViewComponent : ViewComponent
+    public abstract class UserCounterViewComponent : ViewComponent
+    {
+        protected enum UserCounterType
+        {
+            Notification = 1,
+            Basket = 2
+        }

-        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, UserCounterType userCounterType)
+        protected IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, int count, UserCounterType userCounterType)
+        {
-            int count = 0;
+            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
+            return View("~/Views/Shared/Components/UserCounter/Default.cshtml", model);
+        }
+    }

-    if (userCounterType == UserCounterType.Notification)
-    {
-        count = userCounterService.GetNotificationCount();
-    }
-    else if (userCounterType == UserCounterType.Basket)
-    {
-        count = userCounterService.GetBasketCount();
-    }
+    public class NotificationCounterViewComponent : UserCounterViewComponent
+    {
+        public NotificationCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }

-            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
-            return View("Default", model);
+        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
+        {
+            int count = userCounterService.GetNotificationCount();
+            return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Notification);
+        }
+    }
+
+    public class BasketCounterViewComponent : UserCounterViewComponent
+    {
+        public BasketCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }
+
+        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
+        {
+            int count = userCounterService.GetBasketCount();
+            return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Basket);

Part 02/MVC/Views/Shared/_Layout.cshtml

- <vc:user-counter 
+ <vc:notification-counter 

-        icon="bell"
-        user-counter-type="Notification">
-    </vc:user-counter>
+        icon="bell">
+    </vc:notification-counter>

-    <vc:user-counter 
+    <vc:basket-counter 
        title="Basket"
        controller-name="basket"
        css-class="basket"
-        icon="shopping-cart"
-        user-counter-type="Basket">
-    </vc:user-counter>
+        icon="shopping-cart">
+    </vc:basket-counter>    







