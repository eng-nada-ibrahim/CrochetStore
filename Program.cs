using CrochetStore.Data;
using CrochetStore.Models;
using CrochetStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Database Context with MySQL (URL Auto-Fix)
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

string connectionString = rawConnectionString;

// إذا كان الرابط يبدأ بـ mysql:// يتم تحويله تلقائياً للصيغة التي تفهمها المكتبة
if (rawConnectionString.StartsWith("mysql://", StringComparison.OrdinalIgnoreCase))
{
    var uri = new Uri(rawConnectionString);
    var userInfo = uri.UserInfo.Split(':');
    var user = userInfo.Length > 0 ? userInfo[0] : "";
    var password = userInfo.Length > 1 ? userInfo[1] : "";
    var dbName = uri.AbsolutePath.TrimStart('/');
    var host = uri.Host;
    var dbPort = uri.Port > 0 ? uri.Port : 3306;

    connectionString = $"Server={host};Port={dbPort};Database={dbName};Uid={user};Pwd={password};SslMode=Preferred;";
}

string connectionString1 = connectionString;
string connectionString11 = connectionString1;

// التعديل هنا: استخدام المزود الرسمي لـ MySQL والدالة UseMySQL المتوافقة مع .NET 10
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString11));

// 2. Add ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

// 3. Add Session Support & HttpContextAccessor for Cart
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(48);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, CartService>();

// 4. Add MVC Controllers & Views
var appPort = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{appPort}");

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Setup Area Route for Admin
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Auto Database Creation, Direct Data Seeding & Identity Roles Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureCreated(); // يعيد إنشاء الجداول بالهيكل الجديد الكامل على Railway

        // 1. إدخال الفئات (Categories) مباشرة إذا كان الجدول فارغاً
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Id = 1001, Name = "شيلان", Description = "قطعة فنية تكتمل بها إطلالتكِ! شال كروشيه فاخر...", ImageUrl = "/images/scarf1.jpg", IsActive = true },
                new Category { Id = 1002, Name = "شنط كروشيه", Description = "تشكيلة مميزة من الحقائب المصنوعة يدوياً...", ImageUrl = "/images/makramia.jpg", IsActive = true },
                new Category { Id = 1003, Name = "كارديجان كروشيه", Description = "دفء يلتقي بالأناقة في كل غرزة!...", ImageUrl = "/images/jaket.jpg", IsActive = true },
                new Category { Id = 1004, Name = "جونتي", Description = "قفازات كروشيه فريدة مصنوعة يدوياً...", ImageUrl = "/images/glaves.jpg", IsActive = true }
            );
            context.SaveChanges();
        }

        // 2. إدخال المنتجات (Products) مباشرة إذا كان الجدول فارغاً
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Id = 1001,
                    Name = "شال الحب",
                    Description = "الطول: 180 سم. العرض: 50 سم. خامة فاخرة ومقاومة للتحبيب.",
                    Price = 300.00m,
                    StockQuantity = 3,
                    ImageUrl = "/images/scarf2.jpg",
                    YarnType = "خيط هيمالايا أنتي بيلينج",
                    SizeOptions = "S,M,L,تفصيل",
                    ProcessingDays = 6,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 1001,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 1002,
                    Name = "شنطه باردو",
                    Description = "خيط الصوف والأكريليك الفاخر يضفي ملمساً ناعماً.",
                    Price = 400.00m,
                    StockQuantity = 2,
                    ImageUrl = "/images/bag w&b.jpg",
                    YarnType = "قطن 100% طبيعي",
                    SizeOptions = "S, M, L, XL, تفصيل خاص",
                    ProcessingDays = 4,
                    IsCustomizable = true,
                    IsFeatured = false,
                    CategoryId = 1002,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 1003,
                    Name = "كارديجان زهرة الشتاء",
                    Description = "كارديجان كروشيه مصنوع يدوياً بعناية بتصميم أنيق.",
                    Price = 850.00m,
                    StockQuantity = 3,
                    ImageUrl = "/images/jaket1.jpg",
                    YarnType = "قطن وصوف عالي الجودة",
                    SizeOptions = "S, M, L, XL, تفصيل خاص",
                    ProcessingDays = 7,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 1003,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 1004,
                    Name = "جونتي القطّة",
                    Description = "مثالي للارتداء اليومي في الشتاء واستخدام الهاتف.",
                    Price = 400.00m,
                    StockQuantity = 4,
                    ImageUrl = "/images/glaves1.jpg",
                    YarnType = "خيط قطن ناعم",
                    SizeOptions = "S, M, L, XL, تفصيل خاص",
                    ProcessingDays = 4,
                    IsCustomizable = true,
                    IsFeatured = false,
                    CategoryId = 1004,
                    CreatedAt = DateTime.Now
                }
            );
            context.SaveChanges();
        }

        // Seed Roles & Default Admin
        DbInitializer.SeedRolesAndAdminAsync(services).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();