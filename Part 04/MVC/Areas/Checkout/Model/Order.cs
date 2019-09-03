using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MVC.Areas.Checkout.Model
{
    public class Order : BaseModel
    {
        public Order()
        {

        }

        public Order(string userId)
        {
            CustomerId = userId;
        }

        public Order(string userId, string email) : this(userId)
        {
            CustomerEmail = email;
        }

        public Order(List<OrderItem> items, string userId, string customerName, string email, string customerPhone, string customerAddress, string customerAdditionalAddress, string customerDistrict, string customerCity, string customerState, string customerZipCode)
            : this(userId, email)
        {
            Items = items;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            CustomerAddress = customerAddress;
            CustomerAdditionalAddress = customerAdditionalAddress;
            CustomerDistrict = customerDistrict;
            CustomerCity = customerCity;
            CustomerState = customerState;
            CustomerZipCode = customerZipCode;
        }

        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        public string CustomerAdditionalAddress { get; set; }
        [Required]
        public string CustomerDistrict { get; set; }
        [Required]
        public string CustomerCity { get; set; }
        [Required]
        public string CustomerState { get; set; }
        [Required]
        public string CustomerZipCode { get; set; }

        public List<OrderItem> Items = new List<OrderItem>();

        public decimal Total()
        {
            return Items.Sum(i => i.Subtotal);
        }
    }
}
