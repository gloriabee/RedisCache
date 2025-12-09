using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    internal class NorthwindContext: DbContext
    {
      
        public NorthwindContext(DbContextOptions<NorthwindContext> options):base(options) {

        }
        public DbSet<Shipper> Shippers { get; set; }
    }
}
