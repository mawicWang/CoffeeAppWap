using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMS.Core;

namespace URLRewriter
{
    public class CacheHelper
    {
        private const string sCache_Region = "URLRewriter";
        public static object GetCacheItem(string sKey)
        {
            return Container.CacheService.LocalCache.GetItem(sCache_Region, sKey);
        }
        public static void SetCacheItem(string sKey,object obj)
        {
            Container.CacheService.LocalCache.SetItem(sCache_Region,sKey,obj,System.Int32.MaxValue);
        }
    }
}
