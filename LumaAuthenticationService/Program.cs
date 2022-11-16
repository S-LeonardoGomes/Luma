using LumaAuthenticationService.Data;
using LumaAuthenticationService.Data.Context;
using LumaAuthenticationService.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthenticationServiceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationServiceConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/*
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthenticationServiceContext>();
    db.Database.Migrate();
}
*/
app.Run();
