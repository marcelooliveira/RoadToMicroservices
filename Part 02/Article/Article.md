#### Introduction

Welcome to the second installment of the "**ASP.NET Core Roadmap to Microservices**" article series.

In the last article, we saw how to build the basic views of the e-commerce application, using views and Partial Views. Today we will explore the subject of **view components** in ASP.NET CORE.

What are view components? How they compare to **partial views**? And how they apply in our e-commerce project?

#### Partial View vs. View Component
 
The partial views we introduced in the previous article are capable enough to perform the
role of view composition for the e-commerce application.

We have seen how partial views allow us to break up large markup files into smaller components,
and reduce the duplication of common markup content across markup files.

View Component is a concept introduced by ASP.NET Core, and is similar to a partial view.
While view components are as capable as partial views in decomposing large views and reducing duplication,
but they're also built differently, and are much more powerful.

Partial views, just like regular views, use model binding, that is, the model data which is provided by
a specific controller action. View components, on the other hand, only depend on the data provided to them as real parameters.

Although we are implementing the view components in an e-commerce application, which based on controllers and views, 
it is also possible to develop view components for Razor Pages.

#### Replacing Basket Partial Views with View Components

In the previous article, we broke the Basket view into smaller partial views, as we can see in the folder structure below:

![Basket Partial Views](basket_partial_views.png)

**Picture**: basket-related partial views

Each one of these markup files is responsible for rendering a different layer of elements inside the basket view:

- Basket/Index (View)
    - Basket Controls (Partial View)
    - Basket List (Partial View)
    - Basket Item (Partial View)

```razor
<partial name="_BasketControls" />

<h3>My Basket</h3>

<partial name="_BasketList" for="@items" />

<br />

<partial name="_BasketControls" />
```
**Listing**: an example of how to use partial views (\Views\Basket\Index.cshtml)

But partial views are somehow limited, and don't allow some interesting features that
we can find in View Components, such:

- Behavior independent from the hosting view
- Separation of concerns similar to controller/views
- Parameters
- Business logic
- Testability

But these nice features also means we have a little more work to do in order to create view components.
Besides the markup file, we also have to create a dedicated class for the view component.
But this must reside in a **ViewComponents** folder, which we must create first.

Now let's create a class named BasketListViewComponent inside the **ViewComponents** folder.

This class just need to have an Invoke() method calling and returning the "Default" view:

```csharp
public class BasketListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Default");
    }
}
```
**Listing** : the ViewComponents\BasketListViewComponent.cs file

But notice how our previous Basket List partial view had an attribute for the model:

```razor
<partial name="_BasketList" for="@items" />
```

This @items attribute will now be passed to the new BasketListViewComponent through an items parameter
in the Invoke method of the BasketListViewComponent class, and it is then passed as a model
for the Default markup file:

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\ViewComponents\BasketListViewComponent.cs

```csharp
public class BasketListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<BasketItem> items)
    {
        return View("Default", items);
    }
}
```
**Listing** : the ViewComponents\BasketListViewComponent.cs file


By default, view component class names must have the -ViewComponent. But you can override this rule
by using the ViewComponentAttribute and setting the name of the component
(Notice that this allows you to use any class name you want).

```csharp
[ViewComponent(Name = "BasketList")]
public class BasketList : ViewComponent
{
    public IViewComponentResult Invoke(List<BasketItem> items)
    {
        return View("Default", items);
    }
}
```
**Listing** : using attribute to set the view component name

Now let's create the markup (view) file for the component. First, we have to create
a \Components folder under the \Views\Basket folder, and then 
a \BasketList folder under the \Components folder. Then we create the Default.cshtml file
(which is by the way the default name for any component), which looks exactly like a regular
view file. Add a new MVC view (scaffolding) with no template, no model and no layout:

![New Mvc View](new_mvc_view.png)

**Picture**: Adding a new view component view

```razor
@{
    Layout = null;
}

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Default</title>
</head>
<body>
</body>
</html>
```
**Listing**: \Views\Components\BasketList\Default.cshtml file

Notice that the new BasketList View Component is meant to replace the current BasketList partial view.
Therefore, we will overwrite the contents of the former with the contents of the latter:

```razor
@using MVC.Controllers
@model List<BasketItem>;

@{
    var items = Model;
}

<div class="card">
    <div class="card-header">
        <div class="row">
            <div class="col-sm-6">
                Item
            </div>
            <div class="col-sm-2 text-center">
                Unit Price
            </div>
            <div class="col-sm-2 text-center">
                Quantity
            </div>
            <div class="col-sm-2">
                <span class="pull-right">
                    Subtotal
                </span>
            </div>
        </div>
    </div>
    <div class="card-body">
        @foreach (var item in items)
        {
            <partial name="_BasketItem" for="@item" />
        }
    </div>
    <div class="card-footer">
        <div class="row">
            <div class="col-sm-10">
                <span numero-items>
                    Total: @items.Count
                    item@(items.Count > 1 ? "s" : "")
                </span>
            </div>
            <div class="col-sm-2">
                Total: <span class="pull-right" total>
                    @(items.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
                </span>
            </div>
        </div>
    </div>
</div>
```
**Listing**: BasketList View Component with the contents of the BasketList partial view (\Views\Components\BasketList\Default.cshtml)

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Views\Basket\Index.cshtml

Now let's update the basket view to replace the partial view tag helper with the view component tag helper.

Open the \Views\Basket\Index.cshtml file. We now must make the Tag Helpers available for this file.
So, add the following directive:

```razor
@addTagHelper *, MVC
```

The @addTagHelper directive will allow us to use view component tag helpers. The "*" parameter
means all tag helpers will be available, and the "MVC" part means all view components found in the
MVC namespace will be available.

Now comment this line:

```razor
<!--THIS LINE WILL BE COMMENTED OUT-->
@*<partial name="_BasketList" for="@items" />*@
```
**Listing**: removing PartialTagHelper for BasketList (\Views\Basket\Index.cshtml)

Now, let's reference our view component tag helper. View components will be available when
you type the "<vc:" prefix:

![Replacing Partial View Tag Helper](replacing_partial_view_tag_helper.png)

Notice how the BasketList view component is displayed as "basket-list". This is the called
"Kebab-Case" style (because it looks like, you know, a kebab stick).

Another thing you may have noticed is the @_Generated_BasketListViewComponentTagHelper name,
which is the name of the class automatically generated into the assembly when you compile the view component.

Now let's also provide the items parameter for the view component:

```razor
@*<partial name="_BasketList" for="@items" />*@
<vc:basket-list items="@items"></vc:basket-list>
```
**Listing**: the view component tag helper for BasketList (\Views\Basket\Index.cshtml)

At this point, we can run the application, and verify that our view component is now working exactly as the replaced partial view:

![Running Webapp](running_webapp.png)

#### Moving BasketItem to ViewModels

In the above section we are using BasketItem as a view model. So, as a refactoring step, 
let's move it to the Models\ViewModels folder:

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Controllers\BasketController.cs

```csharp
public class BasketItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
```
**Listing**: Moving BasketItem.cs to Models\ViewModels

As this has a side effect, we also have to fix the namespace in the folowing files: 

```csharp
using MVC.Models.ViewModels
```

- /ViewComponents/BasketListViewComponent.cs
- Components/BasketList/Default.cshtml
- /Views/Basket/Index.cshtml
- _BasketItem.cshtml


#### Defining Logic For the View Component Class

Currently, the BasketList View Component class is pretty dumb. But now we have a new task to implement
a new business logic for the class:

- If the list parameter is empty, the component must display an empty view
- If the list parameter contains basket items, the component must display the default view

#### Unit Testing Our View Component

Differently from partial views, view components allow testability. Just like any regular class,
view component classes can be unit-tested. Let's add a new unit testing project to our solution:

Go to the File menu and select: New > Project > Test > xUnit Test Project (.NET Core)

![Add New Project Xunit](add_new_project_xunit.png)

**Figure**: Adding a new xUnit project

The new xUnit test project always contains an empty test class (UnitTest1):

```csharp
public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }
}
```

The xUnit is one of the unit testing project templates that come with Visual Studio.

The [Fact] attribute tells the xUnit framework that a parameterless test method must be run by the Test Runner. 
(We are going to see the Test Runner in the next section).

Since we desire to test the BasketListViewComponent class, we will rename the test class to
BasketListViewComponentTest:

```csharp
public class BasketListViewComponentTest
{
    [Fact]
    public void Test1()
    {

    }
}
```
**Listing**: renaming the test class to BasketListViewComponentTest

Let's also rename the Test1() method to something expressive, something that describes
the behavior we are verifying: "calls to the Invoke() method with items should display default view"

```csharp
public class BasketListViewComponentTest
{
    [Fact]
    public void Invoke_With_Items_Should_Display_Default_View()
    {

    }
}
```
**Listing**: creating our first unit test with xUnit

As a good practice, each unit test method must be split in 3 sections, called "Arrange-Act-Assert":

- The Arrange section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.
- The Act section invokes the method under test with the arranged parameters.
- The Assert section verifies that the action of the method under test behaves as expected.

Let's explicitly introduce this sections in the code:

```csharp
public class BasketListViewComponentTest
{
    [Fact]
    public void Invoke_With_Items_Should_Display_Default_View()
    {
        //arrange 

        //act 

        //assert
    }
}
```
**Listing**: the triple A of unit testing: Arrange, Act and Assert

In the arrange section, we must initialize the object:

```csharp
[Fact]
public void Invoke_With_Items_Should_Display_Default_View()
{
    //arrange 
    var vc = new BasketListViewComponent();

    //act 

    //assert

}
```

In the act section, we invoke the method under test with the arranged parameters:
 
```csharp
[Fact]
public void Invoke_With_Items_Should_Display_Default_View()
{
    //arrange 
    var vc = new BasketListViewComponent();

    //act 
    var result = vc.Invoke();

    //assert

}
```

But the Invoke() method produces an compilation error:

```
error CS0012: The type 'ViewComponent' is defined in an assembly that is not referenced. You must add a reference to assembly 'Microsoft.AspNetCore.Mvc.ViewFeatures, Version=2.2.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'.
```

Press CTRL + DOT to open the context menu and and select: Install package 'Microsoft.AspNetCore.Mvc.ViewFeatures'.

But remember that the BasketListViewComponent.Invoke() method requires an items parameter:

```
Invoke With Items => Display Default View
      ACTION      =>       ASSERT
```

So let's use the arrange section to declare an items variable and populate it with some
basket items:

**Listing**: arranging for the test and calling the Invoke() method

```csharp
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
    }
}
```
**Listing**: providing a parameter for the Invoke() method

Now it's time to implement the assert section of our unit test. The assert section is where
all verifications occur, to make sure the method under test behaves as expected:

```
       CAUSE      =>       EFFECT
=========================================
Invoke With Items => Display Default View
      ACTION      =>       ASSERT
```

The BasketListViewComponent.Invoke() returns a IViewComponentResult, which is an interface. But we must make sure the object returned by the method is
a view, or more specifically, an instance of ViewViewComponentResult.

When using xUnit testing framework, we can verify that a variable is of a certain type by using the method
Assert.IsAssignableFrom<T>(object):

```csharp
//act
var result = vc.Invoke(items);
//assert
Assert.IsAssignableFrom<ViewViewComponentResult>(result);
```

Now we have our first testable arrange-act-assert method. Let's use the Test Explorer to execute it:

Test > Windows > Test Explorer

OR

Ctrl+E, T

![Test Explorer Menu](test_explorer_menu.png)

When you open the Test Explorer for the first time, you see the test structure for the application:

- MVC.Test (Assembly)
  - MVC.Test.ViewComponents (Namespace)
    - BasketListViewComponentTest (Test class)
      - Invoke_With_Items_Should_Display_Default_View (Test method - Fact)

![Test Explorer](test_explorer.png)

This structure is quite helpful in keeping the Test Explorer organized, while we 
implement more and more tests. Otherwise, the Test Explorer might be cluttered by the
increasing volume of a plain list.

When we click the Run All menu, the application will be recompiled if needed,
then the xUnit testing framework will execute the only existing test so far:

![One Test Passed](one_test_passed.png)

As we can see, the test passed successfully. This kind of execution doesn't allow inspecting objects, parameters, variables, etc. while
the test executes. If you want to debug the execution of the test, you should:

1) Place breakpoints in the testable method (and possibly in the rest of the affected code) as you wish:

![Breakpoint Act](breakpoint_act.png)

2) Righ-click the test name and choose Debug Selected Test menu:

![Debug Selected Test](debug_selected_test.png)

3) Now you can debug the executing code just like you would do with a regular application:

![Breakpoint Act Hit](breakpoint_act_hit.png)

At this point our test is doing a simple test, which is just checking whether the result variable contains a view. But this is not
enough: we must also check if the view in the result is actually the Default view.
We can do this by comparing the ViewName property of the ViewViewComponentResult object
with the "Default" string. In unit testing methods, we do this by calling the Assert.Equal(expected, actual):

```csharp
//assert
ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
Assert.Equal("Default", vvcResult.ViewName);
```

And here we have the complete test implementation:

```csharp
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
```
**Listing**: checking the result type and result view name

Any time you change the test method like that, you must run it again.
Running the test again, we can see it's still passing:

![One Test Passed](one_test_passed.png)

Now, the test implementagion can be considered complete, and should not be changed again, 
unless there are changes in the method under test, or in the objects it depends upon.

But be careful not to test more than you should per unit test. Here, always apply the KISS Principle:
(Keep It Simple, Stupid). Each test must do one thing and one thing only. If a single test method is
accumulating multiple responsibilities, refactor it and split it into multiple test methods
with single responsibilities.


#### Basket Component Without Items Should Display Empty View

It's time to implement the second rule regarding the basket list view component:

- If the list parameter contains basket items, the component must display the default view

In the same BasketListViewComponentTest class, let's implement a second unit test method for this rule:

```csharp
[Fact]
public void Invoke_Without_Items_Should_Display_Empty_View()
{
    //arrange 

    //act 

    //assert
}
```
Listing: the new Invoke_Without_Items_Should_Display_Empty_View test method

In this case, the BasketListViewComponent.Invoke() method will be called with an empty list:

```csharp
[Fact]
public void Invoke_Without_Items_Should_Display_Empty_View()
{
    //arrange 
    var vc = new BasketListViewComponent();
    //act 
    var result = vc.Invoke(new List<BasketItem>());

    //assert
}
```
**Listing**: calling the Invoke() method with an empty list

The rest of the test is much like the first test we wrote, with the difference that we
are now checking whether the view named "Empty" is being returned.

```csharp
[Fact]
public void Invoke_Without_Items_Should_Display_Empty_View()
{
    //arrange 
    var vc = new BasketListViewComponent();
    //act 
    var result = vc.Invoke(new List<BasketItem>());

    //assert
    ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
    Assert.Equal("Empty", vvcResult.ViewName);
}
```
**Listing**: testing the basket list view component for an empty basket

Now, let's compile and see the new test being displayed in the Test Explorer:

![Second Test](second_test.png)

And then we run all the tests, or we run only the specified test:

![Run Selected Tests](run_selected_tests.png)

Notice how the entire structure is marked with the fail icon, except the first
unit test we created, which remains green:

![Test Fail](test_fail.png)

Whenever possible, create a test first, then implement the rules in the business classes until the test passes.
Let's do it now. Let's make the test pass.

We should modify the BasketListViewComponent class to include a condition verifying the 
number of items in the basket. If there are no items, we should return an Empty view:

```csharp
public IViewComponentResult Invoke(List<BasketItem> items)
{
    if (items.Count == 0)       // these 3 lines were added
    {                           // so that we can return
        return View("Empty");   // a different view in case
    }                           // of empty basket
    return View("Default", items);
}
```
**Listing**: returning a different view in case of empty basket

Running the tests again, we can notice that everything passes:

![Two Tests Passing](two_tests_passing.png)

But while the second test is passing, we still don't have an Empty view for this basket list condition.
We can solve that by adding a new Empty.cshtml markup file in the \MVC\Views\Basket\ project folder:

```razor
<div class="card">
    <div class="card-body">
        <!--https://getbootstrap.com/docs/4.0/components/alerts/-->
        <div class="alert alert-warning" role="alert">
            There are no items in your basket yet! Click <a asp-controller="catalog"><b>here</b></a> to start shopping!
        </div>
    </div>
</div>
```
**Listing**: the new Empty.cshtml view showing the Bootstrap 4 alert component

We can now stop doing unit tests for a while and start a manual test, where
we try to simulate an empty basket list.

The first step here is to comment out the BasketItem instances in
\MVC\Views\Basket\Index.cshtml file, so that the basket list is empty:

C:\Users\marce\Documents\GitHub\RoadToMicroservices\Part 02\MVC\Views\Basket\Empty.cshtml

```csharp
    List<BasketItem> items = new List<BasketItem>
    {
        @*new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
        new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }*@
    };
```
**Listing**: Commenting the items in order to check the alert while running the application

Running the application again, we can notice the Bootrap Alert component, displaying the alert
message: "There are no items in your basket yet! Click here to start shopping!"

![Alert Empty Items](alert_empty_items.png)


#### Creating a ViewComponent for BasketItem

Not only the basket list, but also the basket item partial view can be converted into a view component.

This require some steps, similar to what we have seen a little earlier:

1) Create a new BasketItemViewComponent class under ViewComponents\ folder

```csharp
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
```
**Listing**: the new BasketItemViewComponent class (\ViewComponents\BasketItemViewComponent.cs)


2) Create a new BasketItem folder under Components

3) Move the partial view file: _BasketItem.cshtml into the folder: Components/BasketItem/

4) Rename this file to Default.cshtml

5) Change the \Views\Basket\Components\BasketList\Default.cshtml file to add the @addTagHelper directive:

```csharp
@addTagHelper *, MVC
```
**Listing**: adding the @addTagHelper directive

6) Remove the reference to the _BasketItem partial view tag helper:

```razor
<partial name="_BasketItem" for="@item" />
```

7) Replace it with the new view component tag helper

```razor
<vc:basket-item item="@item"></vc:basket-item>
```

8) This will give us the following markup:

```razor
<div class="card">
    <div class="card-header">
        <div class="row">
            <div class="col-sm-6">
                Item
            </div>
            <div class="col-sm-2 text-center">
                Unit Price
            </div>
            <div class="col-sm-2 text-center">
                Quantity
            </div>
            <div class="col-sm-2">
                <span class="pull-right">
                    Subtotal
                </span>
            </div>
        </div>
    </div>
    <div class="card-body">
        @foreach (var item in items)
        {
            <vc:basket-item item="@item"></vc:basket-item>
        }
    </div>
    <div class="card-footer">
        <div class="row">
            <div class="col-sm-10">
                <span numero-items>
                    Total: @items.Count
                    item@(items.Count > 1 ? "s" : "")
                </span>
            </div>
            <div class="col-sm-2">
                Total: <span class="pull-right" total>
                    @(items.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
                </span>
            </div>
        </div>
    </div>
</div>
```
**Listing**: the Components/BasketItem/Default.cshtml file

9) Now, reactivate the code lines containing the BasketItem instances, which we previously
commented out to test the basket list:

```csharp
new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
```
**Listing**: restoring the 3 basket items (\MVC\Views\Basket\Index.cshtml)

10) Last step: delete the _BasketList.cshtml partial view file.

#### Unit Testing BasketItemViewComponent

ADD BasketItemViewComponentTest.cs

```csharp
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
```
**Listing**: setting up the first BasketItemViewComponent unit testing 

#### Moving Components to Views/Shared Folder

Part 02/MVC/ViewComponents/BasketItemViewComponent.cs

```csharp
public IViewComponentResult Invoke(BasketItem item, bool isSummary)
```
**Listing**: adding a isSummary parameter to Invoke() method

```csharp
if (isSummary == true)
{
    return View("SummaryItem", item);
}
```
**Listing**: returning a different view for summary presentation mode


Part 02/MVC/ViewComponents/BasketListViewComponent.cs

```csharp
public IViewComponentResult Invoke(List<BasketItem> items, bool isSummary)
```
**Listing**: adding a isSummary parameter to Invoke() method


```csharp
return View("Default", items);
```

```csharp
return View("Default", new BasketItemList
{
    List = items,
    IsSummary = isSummary
});
```
**Listing**: passing the new view model to View() method


Part 02/MVC.Test/BasketItemViewComponentTest.cs

#### changes to Invoke_Should_Display_Default_View() method

```csharp
var result = vc.Invoke(item, false);
```
**Listing**: passing the new isSummary argument to Invoke() method

```csharp
BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
```
**Listing**: verifying that the result model is of type BasketItem

```csharp
Assert.Equal(item.ProductId, resultModel.ProductId);
```
**Listing**: verifying that the producId is the same passed to the view

#### implemening new test: Invoke_Should_Display_SummaryItem_View

```csharp
[Fact]
public void Invoke_Should_Display_SummaryItem_View()
{
    //arrange 
    var vc = new BasketItemViewComponent();
    BasketItem item =
        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 };

    //act 
    var result = vc.Invoke(item, true);

    //assert
    ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
    Assert.Equal("SummaryItem", vvcResult.ViewName);
    BasketItem resultModel = Assert.IsAssignableFrom<BasketItem>(vvcResult.ViewData.Model);
    Assert.Equal(item.ProductId, resultModel.ProductId);
}
```
**Listing**: testing behavior when summary style of ViewComponent is invoked

```csharp
...
var result = vc.Invoke(items, false);
...
var result = vc.Invoke(new List<BasketItem>(), false);
...
```
**Listing**: updating tests class with the isSummary argument (BasketListViewComponentTest.cs)



```csharp
public class BasketItemList
{
    public List<BasketItem> List { get; set; }
    public bool IsSummary { get; set; }
}
```
**Listing**: the new BasketItemList class (ViewModels\BasketItemList.cs)


```razor
<vc:basket-list items="@items" is-summary="false"></vc:basket-list>
```
**Listing**: adding the new is-summary argument to the view component tag helper (/Views/Basket/Index.cshtml)



```razor
 @using MVC.Models.ViewModels
 @model string
 @addTagHelper *, MVC
 @{

    List<BasketItem> items = new List<BasketItem>
    {
        new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
        new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
        new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
    };
```
**Listing**: adding the summary data checkout view (/Views/Checkout/Index.cshtml)


```razor
<h4>Summary</h4>

<vc:basket-list items="@items" is-summary="true"></vc:basket-list>
```
**Listing**: adding the summary basket view component to the checkout view (/Views/Checkout/Index.cshtml)




MOVE
...Basket/Components/BasketItem/Default.cshtml
TO
...Shared/Components/BasketItem/Default.cshtml





```razor
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
```
**Listing**: the new summary item view component (Views/Shared/Components/BasketItem/SummaryItem.cshtml)



MOVE
...Basket/Components/BasketList/Default.cshtml
TO
...Shared/Components/BasketList/Default.cshtml





Remove this lines from BasketList/Default.cshtml...
```razor
@model List<BasketItem>;

@{
    var items = Model;
}
```
... and replace them with...
```razor
@model BasketItemList;
```


REPLACE:
```razor
@foreach (var item in items)
{
    <vc:basket-item item="@item"></vc:basket-item>
}
```
WITH:
```razor
@foreach (var item in Model.List)
{
    <vc:basket-item item="@item" is-summary="Model.IsSummary"></vc:basket-item>
}
```

REPLACE:
```razor
Total: @items.Count
item@(items.Count > 1 ? "s" : "")
```
WITH:
```razor
Total: @Model.List.Count
item@(Model.List.Count > 1 ? "s" : "")
```


REPLACE:
```razor
@(items.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
```
WITH:
```razor
@(Model.List.Sum(item => item.Quantity * item.UnitPrice).ToString("C"))
```

MOVE
...Basket/Components/BasketList/Empty.cshtml
TO
...Shared/Components/BasketList/Empty.cshtml


#### Fixing All Tests for IBasketService

ADD Moq library to MVC.Test project:
Part 02/MVC.Test/MVC.Test.csproj
+ <PackageReference Include="Moq" Version="4.10.1" />


Part 02/MVC.Test/BasketListViewComponentTest.cs

add these lines:
```csharp
using Moq;
using MVC.Services;
```

REPLACE:
```csharp
//arrange 
var vc = new BasketListViewComponent();
```
WITH
```csharp
//arrange
Mock<IBasketService> basketServiceMock =
    new Mock<IBasketService>();
```

ADD:
```csharp
basketServiceMock.Setup(m => m.GetBasketItems())
    .Returns(items);
var vc = new BasketListViewComponent(basketServiceMock.Object);
```

REMOVE THE Products ARGUMENT
```csharp
var result = vc.Invoke(false);
```


```csharp
Mock<IBasketService> basketServiceMock =
    new Mock<IBasketService>();

basketServiceMock.Setup(m => m.GetBasketItems())
    .Returns(new List<BasketItem>());
var vc = new BasketListViewComponent(basketServiceMock.Object);
```
**Listing**: acting against BasketListViewComponent with a mock object



REPLACE:
var result = vc.Invoke(new List<BasketItem>(), false);
WITH:
var result = vc.Invoke(false);

Part 02/MVC/Services/IBasketService.cs

```csharp
public interface IBasketService
{
    List<BasketItem> GetBasketItems();
}
```
**Listing**: the new IBasketService interface (/Services/IBasketService.cs)



Part 02/MVC/Services/BasketService.cs

```csharp
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
```
**Listing**: the new BasketService class (/Services/BasketService.cs)




Part 02/MVC/Startup.cs

```csharp
...
using MVC.Services; 
...
services.AddTransient<IBasketService, BasketService>();
...
```
**Listing**: new lines added to Startup class (MVC/Startup.cs)



Part 02/MVC/ViewComponents/BasketListViewComponent.cs

Adding services namespace
```csharp
using MVC.Services;
```


```csharp
private readonly IBasketService basketService;

public BasketListViewComponent(IBasketService basketService)
{
    this.basketService = basketService;
}

public IViewComponentResult Invoke(bool isSummary)
{
    List<BasketItem> items = basketService.GetBasketItems();
```
**Listing**: consuming IBasketService via dependency injection



Part 02/MVC/Views/Basket/Index.cshtml

Remove this lines from the index view:

```csharp
//List<BasketItem> items = new List<BasketItem>
//{
//    new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
//    new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
//    new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
//};
```

Change this line to remove the `items` attribute... 
```csharp
<vc:basket-list items="@items" is-summary="false"></vc:basket-list>
```

```csharp
<vc:basket-list is-summary="false"></vc:basket-list>
```
**Listing**: the view component tag helper without the `items` attribute

Part 02/MVC/Views/Checkout/Index.cshtml

Remove this lines from the checkout view:
```csharp
//List<BasketItem> items = new List<BasketItem>
//{
//    new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
//    new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
//    new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
//};
```

Change this line to remove the `items` attribute... 
```csharp
<vc:basket-list items="@items" is-summary="true"></vc:basket-list>
```

```csharp
<vc:basket-list is-summary="true"></vc:basket-list>
```
**Listing**: the view component tag helper without the `items` attribute

#### Asserting Collections

MOVE
...2/MVC.Test/BasketItemViewComponentTest.cs
TO
...Components/BasketItemViewComponentTest.cs


MOVE
...2/MVC.Test/BasketListViewComponentTest.cs
TO
...Components/BasketListViewComponentTest.cs


```csharp
var model = Assert.IsAssignableFrom<BasketItemList>(vvcResult.ViewData.Model);
Assert.Collection<BasketItem>(model.List,
    i => Assert.Equal(1, i.ProductId),
    i => Assert.Equal(5, i.ProductId),
    i => Assert.Equal(9, i.ProductId)
);
```
**Listing**: asserting validity of the collection type and contents

#### Creating ViewComponent for Categories

Part 02/MVC/ViewComponents/CategoriesViewComponent.cs

```csharp
public class CategoriesViewComponent : ViewComponent
{
    public CategoriesViewComponent()
    {
    }

    public IViewComponentResult Invoke(List<Product> products)
    {
        return View("Default", products);
    }
}
```
**Listing**: the new CategoriesViewComponent class (/ViewComponents/CategoriesViewComponent.cs)


MOVE
Part 02/MVC/Views/Catalog/_Categories.cshtml
TO
...alog/Components/Categories/Default.cshtml


Part 02/MVC/Views/Catalog/Index.cshtml

Add the addTagHelper directive

```csharp
@addTagHelper *, MVC
@model List<Product>;
```

Remove the partial tag helper
```csharp
<partial name="_Categories" for="@Model" />
```

and replace it with the Categories view component tag helper
```csharp
<vc:categories products="@Model"></vc:categories>
```

#### Creating ViewComponent for ProductCard

Part 02/MVC/ViewComponents/ProductCardViewComponent.cs

```csharp
public class ProductCardViewComponent : ViewComponent
{
    public ProductCardViewComponent()
    {

    }

    public IViewComponentResult Invoke(Product product)
    {
        return View("Default", product);
    }
}
```
**Listing**: the new ProductCardViewComponent class (/ViewComponents/ProductCardViewComponent.cs)

Part 02/MVC/Views/Catalog/Components/Categories/Default.cshtml

Add the addTagHelper directive

```csharp
@addTagHelper *, MVC
@model List<Product>;
```


Remove the foreach instruction with partial tag helper
```razor
foreach (var productIndex in productsInPage)
{
      <partial name="_ProductCard" for="@productIndex" />
}
```

...with the foreach instruction with ProductCard view component
```razor
foreach (var product in productsInPage)
{
      <vc:product-card product="@product"></vc:product-card>
}
```

MOVE
... 02/MVC/Views/Catalog/_ProductCard.cshtml 
TO
...log/Components/ProductCard/Default.cshtml

#### Creating ViewComponents for Catalog

Part 02/MVC/Models/ViewModels/CarouselPageViewModel.cs


```csharp
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
```
**Listing**: the new CarouselPageViewModel class (/Models/ViewModels/CarouselPageViewModel.cs)




Part 02/MVC/Models/ViewModels/CarouselViewModel.cs

```csharp
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
```
**Listing**: the new CarouselViewModel class (/Models/ViewModels/CarouselViewModel.cs)




Part 02/MVC/Models/ViewModels/CategoriesViewModel.cs

```csharp
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
```
**Listing**: the new CategoriesViewModel class (/Models/ViewModels/CategoriesViewModel.cs)




Part 02/MVC/ViewComponents/CarouselPageViewComponent.cs

```csharp
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
```
**Listing**: the new CarouselPageViewComponent class (/ViewComponents/CarouselPageViewComponent.cs)




Part 02/MVC/ViewComponents/CarouselViewComponent.cs

```csharp
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
```

**Listing**: the new CarouselViewComponent class (/ViewComponents/CarouselViewComponent.cs)

Part 02/MVC/ViewComponents/CategoriesViewComponent.cs



```csharp
using MVC.Models.ViewModels;
using System.Linq;
.
.
.

        const int PageSize = 4;   
.
.
.

            var categories = products
                .Select(p => p.Category)
                .Distinct()
                .ToList();
            return View("Default", new CategoriesViewModel(categories, products, PageSize));
.
.
.
```
**Listing**: the CategoriesViewComponent class updated (/ViewComponents/CategoriesViewComponent.cs)




Part 02/MVC/Views/Catalog/Components/Carousel/Default.cshtml

```razor
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
```
**Listing**: the new Carousel/Default view (/Views/Catalog/Components/Carousel/Default.cshtml)



Part 02/MVC/Views/Catalog/Components/CarouselPage/Default.cshtml

```razor
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
```
**Listing**: the new CarouselPage/Default view (/Views/Catalog/Components/CarouselPage/Default.cshtml)



Part 02/MVC/Views/Catalog/Components/Categories/Default.cshtml

```razor
@using MVC.Models.ViewModels
@addTagHelper *, MVC
@model CategoriesViewModel
.
.
.
@foreach (var category in Model.Categories)
.
.
.
<vc:carousel category="@category" products="@Model.Products" page-size="@Model.PageSize"></vc:carousel>
.
.
.
```
**Listing**: the updated Categories/Default markup file (/Views/Catalog/Components/Categories/Default.cshtml)




#### Creating Navigation Bar Notification Icons

Part 02/MVC/Views/Shared/_Layout.cshtml

```razor
<div class="navbar-collapse collapse justify-content-end">
    <ul class="nav navbar-nav">
        <li>
            <div class="container-notification">
                <a asp-controller="notifications"
                    title="Notifications">
                    <div class="user-count notification show-count fa fa-bell" data-count="2">
                    </div>
                </a>
            </div>
        </li>
        <li>
            <span>
                &nbsp;
                &nbsp;
            </span>
        </li>
        <li>
            <div class="container-notification">
                <a asp-action="index" asp-controller="basket"
                    title="Basket">
                    <div class="user-count userbasket show-count fa fa-shopping-cart" data-count="3">
                    </div>
                </a>
            </div>
        </li>
    </ul>
</div>
```
**Listing**: the notification elements at the notification bar (/Views/Shared/_Layout.cshtml)

Part 02/MVC/wwwroot/css/site.css

```css
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
```
**Listing**: the cascade style sheet with styling for the user count controls (/wwwroot/css/site.css)



#### Creating UserCounter ViewComponent

Part 02/MVC/Models/ViewModels/UserCountViewModel.cs

```csharp
public class UserCountViewModel
{   
    public UserCountViewModel(string title, string controllerName, string cssClass, string icon, string count)
    {
        Title = title;
        ControllerName = controllerName;
        CssClass = cssClass;
        Icon = icon;
        Count = count;
    }

    public string ControllerName { get; set; }
    public string Title { get; set; }
    public string CssClass { get; set; }
    public string Icon { get; set; }
    public string Count { get; set; }
}
```
Listing: the new UserCountViewModel class (/Models/ViewModels/UserCountViewModel.cs)




Part 02/MVC/ViewComponents/UserCounterViewComponent.cs

```csharp
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
```
**Listing**: the new UserCounterViewComponent class (/ViewComponents/UserCounterViewComponent.cs)


Part 02/MVC/Views/Shared/Components/UserCounter/Default.cshtml

```razor
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
```
**Listing**: the new UserCounter/Default markup file (/Views/Shared/Components/UserCounter/Default.cshtml)

Part 02/MVC/Views/Shared/_Layout.cshtml

Remove these lines:
```razor
<div class="container-notification">
    <a asp-controller="notifications"
        title="Notifications">
        <div class="user-count notification show-count fa fa-bell" data-count="2">
        </div>
    </a>
</div>
.
.
.
<div class="container-notification">
    <a asp-action="index" asp-controller="basket"
        title="Basket">
        <div class="user-count userbasket show-count fa fa-shopping-cart" data-count="3">
        </div>
    </a>
</div>
```

And add these lines:

```razor
@addTagHelper *, MVC
.
.
.
<vc:user-counter 
    title="Notifications"
    controller-name="notifications"
    css-class="notification"
    icon="bell"
    count="2">
</vc:user-counter>
.
.
.
<vc:user-counter 
    title="Basket"
    controller-name="basket"
    css-class="basket"
    icon="shopping-cart"
    count="3">
</vc:user-counter>
.
.
.
```
**Listing**: UserCounter Tag Helpers added to the layout file (/Views/Shared/_Layout.cshtml)


Part 02/MVC/wwwroot/css/site.css

Replace this line...
```css
content: attr(data-count);
```
...whith this one:
```css
content: attr(count);
```

#### Creating UserCounterService

Part 02/MVC/Models/ViewModels/UserCountViewModel.cs

Replace the string count with int count
```csharp
public UserCountViewModel(string title, string controllerName, string cssClass, string icon, string count)
.
.
.
public string Count { get; set; }
```


```csharp
public UserCountViewModel(string title, string controllerName, string cssClass, string icon, int count)
.
.
.
public int Count { get; set; }
```
**Listing**: Count parameter as integer (/Models/ViewModels/UserCountViewModel.cs)


Part 02/MVC/Services/IUserCounterService.cs

```csharp
public interface IUserCounterService
{
    int GetBasketCount();
    int GetNotificationCount();
}
```
**Listing**: the new IUserCounterService interface (/Services/IUserCounterService.cs)


Part 02/MVC/Services/UserCounterService.cs

```csharp
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
```
**Listing**: the new UserCounterService class (/Services/UserCounterService.cs)


Part 02/MVC/Startup.cs

```csharp
var userCounterServiceInstance = new UserCounterService();
services.AddSingleton<IUserCounterService>(userCounterServiceInstance);
```
**Listing**: new dependency injection instructions (/Startup.cs)



Part 02/MVC/ViewComponents/UserCounterViewComponent.cs

```csharp
public enum UserCounterType
{
    Notification = 1,
    Basket = 2
}
```
**Listing**: the new UserCounterType enum (/ViewComponents/UserCounterViewComponent.cs)





```csharp
protected readonly IUserCounterService userCounterService;
 
public UserCounterViewComponent(IUserCounterService userCounterService)
{
    this.userCounterService = userCounterService;
}
.
.
.
public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, UserCounterType userCounterType)
{
    int count = 0;

    if (userCounterType == UserCounterType.Notification)
    {
        count = userCounterService.GetNotificationCount();
    }
    else if (userCounterType == UserCounterType.Basket)
    {
        count = userCounterService.GetBasketCount();
    }
```
**Listing**: modifying the UserCounterViewComponent class to use enum (/ViewComponents/UserCounterViewComponent.cs)



Part 02/MVC/Views/Shared/_Layout.cshtml

```razor
user-counter-type="Notification">
.
.
.
user-counter-type="Basket">
```
**Listing**: the user counter tag helpers with the appropriate UserCounterType enum (/Views/Shared/_Layout.cshtml)


#### Creating NotificationCounter, BasketCounter Subclasses

Part 02/MVC/ViewComponents/UserCounterViewComponent.cs



```csharp
public abstract class UserCounterViewComponent : ViewComponent
{
    protected enum UserCounterType
    {
        Notification = 1,
        Basket = 2
    }

    protected IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, int count, UserCounterType userCounterType)
    {
        var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
        return View("~/Views/Shared/Components/UserCounter/Default.cshtml", model);
    }
}
```
**Listing**: the UserCounterViewComponent class became superclass (/ViewComponents/UserCounterViewComponent.cs)


```csharp
if (userCounterType == UserCounterType.Notification)
{
    count = userCounterService.GetNotificationCount();
}
else if (userCounterType == UserCounterType.Basket)
{
    count = userCounterService.GetBasketCount();
}
```
**Listing**: now each count receives the result of the appropriate service method

    
```csharp
public class NotificationCounterViewComponent : UserCounterViewComponent
{
    public NotificationCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }
 
    public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
    {
        int count = userCounterService.GetNotificationCount();
        return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Notification);
    }
}
```
**Listing**: updates needed so that the NotificationCounterViewComponent class becomes a subclass



```csharp
public class BasketCounterViewComponent : UserCounterViewComponent
{
    public BasketCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }

    public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
    {
        int count = userCounterService.GetBasketCount();
        return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Basket);
    }
}
```
**Listing**: updates needed so that the BasketCounterViewComponent class becomes a subclass


Part 02/MVC/Views/Shared/_Layout.cshtml

```razor
<vc:notification-counter 
.
.
.
</vc:notification-counter>

<vc:basket-counter 
.
.
.
</vc:basket-counter>    
```
**Listing**: replacing the old UserCounter tag helper with specialized counter tag helpers (/Views/Shared/_Layout.cshtml)







