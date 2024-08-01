using Blog.Data.Concrete.EfCore;
using Blog.Entity;
using Blog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(
    options => 
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("SQLite_Connection");
    options.UseSqlite(connectionString);
});


builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<BlogContext>()
    .AddDefaultTokenProviders();
    
builder.Services.AddScoped<RoleService>();

builder.Services.Configure<IdentityOptions>(options => 
{
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

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


SeedData.TestUser(app);
app.Run();
