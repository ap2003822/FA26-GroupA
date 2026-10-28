using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
