using RedisCache;

var cache = RedisHelper.Database;
//set key-value pair
await cache.StringSetAsync("OURKEY", "GLORIA");
//get value by key
var value=await cache.StringGetAsync("OURKEY");

for(int i=0;i<100;i++)
{
    await cache.StringSetAsync($"OURKEY_{i}",$"GLORIA_{i}");
}

for (int i = 0; i < 100; i++)
{
    Console.WriteLine(await cache.StringGetAsync($"OURKEY_{i}"));
}


