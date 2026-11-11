using System.Text;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Repository;
using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace AllianceInventoryApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();

           
            // Add JWT authentication and configure token validation parameters
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;   // keep short claim names: email, role
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    NameClaimType = "email",
                    RoleClaimType = "role"
                };
            });

            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Alliance Inventory",
                    Version = "v1"
                });
            });
            builder.Services.AddScoped<IItem, ItemDAL>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                foreach (var u in db.User.ToList())
                {
                    if (!string.IsNullOrEmpty(u.User_Password) && !u.User_Password.StartsWith("AQAAAA"))
                        u.User_Password = PasswordHelper.Hash(u, u.User_Password);
                }
                db.SaveChanges();


            }
            app.MapGet("/debug/database", async (
                InventoryDbContext context) =>
            {
                bool canConnect = await context.Database.CanConnectAsync();

                return Results.Ok(new
                {
                    Connected = canConnect,
                    Database = context.Database.GetDbConnection().DataSource
                });
            });


            app.UseHttpsRedirection();

            app.UseAuthentication(); // Add this line to enable authentication

            app.UseAuthorization();

            app.MapControllers();

            app.Run();


        }
    }
}