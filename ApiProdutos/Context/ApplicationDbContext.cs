using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiProdutos.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}