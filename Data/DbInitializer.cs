using Microsoft.AspNetCore.Identity;

namespace CrochetStore.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create Default Admin User (حساب الأدمن الخاص بكِ)
            var adminEmail = "admin_nada@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "ندا (مدير المتجر)",
                    EmailConfirmed = true,
                    PhoneNumber = "01000000000",
                    City = "القاهرة",
                    Address = "القاهرة"
                };

                var createResult = await userManager.CreateAsync(newAdmin, "nada1682005");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // Create Default Demo Customer
            var customerEmail = "customer@example.com";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                var newCustomer = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    FullName = "سارة أحمد",
                    EmailConfirmed = true,
                    PhoneNumber = "01111111111",
                    City = "الإسكندرية",
                    Address = "سموحة - الإسكندرية"
                };

                var createResult = await userManager.CreateAsync(newCustomer, "Customer@123456");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newCustomer, "Customer");
                }
            }
        }
    }
}