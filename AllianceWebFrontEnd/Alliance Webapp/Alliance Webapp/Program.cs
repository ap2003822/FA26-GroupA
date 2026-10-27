using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Configure JWT auth
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        // grab the key and throw if not found
        var key = builder.Configuration["Jwt:Key"]
                  ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)   // warning gone, since `key` is non-null
            )
        };

        // events...
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = async context =>
            {
                Console.WriteLine("OnAuthFailed");
                await Task.CompletedTask;
            },
            OnChallenge = async context =>
            {
                Console.WriteLine("OnChallenge");
                context.HandleResponse();
                if (context.AuthenticateFailure != null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(
                        "Token Validation Has Failed. Request Access Denied");
                }
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
