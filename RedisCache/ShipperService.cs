using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    internal class ShipperService : IShipperService
    {
        private readonly IDistributedCache _cache;
        private readonly NorthwindContext _db;
        private readonly TimeSpan _absoluteExpiry = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _slidingExpiry = TimeSpan.FromMinutes(3);

        public ShipperService(IDistributedCache cache, NorthwindContext db)
        {
            _cache = cache;
            _db = db;
        }
        public async Task<Shipper> GetShipperAsync(int id)
        {
            string cacheKey = $"shipper:{id}";

            // Step1 :  Try to get the shipper from Redis cache
            var cachedShipper = await _cache.GetStringAsync(cacheKey);
            if (cachedShipper != null)
            {
                Console.WriteLine("Shipper retrieved from cache!");
                return JsonConvert.DeserializeObject<Shipper>(cachedShipper);
            }

            // step2: fetch the shipper from database

            var shipper = await _db.Shippers.FirstOrDefaultAsync(s => s.ShipperID == id);

            if (shipper == null)
            {
                throw new Exception($"Shipper with ID {id} not found.");
            }

            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(shipper),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _absoluteExpiry,
                    SlidingExpiration = _slidingExpiry
                }
                );

            Console.WriteLine(" Cache miss and data saved to cache");
            return shipper;
        }

        public async Task BenchmarkShippersAsync(int id)
        {
            var sw = new Stopwatch();

            // SQL 
            sw.Start();
            var sqlData = await _db.Shippers.FirstOrDefaultAsync(s => s.ShipperID == id);
            sw.Stop();
            Console.WriteLine($"SQL Fetch: {sw.ElapsedMilliseconds} ms");

            // Redis
            sw.Restart();
            var redis= await _cache.GetStringAsync($"shipper:{id}");
            sw.Stop();
            Console.WriteLine($"Redis Fetch: {sw.ElapsedMilliseconds} ms");
        }
    }
}
