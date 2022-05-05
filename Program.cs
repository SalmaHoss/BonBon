using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AngularProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AngularProject.Models;
using AngularProject.Data.Cart;
using AngularProject;

var builder = WebApplication.CreateBuilder(args);
//3
var MyAllowSpecificOrigins = "_MyAllowSpecificOrigins";
var connectionString = builder.Configuration.GetConnectionString("DbContextConnection");;

 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<AppDbContext>();



// Add services to the container.
builder.Services.AddScoped<ICategoryRepository, CategoryRepoService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

builder.Services.AddScoped<IMailService, SendGridMailService>();
//builder.Services.AddTransient<IMailService, SendGridMailService>(); //Confirmation Mail

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 5;

    }).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth => 
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true
        };

});

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

//1
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddRazorPages();  //so we can access ResetPassword page

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));
//builder.Services.AddSession();
//2
*/
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    //options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication()
   .AddGoogle(googleOptions =>
   {
       googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
       googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
   })
   .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = "1123745748470433";
        facebookOptions.AppSecret = "404ba181b770660be894b1c84b01f5d8";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseStaticFiles(); //so we can access ConfirmEmail.html in wwwroot
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//4
app.UseCors(MyAllowSpecificOrigins);

app.Run();
