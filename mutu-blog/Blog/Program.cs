using Blog.Data.Concrete.EfCore;
using Blog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:SQLite_Conntection"]));

// builder.Services.AddIdentity<AppUser, AppRole>(options )
// {
//     .AddEntityFramworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders()
//     .AddDefaultUI();
// }

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();
    
builder.Services.AddScoped<RoleService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
    var roles = new[] {"Admin","Blogger","User"};

    foreach(var role in roles)
    {
        var result = roleService.CreateRoleAsync(role).GetAwaiter().GetResult();
        if(result.Succeeded)
        {
            Console.WriteLine($"Role {role} created successfully");
        }
        else
        {
            Console.WriteLine($"Role {role} creation failed");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
