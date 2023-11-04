using API.Data;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

//// configure http request
//app.UseCors(builder => builder
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    .AllowCredentials()
//    .WithOrigins("http://localhost:4200"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
   var context = services.GetRequiredService<DataContext>();
   var userManager = services.GetRequiredService<UserManager<AppUser>>();
   var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");

}

app.Run();
