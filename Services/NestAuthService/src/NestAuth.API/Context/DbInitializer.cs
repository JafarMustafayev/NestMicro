namespace NestAuth.API.Context;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            string[] roles = { Roles.SuperAdmin, Roles.Admin, Roles.Vendor, Roles.Customer, Roles.Support, Roles.Accountant, Roles.InventoryManager };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            var defaultUser = await userManager.FindByEmailAsync("admin@example.com");
            if (defaultUser == null)
            {
                var newUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    UserStatus = UserStatus.Active,
                };

                var result = await userManager.CreateAsync(newUser, "Admin@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, Roles.SuperAdmin);
                }
            }
        }
    }
}