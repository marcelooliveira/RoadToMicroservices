using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MVC.Areas.Checkout.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Checkout.Data
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly CheckoutDbContext context;

        public CheckoutRepository(CheckoutDbContext contexto)
        { 
            this.context = contexto;
        }

        public async Task<Order> CreateOrUpdate(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            if (order.Items.Count == 0)
                throw new NoItemsException();

            foreach (var item in order.Items)
            {
                if (
                    string.IsNullOrWhiteSpace(item.ProductId)
                    || string.IsNullOrWhiteSpace(item.ProductName)
                    || item.Quantity <= 0
                    || item.UnitPrice <= 0
                    )
                {
                    throw new InvalidItemException();
                }
            }

            if (string.IsNullOrWhiteSpace(order.CustomerId)
                 || string.IsNullOrWhiteSpace(order.CustomerName)
                 || string.IsNullOrWhiteSpace(order.CustomerEmail)
                 || string.IsNullOrWhiteSpace(order.CustomerPhone)
                 || string.IsNullOrWhiteSpace(order.CustomerAddress)
                 || string.IsNullOrWhiteSpace(order.CustomerAdditionalAddress)
                 || string.IsNullOrWhiteSpace(order.CustomerDistrict)
                 || string.IsNullOrWhiteSpace(order.CustomerCity)
                 || string.IsNullOrWhiteSpace(order.CustomerState)
                 || string.IsNullOrWhiteSpace(order.CustomerZipCode)
                )
                throw new InvalidUserDataException();

            EntityEntry<Order> entityEntry;
            try
            {
                entityEntry = await context.Set<Order>().AddAsync(order);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
            return entityEntry.Entity;
        }

        public async Task<IList<Order>> GetOrders(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentNullException();
            }
            return await
                context.Set<Order>()
                .Include(p => p.Items)
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }
    }


    [Serializable]
    public class NoItemsException : Exception
    {
        public NoItemsException() { }
        public NoItemsException(string message) : base(message) { }
        public NoItemsException(string message, Exception inner) : base(message, inner) { }
        protected NoItemsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class InvalidItemException : Exception
    {
        public InvalidItemException() { }
        public InvalidItemException(string message) : base(message) { }
        public InvalidItemException(string message, Exception inner) : base(message, inner) { }
        protected InvalidItemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class InvalidUserDataException : Exception
    {
        public InvalidUserDataException() { }
        public InvalidUserDataException(string message) : base(message) { }
        public InvalidUserDataException(string message, Exception inner) : base(message, inner) { }
        protected InvalidUserDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
