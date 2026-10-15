using Microsoft.EntityFrameworkCore;
using AllianceInventoryApi.Models;
using Microsoft.Extensions.Configuration;

namespace AllianceInventoryAPI.Data
{
    public partial class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) 
        {
       
        }

        public DbSet<ItemModel> Items { get; set; } = null!;
    }
}
