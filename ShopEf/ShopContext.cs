using Microsoft.EntityFrameworkCore;
using ShopEf.Models;

namespace ShopEf
{
    public class ShopContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(c =>
            {
                c.Property(c => c.Name).HasMaxLength(50).IsRequired();

                c.HasMany(c => c.CategoryProducts)
                .WithOne(cp => cp.Category)
                .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(p => p.Name).HasMaxLength(50).IsRequired();

                p.HasMany(p => p.CategoryProducts)
                .WithOne(cp => cp.Product)
                .HasForeignKey(p => p.ProductId);

                p.HasMany(p => p.Orders)
                .WithOne(o => o.Product)
                .HasForeignKey(p => p.ProductId);
            });

            modelBuilder.Entity<Customer>(c =>
            {
                c.Property(c => c.LastName).HasMaxLength(50).IsRequired();
                c.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
                c.Property(c => c.MiddleName).HasMaxLength(50);
                c.Property(c => c.PhoneNumber).HasMaxLength(11).IsRequired();
                c.Property(c => c.Email).HasMaxLength(255).IsRequired();

                c.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(p => p.CustomerId);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                //.LogTo(System.Console.WriteLine)
                .UseLazyLoadingProxies()
                .UseSqlServer("Data Source=Zlobin\\SQLEXPRESS; Initial Catalog=ShopEf; Integrated Security=True; MultipleActiveResultSets=True;");
        }
    }
}