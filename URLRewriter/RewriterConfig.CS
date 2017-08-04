using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Web;
using XMS.Core;
using System.Text.RegularExpressions;

namespace URLRewriter
{
    public class RewriterConfig
    {
        public static RewriterConfig Instance = new RewriterConfig();

        private const string sRewriterFileName = "conf/Rewriter.config";
        private object objLock = new object();
        private bool bIsRewriterConfigExist
        {
            get
            {
                string sCacheKey = "IsRewriterConfigExist";
                object obj = CacheHelper.GetCacheItem("IsRewriterConfigExist");
                if (obj == null)
                {
                    lock (objLock)
                    {
                        obj = CacheHelper.GetCacheItem("IsRewriterConfigExist");
                        if (obj == null)
                        {
                            if (String.IsNullOrWhiteSpace(sRewriterConfigFilePath))
                            {
                                obj = false;
                            }
                            else
                                obj = true;
                            CacheHelper.SetCacheItem(sCacheKey, obj);
                        }
                    }
                }
                return (bool)obj;

            }
        }


        private string sRewriterConfigFilePath
        {
            get
            {
                return GetConfigPath(sRewriterFileName);
            }
        }

        private string GetConfigPath(string sConfigFileName)
        {
            return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, sConfigFileName);

        }
        static string CurrentHost
        {
            get
            {
                string sHost = "www";
                try
                {
                    sHost = HttpContext.Current.Request.Url.Host.Split('.')[0];
                }
                catch { }
                return sHost;
            }
        }

        public RewriterConfig()
        {

        }

        public bool bIsXiaomishuSite
        {
            get
            {
                return GetFromCache<bool>("root/IsXiaomishuSite", true, sRewriterConfigFilePath);
            }
        }

        /// <summary>
        /// 来源为移动搜索时跳转到相应wap链接
        /// </summary>
        public bool IsMobileSearchRedirect
        {
            get { return GetFromCache<bool>("root/IsMobileSearchRedirect", false, sRewriterConfigFilePath); }
        }

        public string MobileSearchHosts
        {
            get { return GetFromCache<string>("root/MobileSearchHosts", "", sRewriterConfigFilePath); }
        }

        public string sDefaultGoHost
        {
            get
            {
                return "www." + sCrntDomain;
            }
        }


        private Regex regMainHost = new Regex(@"(\w+\.)*?(\w+\.\w+)$", RegexOptions.Compiled);
        public string sCrntDomain
        {
            get
            {

                return regMainHost.Replace(HttpContext.Current.Request.Url.Host, "$2");
            }
        }
        /// <summary>
        /// 访问当前应用下的普通页面是否写SafeHost Cookie
        /// </summary>
        public bool bIsWriteSafeHost
        {
            get
            {
                return GetFromCache<bool>("root/WriteSafeHost", true, sRewriterConfigFilePath);

            }
        }


        public int nCookieDayLength
        {
            get
            {
                return GetFromCache<int>("root/CookieDayLength", 1, sRewriterConfigFilePath);
            }
        }



        private List<string> lstSearchEngineRobot
        {
            get
            {
                return GetFromCache<List<string>>("root/SearchEngineRobot", new List<string>(), sRewriterConfigFilePath);
            }
        }
        public List<URLRewriter.Config.RewriterRule> lstMigrationRules
        {
            get
            {
                if (dicOtherConfig.ContainsKey(CurrentHost.ToUpper()))
                {
                    return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/MigrationConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), GetConfigPath(dicOtherConfig[CurrentHost.ToUpper()]));
                }
                return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/MigrationConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), sRewriterConfigFilePath);
            }
        }

        public List<URLRewriter.Config.RewriterRule> lstOldWebRules
        {
            get
            {
                if (dicOtherConfig.ContainsKey(CurrentHost.ToUpper()))
                {
                    return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/OldWebConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), GetConfigPath(dicOtherConfig[CurrentHost.ToUpper()]));
                }
                return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/OldWebConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), sRewriterConfigFilePath);
            }
        }

        public List<URLRewriter.Config.RewriterRule> lstRules
        {
            get
            {
                if (dicOtherConfig.ContainsKey(CurrentHost.ToUpper()))
                {
                    return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/RewriterConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), GetConfigPath(dicOtherConfig[CurrentHost.ToUpper()]));
                }
                return GetFromCache<List<URLRewriter.Config.RewriterRule>>("root/RewriterConfig/Rules", new List<URLRewriter.Config.RewriterRule>(), sRewriterConfigFilePath);

            }
        }

        private Dictionary<string, string> dicOtherConfig
        {
            get
            {
                return GetFromCache<Dictionary<string, string>>("root/OtherConfig", new Dictionary<string, string>(), sRewriterConfigFilePath);
            }
        }




        private object objFileLock = new object();
        private XmlNode GetXMLNodeFromConfigFile(string sNodeName, string sFilePath)
        {
            lock (objFileLock)
            {
                if (!File.Exists(sFilePath))
                    throw new ArgumentException("Config file Cannot find,file=" + sFilePath);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sFilePath);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException("cannot find node,node=" + sNodeName);
                }
                return xmlNode;
            }
        }

        private Dictionary<string, string> GetDictionaryFromConfig(string sNodeName, string sFilePath)
        {
            XmlNode objFatherNode = GetXMLNodeFromConfigFile(sNodeName, sFilePath);
            lock (objFileLock)
            {
                Dictionary<string, string> objRslt = new Dictionary<string, string>();
                foreach (XmlNode objChild in objFatherNode.ChildNodes)
                {
                    if (objChild.Attributes["key"] == null || objChild.Attributes["value"] == null || String.IsNullOrWhiteSpace(objChild.Attributes["key"].InnerText) || String.IsNullOrWhiteSpace(objChild.Attributes["value"].InnerText))
                    {
                        throw new ArgumentException("key or value not exist");
                    }
                    objRslt[objChild.Attributes["key"].InnerText.ToUpper()] = objChild.Attributes["value"].InnerText;
                }
                return objRslt;
            }
        }
        private List<string> GetListFromConfig(string sNodeName, string sFilePath)
        {
            XmlNode objFatherNode = GetXMLNodeFromConfigFile(sNodeName, sFilePath);
            WriteLog("node=" + sNodeName);
            lock (objFileLock)
            {
                List<string> lstRslt = new List<string>();
                foreach (XmlNode objChild in objFatherNode.ChildNodes)
                {
                    lstRslt.Add(objChild.InnerText);
                    WriteLog("Value=" + objChild.InnerText);
                }
                return lstRslt;
            }
        }
        private string GetStringFromConfig(string sNodeName, string sFilePath)
        {
            XmlNode objFatherNode = GetXMLNodeFromConfigFile(sNodeName, sFilePath);
            lock (objFileLock)
            {
                return objFatherNode.InnerText;
            }
        }


        private List<URLRewriter.Config.RewriterRule> GetRewriteRuleFromConfigFile(string sNodeName, string sFilePath)
        {
            lock (objFileLock)
            {

                XmlNode objFatherNode = GetXMLNodeFromConfigFile(sNodeName, sFilePath);
                WriteLog("node=" + sNodeName);
                List<URLRewriter.Config.RewriterRule> _lstMigrationRules = new List<URLRewriter.Config.RewriterRule>();
                foreach (XmlNode xmlNode in objFatherNode.ChildNodes)
                {
                    if (!xmlNode.HasChildNodes || xmlNode.ChildNodes.Count < 2)
                    {
                        continue;
                    }

                    URLRewriter.Config.RewriterRule objRule = new URLRewriter.Config.RewriterRule();

                    objRule.LookFor = xmlNode.ChildNodes[0].InnerText;
                    objRule.SendTo = xmlNode.ChildNodes[1].InnerText;

                    _lstMigrationRules.Add(objRule);
                    WriteLog(" LookFor" + objRule.LookFor);
                    WriteLog(" SendTo" + objRule.SendTo);
                }
                return _lstMigrationRules;
            }
        }


        private T GetFromCache<T>(string sNodeName, T tDefaultvalue, string sFilePath)
        {
            string sCacheName = sFilePath + "_" + sNodeName;
            object obj = CacheHelper.GetCacheItem(sCacheName);
            if (obj != null)
                return (T)obj;
            lock (objLock)
            {
                obj = CacheHelper.GetCacheItem(sCacheName);
                if (obj != null)
                    return (T)obj;
                if (typeof(T) == typeof(List<string>))
                {
                    try
                    {
                        obj = GetListFromConfig(sNodeName, sFilePath);
                    }
                    catch
                    {
                        obj = tDefaultvalue;
                    }
                    CacheHelper.SetCacheItem(sCacheName, obj);
                    return (T)obj;
                }
                if (typeof(T) == typeof(Dictionary<string, string>))
                {
                    try
                    {
                        obj = GetDictionaryFromConfig(sNodeName, sFilePath);
                    }
                    catch
                    {
                        obj = tDefaultvalue;
                    }
                    CacheHelper.SetCacheItem(sCacheName, obj);
                    return (T)obj;
                }
                if (typeof(T) == typeof(List<URLRewriter.Config.RewriterRule>))
                {
                    try
                    {
                        obj = GetRewriteRuleFromConfigFile(sNodeName, sFilePath);
                    }
                    catch (Exception exp)
                    {
                        obj = tDefaultvalue;
                        WriteLog("读取重写配置出错：" + exp.ToString());
                    }
                    CacheHelper.SetCacheItem(sCacheName, obj);
                    return (T)obj;
                }
                string sConfigedText = "";
                if (bIsRewriterConfigExist)
                {
                    try
                    {
                        sConfigedText = GetStringFromConfig(sNodeName, sRewriterConfigFilePath);
                    }
                    catch
                    {
                    }
                }
                if (String.IsNullOrWhiteSpace(sConfigedText))
                    obj = tDefaultvalue;
                else
                {
                    if (typeof(T) == typeof(System.Int32))
                    {
                        int nTmp = sConfigedText.ConvertToInt32();
                        if (nTmp <= 0)
                            obj = tDefaultvalue;
                        else
                            obj = nTmp;
                    }
                    else
                        if (typeof(T) == typeof(System.String))
                        {
                            if (String.IsNullOrWhiteSpace(sConfigedText))
                                obj = tDefaultvalue;
                            else
                                obj = sConfigedText;
                        }
                        else
                            if (typeof(T) == typeof(System.Boolean))
                            {
                                if (String.IsNullOrWhiteSpace(sConfigedText) || (sConfigedText.ToUpper() != "TRUE" && sConfigedText.ToUpper() != "FALSE"))
                                    obj = tDefaultvalue;
                                else
                                    obj = sConfigedText.ConvertToBoolean();
                            }
                }
                WriteLog(sNodeName + "=" + obj.ToString());
                CacheHelper.SetCacheItem(sCacheName, obj);
                return (T)obj;
            }
        }


        public bool IsVistorRobot(string sUserAgent)
        {
            if (String.IsNullOrEmpty(sUserAgent))
                return true;
            foreach (string s in lstSearchEngineRobot)
            {
                if (sUserAgent.ToLower().IndexOf(s.ToLower()) >= 0)
                    return true;
            }
            return false;
        }

        private object _objLogLock = new object();
        public void WriteLog(string s)
        {
            lock (_objLogLock)
            {
                try
                {
                    string sFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogForRewriter");
                    if (!Directory.Exists(sFolder))
                        Directory.CreateDirectory(sFolder);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(sFolder, System.DateTime.Now.ToString("yyyyMMdd") + "log.config"), true))
                    {
                        sw.WriteLine(System.DateTime.Now.ToString() + "--" + s);
                        sw.Close();
                    }
                }
                catch { }
            }

        }
        public void WriteWrongLog(System.Exception e)
        {
            WriteWrongLog(e.ToString());

        }
        private object _objWrongLock = new object();
        public void WriteWrongLog(string sMessage)
        {
            lock (_objWrongLock)
            {
                try
                {
                    string sFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogForRewriter");
                    if (!Directory.Exists(sFolder))
                        Directory.CreateDirectory(sFolder);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(sFolder, System.DateTime.Now.ToString("yyyyMMdd") + "wronglog.config"), true))
                    {
                        sw.WriteLine(System.DateTime.Now.ToString() + "--" + sMessage);
                        sw.Close();
                    }
                }
                catch { }
            }

        }
        public bool IsUrlSiteHomePage(string sUrl)
        {
            if (String.IsNullOrEmpty(sUrl))
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(sUrl, @"^(?:([a-zA-Z]+):)?(\/{0,3})([0-9.\-a-zA-Z]+)(?::(\d+))?/?$");
        }




        /// <summary>
        /// 手机访问首页跳转到指定的页面
        /// </summary>
        /// <param name="app"></param>
        public bool GotoMobilePage(HttpApplication app)
        {
            if (app == null || app.Context == null)
                return false;
            HttpCookie objCookie = app.Context.Request.Cookies[AppSettingHelper.sCookieName_Platform];
            if (objCookie != null)
            {
                return false;
            }
            string sAgent = app.Context.Request.UserAgent;
            if (string.IsNullOrEmpty(sAgent))
            {
                return false;
            }
            try
            {
                string sQuery = app.Context.Request.Url.Query;
                if (!string.IsNullOrEmpty(sQuery) && sQuery.IndexOf("utm_campaign", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    return false;
                }
            }
            catch { }

            bool bIsPhoneClient = IsMobileDevice(sAgent);

            if (!bIsPhoneClient)
            {
                return false;
            }
            app.Context.Response.Redirect("http://m.xiaomishu.com", true);
            return true;
        }

        public bool IsMobileDevice(string userAgent)
        {
            bool isMobile = false;

            if (string.IsNullOrEmpty(userAgent))
            {
                return isMobile;
            }

            if (userAgent.IndexOf("iPhone", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                isMobile = true;
            }
            else if (userAgent.IndexOf("Windows Phone", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                isMobile = true;
            }
            else if (userAgent.IndexOf("Android", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                isMobile = true;
            }

            return isMobile;
        }
    }
}
