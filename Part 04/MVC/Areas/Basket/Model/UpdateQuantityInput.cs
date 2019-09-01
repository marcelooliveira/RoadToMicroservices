using System.ComponentModel.DataAnnotations;

namespace MVC.Areas.Basket.Model
{
    public class UpdateQuantityInput
    {
        public UpdateQuantityInput()
        {

        }

        public UpdateQuantityInput(string productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
