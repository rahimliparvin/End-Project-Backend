

using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Service.Interfaces;
using Savoy.Service;
using Microsoft.AspNetCore.Identity;
using Savoy.Models;
using Savoy.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    //bu o demekdir ki Password'un minimum uzunlugu 8 characterden ibaret olsun !
    opt.Password.RequireDigit = true;
    //Reqem olmasi Shertdir !
    opt.Password.RequireLowercase = true;
    //Kicik herf olmasi Shertdir !
    opt.Password.RequireUppercase = true;
    //Boyuk herif olmasi Shertdir !
    opt.Password.RequireNonAlphanumeric = true;
    //Simvol olmasi Shertdir !


    opt.User.RequireUniqueEmail = true;
    //Email unique olmalidir !
    opt.SignIn.RequireConfirmedEmail = true;
    //Email confirmed(yeni tesdiqlenme) ucundur !
    opt.Lockout.MaxFailedAccessAttempts = 3;
    //(LOGIN)Yeniden cehd max 3 defe !
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    //(LOGIN)Bu o demekdir 3 defe sehv etdise 30deq bloka dusecek ve gozlemeli olacaq !
    opt.Lockout.AllowedForNewUsers = true;
    //Yeni qeydiyyatdan kecen userlere qadagalari qoymasin(3 defe yeniden yoxlanis ve s.) !


});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();


var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
