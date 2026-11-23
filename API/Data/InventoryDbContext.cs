using System.ComponentModel;
using System.Reflection.Metadata;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace AllianceInventoryApi.API.Data
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

        public virtual DbSet<Item> Item { get; set; } = null!;
        public virtual DbSet<UserModel> User { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var cs = _config["ConnectionStrings:DefaultConnection"];
                optionsBuilder.UseSqlite(cs);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(x => x.Item_Image).HasColumnType("blob");
            });

        }
    }
}
