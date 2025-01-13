using Microsoft.AspNetCore.Identity;

namespace MovieReservationSystem.Data {
    public static class DataExtension {

        public static async Task SeedData(this WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                string adminEmail = "wonyoung@gmail.com";
                string adminPassword = "Wonyoung@123";

                var roles = new string[] { "Admin", "User" };
                foreach (var role in roles) {
                    if (!await roleManager.RoleExistsAsync(role)) {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                if (await userManager.FindByEmailAsync(adminEmail) == null) {
                    var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded) {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }
}
