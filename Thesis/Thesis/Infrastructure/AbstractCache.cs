using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Thesis.Infrastructure
{
    public class AbstractCache : ICache
    {
        
        private Cache Cache { get { return HttpContext.Current.Cache; } }

        public object Get(string key)
        {
            if (key == null) return null;
            return Cache[key];
        }

        public void Invalidate(string key)
        {
            if (key != null)
                Cache.Remove(key);
        }

        public bool IsSet(string key)
        {
            if (key == null) return false;
            else
                return (Cache[key] != null);
        }

        public void Set(string key, object data, int cacheTime)
        {
            if(key == null)
                Cache.Insert("defaultCache", data, null, DateTime.Now + TimeSpan.FromMinutes(cacheTime), Cache.NoSlidingExpiration);
            else
                Cache.Insert(key, data, null, DateTime.Now + TimeSpan.FromMinutes(cacheTime), Cache.NoSlidingExpiration);
        }
    }
}