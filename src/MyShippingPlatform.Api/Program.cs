using Microsoft.Extensions.DependencyInjection;
using MyShippingPlatform.Application;
using MyShippingPlatform.Infrastructure;
using MyShippingPlatform.Api.Services;
using MyShippingPlatform.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Layer Dependencies
builder.Services.AddApplicationServices(); // MediatR & CQRS
builder.Services.AddInfrastructureServices(builder.Configuration); // EF Core & MySQL

// 2. Add API specific services
builder.Services.AddScoped<ICurrentUserService, MockCurrentUserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
