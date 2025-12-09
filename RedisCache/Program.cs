using Microsoft.Extensions.DependencyInjection;
using RedisCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();



// Redis Cache
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Database Context 
services.AddDbContext<NorthwindContext>(options =>
{
    options.UseSqlServer("Server=**;Database=**;User ID=**;Password=**;Encrypt=False;TrustServerCertificate=True;");
});

// Register ShipperService 
services.AddScoped<IShipperService, ShipperService>();

var provider = services.BuildServiceProvider();

using (var scope = provider.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IShipperService>();

    Console.WriteLine("---- First Call ----");
    var s1 = await service.GetShipperAsync(2);
    Console.WriteLine($"{s1.ShipperID} - {s1.CompanyName} - {s1.Phone}");

    Console.WriteLine("---- Second Call ----");
    var s2 = await service.GetShipperAsync(2);
    Console.WriteLine($"{s2.ShipperID} - {s2.CompanyName} - {s2.Phone}");

    Console.WriteLine("---Benchmark---");
    await service.BenchmarkShippersAsync(2);
}

Console.WriteLine("Done.");
