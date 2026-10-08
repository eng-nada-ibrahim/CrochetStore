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

            // Create Default Admin User
            var adminEmail = "admin@crochetstore.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "مدير متجر الكروشيه",
                    EmailConfirmed = true,
                    PhoneNumber = "01000000000",
                    City = "القاهرة",
                    Address = "شارع المعز - القاهرة"
                };

                var createResult = await userManager.CreateAsync(newAdmin, "Admin@123456");
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
