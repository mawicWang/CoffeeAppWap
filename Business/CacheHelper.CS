using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public abstract class CacheHelper
    {
        static string sRegionName = "AppWapXiaoBao";

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCachedItem(string key)
        {
            return XMS.Core.Container.CacheService.GetItem(sRegionName, key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCachedItem(string key, object value, int ntimeToLiveInSeconds = 60*5)
        {
            XMS.Core.Container.CacheService.SetItem(sRegionName, key, value, ntimeToLiveInSeconds);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveCacheItem(string key)
        {
            return XMS.Core.Container.CacheService.RemoveItem(sRegionName, key);
        }

        public static object GetCachedItemAndSet(string key, Func<object, object> callback, object callBackState)
        {
            return XMS.Core.Container.CacheService.GetAndSetItem(sRegionName, key, callback, callBackState);
        }
    }
}
