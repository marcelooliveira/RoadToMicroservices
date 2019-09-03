using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Areas.Checkout.Model.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {

        }

        public CheckoutViewModel(string userId)
        {
            UserId = userId;
        }

        public CheckoutViewModel(string userId, string email) : this(userId)
        {
            Email = email;
        }

        public CheckoutViewModel(List<CheckoutItem> items, string userId, string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
            : this(userId, email)
        {
            Items = items;
            Name = name;
            Phone = phone;
            Address = address;
            AdditionalAddress = additionalAddress;
            District = district;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        [Required]
        public List<CheckoutItem> Items { get; set; } = new List<CheckoutItem>();
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string AdditionalAddress { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
    }
}
