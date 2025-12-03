using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    internal class RedisHelper
    {
        private static IDatabase redisDatabase;
        public static IDatabase Database
        {
            get
            {
                return redisDatabase;
            }
        }
        static RedisHelper()
        {
            // creating connection
            var connection = ConnectionMultiplexer.Connect("localhost:6379");
            redisDatabase=connection.GetDatabase();


        }
    }
}
