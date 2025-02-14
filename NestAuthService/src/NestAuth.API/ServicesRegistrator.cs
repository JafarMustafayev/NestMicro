namespace NestAuth.API;

public static class ServicesRegistrator
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var subsection = string.Empty;

            subsection = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue("ServerIP")).Result ? "SqlConnectionOnServer" : "SqlConnectionOnPrem";

            options.UseSqlServer(Configuration.GetConfiguratinValue("ConnectionStrings", subsection));
        });

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }

    public static async void AddMassTransit(this IServiceCollection services)
    {
    }
}