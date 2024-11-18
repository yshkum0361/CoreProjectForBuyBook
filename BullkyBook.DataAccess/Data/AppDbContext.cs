using BullkyBook.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BullkyBook.DataAccess
{
    public class AppDbContext :IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<BullkyBookModel> bullkyBookModels { get; set; }

        public DbSet<CoverTypeModel> CoverTypeModels { get; set; }

        public DbSet<ProductModel> productModels { get; set; }
        public DbSet<AppUser> appUsers { get; set; }
        public DbSet<CompanyModel> companyModels { get; set; }
        public DbSet<ShoppingCart> shoppingCarts{ get; set; }
        public DbSet<OrderHeader> orderHeaders{ get; set; }
        public DbSet<OrderDetails> orderDetail{ get; set; }


    }
}
