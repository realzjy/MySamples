using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCacheDemo
{
    class Program
    {
        static string CacheKeyword = "testkeyword";

        static string[] keyvalues = { "hotel", "28", "hotels", "28", "holiday hotel", "28", "a hotel", "29", "dinnig", "29", "restaurant", "30", "hotel", "30", "hotel", "31" };

        class keyvalue { public string @key { get; set; } public string @value { get; set; } }

        static string searchword = @"let me find a hotel or some hotels";

        static void Main(string[] args)
        {
            List<keyvalue> list = new List<keyvalue>();
            for (int i = 0; i < 16; i = i + 2)
            {
                keyvalue kv = new keyvalue() { key = keyvalues[i], value = keyvalues[i + 1] };
                list.Add(kv);
            }

            //var findlist = from f in list
            //               where searchword.Contains(f.key)
            //               select new { f.key, f.value };
            var grouplist = from f in list
                            where searchword.Contains(f.key)
                            group f by f.value into g
                            orderby g.Count() descending
                            select g.FirstOrDefault();
            keyvalue first = grouplist.FirstOrDefault();

            //var findlist = list.Where(o => searchword.Contains(o.key)).ToList();
            //var grouplist = findlist.GroupBy(o => o.value).ToList();
            //var desclist = grouplist.OrderByDescending(o => o.)


            for (int i = 0; i < 16; i = i + 2)
            {
                bool radd = AddItem(keyvalues[i], keyvalues[i + 1]);
            }

            MemoryCache cache = MemoryCache.Default;
            var vv = cache.ToList();

            Console.ReadLine();
        }


        static object GetItem(string key)
        {
            MemoryCache cache = MemoryCache.Default;
            return cache[key];
        }
        static bool AddItem(string @key, string @value)
        {
            MemoryCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = new TimeSpan(0, 30, 0);
            return cache.Add(@key, @value, policy);
        }
    }
}
