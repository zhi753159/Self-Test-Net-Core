using Microsoft.EntityFrameworkCore;
using api_server.Models;
using Microsoft.Extensions.Configuration;

namespace api_server.Data
{
    public class ProjectContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public ProjectContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connection_str = Configuration.GetConnectionString("ApplicationDatabase");
            Console.WriteLine($"Connecting to MSSQL Database: {connection_str}");
            optionsBuilder.UseSqlServer(connection_str);
        }
    }
}
