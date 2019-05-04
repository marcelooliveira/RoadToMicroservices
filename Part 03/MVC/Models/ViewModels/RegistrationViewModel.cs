using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel()
        {

        }

        public RegistrationViewModel(string userId)
        {
            UserId = userId;
        }

        public RegistrationViewModel(string userId, string name, string email, string phone, string address, string additionalAddress, string district, string city, string state, string zipCode)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            AdditionalAddress = additionalAddress;
            District = district;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

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

        public RegistrationViewModel GetClone()
        {
            return new RegistrationViewModel(this.UserId, this.Name, this.Email, this.Phone, this.Address, this.AdditionalAddress, this.District, this.City, this.State, this.ZipCode);
        }
    }
}
