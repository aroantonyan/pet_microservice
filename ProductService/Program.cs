using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceContracts;
using ProductService.Data;
using ProductService.Events;
using ProductService.Extensions;
using ProductService.Models;
using ProductService.Services;
using ProductService.Services.TokenGenerator;

var builder = WebApplication.CreateBuilder(args);


var jwt = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));


builder.Services.AddStackExchangeRedisCache(o =>
    o.Configuration = builder.Configuration.GetConnectionString("Redis"));

builder.Services.AddGrpcClient<PriceService.PriceServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["Grpc:PriceUrl"]!));

builder.Services.AddHttpClient<LogSenderService>((sp, c) =>
    c.BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>()["LogService:LogServiceHost"]!));


builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("ProductDb")));

builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton<ProductEventSubscriber>();
builder.Services.AddScoped<AccessTokenGenerator>();
builder.Services.AddScoped<RefreshTokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = key
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSwaggerWithJwt();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();