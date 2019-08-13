using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using MVC.Areas.Basket.Services;
using MVC.Areas.Basket.ViewComponents;
using MVC.Models.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace MVC.Test.ViewComponents
{
    public class BasketListViewComponentTest
    {
        [Fact]
        public void Invoke_With_Items_Should_Display_Default_View()
        {
            //arrange
            Mock<IBasketService> basketServiceMock =
                new Mock<IBasketService>();

            List<BasketItem> items =
            new List<BasketItem>
            {
                new BasketItem { Id = 1, ProductId = 1, Name = "Broccoli", UnitPrice = 59.90m, Quantity = 2 },
                new BasketItem { Id = 2, ProductId = 5, Name = "Green Grapes", UnitPrice = 59.90m, Quantity = 3 },
                new BasketItem { Id = 3, ProductId = 9, Name = "Tomato", UnitPrice = 59.90m, Quantity = 4 }
            };
            basketServiceMock.Setup(m => m.GetBasketItems())
                .Returns(items);
            var vc = new BasketListViewComponent(basketServiceMock.Object);

            //act 
            var result = vc.Invoke(false);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Default", vvcResult.ViewName);
            var model = Assert.IsAssignableFrom<BasketItemList>(vvcResult.ViewData.Model);
            Assert.Collection<BasketItem>(model.List,
                i => Assert.Equal(1, i.ProductId),
                i => Assert.Equal(5, i.ProductId),
                i => Assert.Equal(9, i.ProductId)
            );
        }

        [Fact]
        public void Invoke_Without_Items_Should_Display_Empty_View()
        {
            //arrange 
            Mock<IBasketService> basketServiceMock =
                new Mock<IBasketService>();

            basketServiceMock.Setup(m => m.GetBasketItems())
                .Returns(new List<BasketItem>());
            var vc = new BasketListViewComponent(basketServiceMock.Object);

            //act 
            var result = vc.Invoke(false);

            //assert
            ViewViewComponentResult vvcResult = Assert.IsAssignableFrom<ViewViewComponentResult>(result);
            Assert.Equal("Empty", vvcResult.ViewName);
        }
    }
}
