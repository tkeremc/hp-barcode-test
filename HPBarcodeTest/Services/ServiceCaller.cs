using HPBarcodeTest.DbContext;
using HPBarcodeTest.Interfaces;

namespace HPBarcodeTest.Services;

public sealed class ServiceCaller
{
    public static void Register(IServiceCollection services)
    {
        ScopedServices(services);
        SingletonServices(services);
    }

    private static void ScopedServices(IServiceCollection services)
    {
        services.AddScoped<IBarcodeService, BarcodeService>();
        services.AddScoped<IUserService, UserService>();
        
    }

    private static void SingletonServices(IServiceCollection services)
    {
        services.AddScoped<IMongoDbContext, MongoDbContext>();
    }
}