using Microsoft.EntityFrameworkCore;
using MVC.Areas.Checkout.Model;

namespace MVC.Areas.Checkout.Data
{
    //PM> Add-Migration Checkout -Context CheckoutDbContext -OutputDir "Areas/Checkout/Data/Migrations"
    public class CheckoutDbContext : DbContext
    {
        public CheckoutDbContext()
        {

        }

        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>(b =>
            {
                b.HasKey(t => t.Id);
            });

            builder.Entity<OrderItem>(b =>
            {
                b.HasKey(t => t.Id);
            });
        }
    }
}
