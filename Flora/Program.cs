using Flora.Data;
using Flora.Services;
using Flora.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add services to the container.  AddSwaggerGen

builder.Services.AddControllers();
///builder.Services.AddSwaggerGen();
///builder.Services.AddOpenApi();

//DI و وصل connectionstring به floracontext و ...
builder.Services.AddDbContext<FloraContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FloraConnection")));
//BuyerSevice DI
builder.Services.AddScoped<IBuyerService, BuyerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.    AddSwaggerGen

/*if (app.Environment.IsDevelopment())
{
 ///   app.UseSwagger();
   /// app.UseSwaggerUI();
    ///app.MapOpenApi();
}*/


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
