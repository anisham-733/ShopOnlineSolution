using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.API.Data;
using ShopOnline.API.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopOnlineDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"));
});

//same instance of object is created withing a particular http request
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(policy=>policy.WithOrigins("https://localhost:7052", "http://localhost:7052").AllowAnyMethod().WithHeaders(HeaderNames.ContentType));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
