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
