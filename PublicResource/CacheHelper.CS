using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnitTest,PublicKey=null")]
namespace PublicResource
{
    public class CacheHelper
    {
        private static CacheHelper _objInstance = null;
        public static CacheHelper objInstance
        {
            get
            {
                if (_objInstance == null)
                    _objInstance = new CacheHelper();
                return _objInstance;
            }
        }
        internal class CachedItem
        {
            public object obj;
            public DateTime tValidTime;
            public DateTime tFileLastModifyTime;
            public string sFileName;
        };
        private Hashtable _ht = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCacheName"></param>
        /// <returns></returns>
        public object GetCachedItem(string sCacheName)
        {
            if (String.IsNullOrEmpty(sCacheName))
                return null;
            if (_ht[sCacheName] == null)
                return null;
            CachedItem obj = (CachedItem)_ht[sCacheName];
            if (obj.tValidTime < System.DateTime.Now)
            {
                ChangeHtValue(sCacheName, null, 0, null, true);
                return null;
            }
            if (obj.tFileLastModifyTime != GetLastModifyTime(obj.sFileName))
            {
                ChangeHtValue(sCacheName, null, 0, null, true);
                return null;
            }
            return obj.obj;


        }
        /// <summary>
        /// 设置缓存，如果过期时间跟文件关联缓存都设置，先判断过期时间， 再判断文件是否已更改
        /// </summary>
        /// <param name="sCacheName">为了防止跟其他人的Cache冲突，要求length>=5,如小于5 会抛出Exception</param>
        /// <param name="nCacheLengthInSeconds">过期时间， 单位s，如小于等于0,无限期缓存</param>
        /// <param name="obj">不能为空，为空将抛出Exception</param>
        /// <param name="sFileName">文件关联缓存名，可以为全路径，不是全路径，在当前目录下，不需要文件关联缓存，可为null</param>
        public void SetCachedItem(string sCacheName, int nCacheLengthInSeconds, object obj, string sFileName)
        {
            if (String.IsNullOrEmpty(sCacheName))
                throw (new Exception("No CacheName!"));
            if (sCacheName.Length < 5)
            {
                throw (new Exception("CacheName is too simple,length must be bigger then 5!"));
            }

            if (obj == null)
            {
                throw (new Exception("object is null!"));
            }


            ChangeHtValue(sCacheName, obj, nCacheLengthInSeconds, sFileName, false);
        }


        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="sCacheName">缓存名</param>
        public void ClearCache(string sCacheName)
        {
            ChangeHtValue(sCacheName, null, 0, "", true);
        }

        private string GetRealFilePath(string sPath)
        {
            if (String.IsNullOrEmpty(sPath))
                return sPath;
            if (sPath.IndexOf(':') >= 0)
                return sPath;
            else
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sPath);
        }
        private object objLock = new object();

        private void ChangeHtValue(string sCacheName, object obj, int nCacheLengthInSeconds,
            string sFileName, bool bIsOnlyRemove)
        {

            lock (objLock)
            {
                _ht.Remove(sCacheName);
                if (!bIsOnlyRemove)
                {
                    CachedItem obj1 = new CachedItem();
                    obj1.obj = obj;
                    if (nCacheLengthInSeconds <= 0)
                    {
                        obj1.tValidTime = System.DateTime.MaxValue;
                    }
                    else
                    {
                        obj1.tValidTime = System.DateTime.Now.AddSeconds(nCacheLengthInSeconds);
                    }
                    obj1.sFileName = sFileName;
                    obj1.tFileLastModifyTime = GetLastModifyTime(sFileName);
                    _ht[sCacheName] = obj1;
                }

            }

        }
        private DateTime GetLastModifyTime(string sPath)
        {
            sPath = GetRealFilePath(sPath);
            if (!String.IsNullOrEmpty(sPath))
            {
                if (File.Exists(sPath))
                    return File.GetLastWriteTime(sPath);
            }
            return System.DateTime.MaxValue;
        }

    }
}
