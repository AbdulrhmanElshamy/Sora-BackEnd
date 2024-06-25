using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Sofra;
using Sofra.Api.Models;
using Sofra.Api.Seeds;
using Sofra.API.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencies(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();

var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

await DefaultRoles.SeedAsync(roleManger);
await DefaultUsers.SeedAsync(userManger);

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
//    RequestPath = "/Resources"
//});
app.MapControllers();

app.Run();
