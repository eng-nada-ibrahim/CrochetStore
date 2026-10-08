using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CrochetStore.Models;

namespace CrochetStore.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Category -> Product (Restrict delete if products exist)
            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderItem (Cascade delete when order is deleted)
            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> OrderItem (Restrict delete of product if it is in an order)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> ShoppingCartItem (Cascade delete if product removed)
            builder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany()
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Initial Categories
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "شيلان وكوفيات",
                    Description = "شيلان كروشيه راقية وكوفيات شتوية دافئة محاكة يدوياً بأجود خيوط الصوف والموهير والقطن التركي.",
                    ImageUrl = "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?auto=format&fit=crop&w=600&q=80",
                    IsActive = true
                },
                new Category
                {
                    Id = 2,
                    Name = "بلوزات وسويترات",
                    Description = "ملابس كروشيه عصرية وتصاميم مميزة تناسب جميع الفصول بأنماط غرز فريدة وألوان متناسقة.",
                    ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?auto=format&fit=crop&w=600&q=80",
                    IsActive = true
                },
                new Category
                {
                    Id = 3,
                    Name = "كروشيه أطفال",
                    Description = "أطقم مواليد، فساتين بنات، بطانيات أطفال وألعاب أميجورومي آمنة وناعمة جداً على بشرة الأطفال.",
                    ImageUrl = "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&q=80",
                    IsActive = true
                },
                new Category
                {
                    Id = 4,
                    Name = "إكسسوارات وهدايا",
                    Description = "حقائب يد، مفارش، كوستر، بندانات، وميداليات مصنوعة بحب وإتقان لتكون هدية تذكارية رائعة.",
                    ImageUrl = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=600&q=80",
                    IsActive = true
                }
            );

            // Seed Initial Products
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "شال كروشيه مثلث بغرزة الجراني",
                    Description = "شال يدوي فاخر مصنوع من خيوط الموهير والأكريليك التركي الناعم، يمنحك دفئاً وأناقة لا مثيل لها.",
                    Price = 450.00m,
                    StockQuantity = 10,
                    ImageUrl = "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?auto=format&fit=crop&w=600&q=80",
                    YarnType = "موهير مخلوط بأكريليك ناعم",
                    SizeOptions = "مقاس حر (Free Size)",
                    ProcessingDays = 4,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 1,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Name = "كارديجان كروشيه بوحدات الورد المربعة",
                    Description = "كارديجان بوهيمي كروشيه أنيق جداً مصنوع من وحدات الورود الملونة بخيوط قطن 100% طبيعية.",
                    Price = 850.00m,
                    StockQuantity = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?auto=format&fit=crop&w=600&q=80",
                    YarnType = "قطن مصري 100%",
                    SizeOptions = "S, M, L, XL, تفصيل خاص",
                    ProcessingDays = 7,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 2,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Name = "طقم سبوع بيبي كامل (سالوبيت + حذاء + قبعة)",
                    Description = "طقم مواليد فاخر مصنوع من خيوط أليزا بيبي ضد الحساسية وناعم جداً على بشرة حديثي الولادة.",
                    Price = 520.00m,
                    StockQuantity = 8,
                    ImageUrl = "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&q=80",
                    YarnType = "خيوط بيبي قطن هيبوإليرجينيك",
                    SizeOptions = "0-3 شهور, 3-6 شهور, 6-12 شهر",
                    ProcessingDays = 3,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 3,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 4,
                    Name = "حقيبة يد كروشيه دائرية بخيط المكرمية",
                    Description = "شنطة كروس أنيقة وعملية مصنوعة من خيط المكرمية القطني مع يد جلدية طبيعية وبطانة ستان داخلية.",
                    Price = 380.00m,
                    StockQuantity = 15,
                    ImageUrl = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=600&q=80",
                    YarnType = "مكرمية قطن مصري",
                    SizeOptions = "قطر 24 سم",
                    ProcessingDays = 2,
                    IsCustomizable = true,
                    IsFeatured = true,
                    CategoryId = 4,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
