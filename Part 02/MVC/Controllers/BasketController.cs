using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BasketController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
