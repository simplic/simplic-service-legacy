using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Cache.Test
{
    public class CacheTestObject : ICacheObject
    {
        public string CacheKey
        {
            get;
            set;
        }
    }
}
