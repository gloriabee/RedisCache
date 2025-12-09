using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    internal interface IShipperService
    {
        Task<Shipper> GetShipperAsync(int id);
        Task BenchmarkShippersAsync(int id);

    }
}
