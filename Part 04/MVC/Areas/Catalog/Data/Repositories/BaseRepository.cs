using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVC.Areas.Catalog.Models;

namespace MVC.Areas.Catalog.Data.Repositories
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        protected readonly IConfiguration configuration;
        protected readonly CatalogDbContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(IConfiguration configuration,
            CatalogDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
            dbSet = context.Set<T>();
        }
    }
}
