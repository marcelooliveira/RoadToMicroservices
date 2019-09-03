using System.Collections.Generic;
using System.Threading.Tasks;
using MVC.Areas.Checkout.Model;

namespace MVC.Areas.Checkout.Data
{
    public interface ICheckoutRepository
    {
        Task<Order> CreateOrUpdate(Order order);
        Task<IList<Order>> GetOrders(string customerId);
    }
}