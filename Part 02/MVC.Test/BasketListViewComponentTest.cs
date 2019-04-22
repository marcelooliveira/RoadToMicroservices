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

        [Fact]
        public void Invoke_With_Items_Should_Display_Empty_View()
        {
            //arrange 
            var vc = new BasketListViewComponent();
            //act 
            var result = vc.Invoke(new List<BasketItem>());

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Empty", vvcResult.ViewName);
        }
    }
}
