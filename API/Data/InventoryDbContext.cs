using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.API.Data
{
    public partial class InventoryDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public InventoryDbContext(IConfiguration config)
        {
            config = _config;
        }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IConfiguration config) : base(options) 
        {
            _config = config;
        }

        public DbSet<Item> Item { get; set; } = null!;
    }
}
