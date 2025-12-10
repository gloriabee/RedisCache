using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public class RedisPracticeService
    {
        private readonly IDatabase _redis;
        public RedisPracticeService()
        {
            _redis = RedisHelper.Database;
        }

        // 1. Strings
        public async Task PracticeStrings()
        {
            Console.WriteLine("==== Strings======");
            await _redis.StringSetAsync("name", "John Doe");
            await _redis.StringSetAsync("age", 30);
            await _redis.StringIncrementAsync("age", 10);

            await _redis.KeyExpireAsync("name", TimeSpan.FromSeconds(60));

            Console.WriteLine("name: string " + await _redis.StringGetAsync("name"));
            Console.WriteLine("age: number " + await _redis.StringGetAsync("age"));
        }


        // 2. Hashes 
        public async Task PracticeHashes()
        {
            Console.WriteLine("====Hashes=====");
            // setting multiple fields at once
            await _redis.HashSetAsync("shipper:1", new HashEntry[]
            {
                new HashEntry("company", "johndoe"),
                new HashEntry("address", "Chiangmai")
            });

            // setting single field 
            await _redis.HashSetAsync("Shipper:1", "phone", "09324342");

            // getting all fields
            var hash= await _redis.HashGetAllAsync("shipper:1");

            foreach (var entry in hash)
            {
                Console.WriteLine($"{entry.Name}: {entry.Value}");
            }

            // Removing hash field
            await _redis.HashDeleteAsync("shipper:1", "address");
        }


        // 3. Lists 
        public async Task PracticeLists()
        {
            Console.WriteLine("==== Lists =====");
            await _redis.ListRightPushAsync("tasks", "Task 1");
            await _redis.ListRightPushAsync("tasks", "Task 4");
            await _redis.ListLeftPushAsync("tasks", "Task 2");

            var tasks= await _redis.ListRangeAsync("tasks", 0, -1);
            Console.WriteLine("List items: ");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            var popped= await _redis.ListLeftPopAsync("tasks");
            Console.WriteLine("Popped: "+popped);
        }

        // 4. Sets
        public async Task PracticeSets()
        {
            Console.WriteLine("==== Sets ====");
            await _redis.SetAddAsync("Categories", "Electronics");
            await _redis.SetAddAsync("Categories", "Books");
            await _redis.SetAddAsync("Categories", "Clothing"); 
            await _redis.SetAddAsync("Categories", "Books");


            var categories = _redis.SetMembers("Categories");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }

            bool exists= await _redis.SetContainsAsync("Categories", "Books");
            Console.WriteLine("Books exists: "+exists);

            await _redis.SetRemoveAsync("Categories", "Clothing");

        }

        // 5. Sorted Sets
        public async Task PracticeSortedSets()
        {
            Console.WriteLine("==== Sorted Sets ====");
            await _redis.SortedSetAddAsync("leaderboard", "Player1", 100);
            await _redis.SortedSetAddAsync("leaderboard", "Player2", 150);
            await _redis.SortedSetAddAsync("leaderboard", "Player3", 120);
            var topPlayers = await _redis.SortedSetRangeByScoreWithScoresAsync("leaderboard", order: Order.Descending);
            Console.WriteLine("Leaderboard:");
            foreach (var player in topPlayers)
            {
                Console.WriteLine($"{player.Element}: {player.Score}");
            }
            // Update score
            await _redis.SortedSetIncrementAsync("leaderboard", "Player1", 50);
        }

        // 6. JSON
        public async Task PracticeJson()
        {
            Console.WriteLine("=== JSON ===");
            var shipper = new
            {
                Id = 100,
                Name = "Fast Delivery",
                Phone = "3243242342"
            };

            string json= JsonConvert.SerializeObject(shipper);

            await _redis.StringSetAsync("shipper:100", json);
            var result= await _redis.StringGetAsync("shipper:100");
            Console.WriteLine("JSON: "+result);
        }

        // 7. Key operations 
        public async Task PracticeKeyOperations()
        {
            Console.WriteLine("=== Key Operations ");

            await _redis.StringSetAsync("key:test", "value");
            Console.WriteLine("Exists? " + await _redis.KeyExistsAsync("key:test"));

            await _redis.KeyExpireAsync("key:test", TimeSpan.FromSeconds(30));
            Console.WriteLine("TTL: " + await _redis.KeyTimeToLiveAsync("key:test"));

            await _redis.KeyDeleteAsync("key:test");
            Console.WriteLine("Deleted. Exists? " + await _redis.KeyExistsAsync("key:test"));


        }


        // 8. Search keys 
       
        public async Task PracticeSearchKeys()
        {
            Console.WriteLine("=== Key Searching by Pattern ====");
            string pattern = "shipper*";
            foreach(var key in _redis.Multiplexer.GetServer("localhost:6379").Keys(pattern: pattern))
            {
                Console.WriteLine("Found key: "+key);
            }
        }
        
        
    }
}
