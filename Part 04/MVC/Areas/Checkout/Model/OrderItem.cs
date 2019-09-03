using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Areas.Checkout.Model
{
    public class OrderItem : BaseModel
    {
        public OrderItem()
        {

        }

        public OrderItem(string productId, string productName, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        [Required]
        public Order Order { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
        public string ImageURL { get { return $"/images/catalog/large_{ProductId}.jpg"; } }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Quantity < 1)
            {
                results.Add(new ValidationResult("Invalid quantity", new[] { "Quantity" }));
            }

            return results;
        }
    }
}