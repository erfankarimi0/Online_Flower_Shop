using Flora.Data;
using Flora.Services;
using Flora.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<FloraContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FloraConnection")));

// BuyerService
builder.Services.AddScoped<IBuyerService, BuyerService>();

// TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!)
            ),

            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // برای فهمیدن دلیل دقیق 401
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("========== JWT ERROR ==========");
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine("================================");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("========== JWT VALID ==========");
                Console.WriteLine("Token is valid!");
                Console.WriteLine("================================");

                return Task.CompletedTask;
            }
        };
    });

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


/*using Flora.Data;
using Flora.Services;
using Flora.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// اتصال FloraContext به دیتابیس
builder.Services.AddDbContext<FloraContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FloraConnection")));

// BuyerService DI
builder.Services.AddScoped<IBuyerService, BuyerService>();

// TokenService DI
builder.Services.AddScoped<ITokenService, TokenService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!)
            ),

            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // نمایش دلیل خطای JWT در زمان تست
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("========== JWT ERROR ==========");
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine("================================");

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// HTTP Request Pipeline
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();*/