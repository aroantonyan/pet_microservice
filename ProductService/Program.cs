using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceContracts;
using ProductService;
using ProductService.Data;
using ProductService.Interfaces;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<LogSenderService>((provider, client) =>
{
    var cfg = provider.GetRequiredService<IConfiguration>();

    var baseUrl = cfg["LogService:LogServiceHost"] ?? throw new InvalidOperationException("LogServiceHost not configured");

    client.BaseAddress = new Uri(baseUrl);  
});
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ProductDb")));

builder.Services.AddGrpcClient<PriceService.PriceServiceClient>(o =>
{
    o.Address = new Uri(
        builder.Configuration["Grpc:PriceUrl"]     
        ?? throw new InvalidOperationException(
            "Grpc:PriceUrl not configured"));
});


builder.Services.AddScoped<IProductInterface, ProductService.Services.ProductService>();
var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
