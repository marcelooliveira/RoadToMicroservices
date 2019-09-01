using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Basket.Data;
using MVC.Areas.Basket.Model;
using MVC.Areas.Catalog.Data.Repositories;
using MVC.Areas.Identity.Data;
using MVC.Controllers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MVC.Areas.Basket.Controllers
{
    [Authorize]
    [Area("Basket")]
    public class HomeController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<AppIdentityUser> userManager;

        public HomeController(IBasketRepository basketRepository
            , IProductRepository productRepository
            , UserManager<AppIdentityUser> userManager)
        {
            this.basketRepository = basketRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string code = null)
        {
            string customerId = userManager.GetUserId(this.User);

            CustomerBasket basket;
            if (!string.IsNullOrWhiteSpace(code))
            {
                var product = await productRepository.GetProductAsync(code);
                if (product == null)
                {
                    return RedirectToAction("ProductNotFound", "Basket", code);
                }

                var item = new BasketItem(product.Code, product.Code, product.Name, product.Price, 1);
                basket = await basketRepository.AddBasketAsync(customerId, item);
            }
            else
            {
                basket = await basketRepository.GetBasketAsync(customerId);
            }
            return View(basket);
        }

        /// <summary>
        /// Updates shopping basket item quantity
        /// </summary>
        /// <param name="customerId">CustomerId</param>
        /// <param name="input">Shopping basket item to be updated</param>
        /// <returns></returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateQuantityOutput>> UpdateItem([FromBody] UpdateQuantityInput input)
        {
            string customerId = userManager.GetUserId(this.User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var output = await basketRepository.UpdateBasketAsync(customerId, input);
                return Ok(output);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(customerId);
            }
        }
    }
}
