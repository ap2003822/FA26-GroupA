using Microsoft.EntityFrameworkCore;
using AllianceInventoryApi.Models;
using Microsoft.Extensions.Configuration;

namespace AllianceInventoryAPI.Data
{
    public partial class InventoryDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public InventoryDbContext(IConfiguration config)
        {
            _config = config;
        }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IConfiguration config) : base(options) 
        {
            _config = config;
        }

        public virtual DbSet<ItemModel> Items { get; set; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=AllianceInventory.db");
        }
    }
}
