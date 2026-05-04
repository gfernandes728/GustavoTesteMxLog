using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Data;
using GustavoTesteMxLog.Infra.Interfaces;

namespace GustavoTesteMxLog;

public class AppDbContextInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        context.Database.EnsureCreated();

        if (!context.Users.Any() && userRepository != null)
        {
            var user = User.TryCreate
            (
                name:  "admin_mxlog",
                email: "admin@mxlog.com.br",
                password: passwordHasher.Hash("admin123")
            );

            await userRepository?.AddAsync(user);
        }
    }
}
