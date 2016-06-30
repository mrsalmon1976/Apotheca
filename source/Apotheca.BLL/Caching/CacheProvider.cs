using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Apotheca.BLL.Caching
{
    public interface ICacheProvider
    {
        T GetCacheable<T>(string cacheKey, Func<T> cachedMethod, DateTime? absoluteExpiration, TimeSpan? slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal, CacheItemRemovedCallback onRemoveCallback = null);
    }

    public class CacheProvider : ICacheProvider
    {

        public T GetCacheable<T>(string cacheKey, Func<T> cachedMethod, DateTime? absoluteExpiration, TimeSpan? slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal, CacheItemRemovedCallback onRemoveCallback = null)
        {
            var cache = HttpRuntime.Cache;
            object item = cache.Get(cacheKey);
            if (item == null)
            {
                item = cachedMethod();
                cache.Add(cacheKey, item, null, (absoluteExpiration ?? Cache.NoAbsoluteExpiration), (slidingExpiration ?? Cache.NoSlidingExpiration), priority, onRemoveCallback);
            }
            return (T)item;
        }
    }
}
