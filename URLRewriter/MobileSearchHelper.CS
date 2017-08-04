using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using XMS.Core;
using XMS.Core.Web;

namespace URLRewriter
{
    /// <summary>
    /// 移动搜索帮助类
    /// </summary>
    public class MobileSearchHelper
    {
        private const string mobileSearchFilePath = "conf/MobileSearchUrlMap.config";
        static object objSync = new object();

        public static readonly MobileSearchHelper Instance = new MobileSearchHelper();

        internal bool Location(System.Web.HttpApplication app, string sRequestedPath)
        {
            bool isValidReferrer = false;
            bool isValidDevice = false;

            Uri referrer = app.Request.UrlReferrer;
            if (referrer != null)
            {
                string host = referrer.Host.ToLower();
                if (RewriterConfig.Instance.MobileSearchHosts.Contains(host))
                {
                    isValidReferrer = true;
                }
            }

            HttpCookie cookie = app.Context.Request.Cookies[AppSettingHelper.sCookieName_Platform];
            if (cookie == null)
            {
                //检查是否是移动设备
                isValidDevice = RewriterConfig.Instance.IsMobileDevice(app.Context.Request.UserAgent);
            }


            if (!isValidReferrer ||!isValidDevice)
            {
                return false;
            }



            string mobileUrl = GetMobileUrl(app, sRequestedPath, true);
            if (!string.IsNullOrEmpty(mobileUrl))
            {
                return RedirectWithCheck(mobileUrl, 302);
            }
            return false;
        }

        public string GetMobileUrl(System.Web.HttpApplication app, string requestPath, bool appendQueryString)
        {
            string mobileUrl = string.Empty;

            List<URLRewriter.Config.RewriterRule> rules = MobileSearchRules;
            if (rules == null)
            {
                return mobileUrl;
            }

            foreach (URLRewriter.Config.RewriterRule objRule in rules)
            {
                string sLookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, objRule.LookFor) + "$";
                bool bLookForWithQueryString = sLookFor.IndexOf("\\?", StringComparison.CurrentCultureIgnoreCase) > 0;
                if (bLookForWithQueryString)
                {
                    requestPath = app.Request.RawUrl;
                }

                Regex re = new Regex(sLookFor, RegexOptions.IgnoreCase);
                if (re.IsMatch(requestPath))
                {
                    mobileUrl = re.Replace(requestPath, objRule.SendTo);
                    if (!mobileUrl.StartsWith("http"))
                    {
                        mobileUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, mobileUrl);
                    }

                    //参数要带过去
                    if (!bLookForWithQueryString && appendQueryString && app.Context.Request.QueryString.Count > 0)
                    {
                        if (mobileUrl.IndexOf('?') != -1)
                            mobileUrl += "&" + app.Context.Request.QueryString.ToString();
                        else
                            mobileUrl += "?" + app.Context.Request.QueryString.ToString();
                    }
                }
            }
            return mobileUrl;
        }

        public string GetMobileMeta()
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                return string.Empty;
            }
            string path = context.Request.RawUrl;
            string query = string.Empty;

            int x = context.Request.RawUrl.IndexOf("?");
            if (x > 0)
            {
                path = context.Request.RawUrl.Substring(0, x);
                query = context.Request.RawUrl.Substring(x + 1);
            }


            string cacheKey = string.Format("MobileMeta_{0}", path);
            string url = CacheHelper.GetCacheItem(cacheKey) as string;
            if (url == null)
            {
                url = GetMobileUrl(context.ApplicationInstance, path, false);
                if (url == null)
                {
                    url = string.Empty;
                }
                CacheHelper.SetCacheItem(cacheKey, url);
            }
            string content = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                if (!string.IsNullOrEmpty(query))
                {
                    if (url.IndexOf('?') != -1)
                        url += "&" + query;
                    else
                        url += "?" + query;
                }

                content += string.Format("<meta http-equiv=\"mobile-agent\" content=\"format=xhtml; url={0}\" />\n\t", url);
                content += string.Format("<meta http-equiv=\"mobile-agent\" content=\"format=html5; url={0}\" />\n\t", url);
                content += string.Format("<meta http-equiv=\"mobile-agent\" content=\"format=wml; url={0}\" />", url);

            }

            return content;
        }

        private List<URLRewriter.Config.RewriterRule> MobileSearchRules
        {
            get
            {
                return GetRewriteRuleFromCache("root/MobileSearchConfig/Rules");
            }
        }


        private XmlNode GetXMLNodeFromConfigFile(string sNodeName, string sFilePath)
        {
            lock (objSync)
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

        private List<URLRewriter.Config.RewriterRule> GetRewriteRuleFromCache(string sNodeName)
        {
            string cacheKey = "MobileSearchUrlMap";
            List<URLRewriter.Config.RewriterRule> rules = CacheHelper.GetCacheItem(cacheKey) as List<URLRewriter.Config.RewriterRule>;

            if (rules != null)
            {
                return rules;
            }

            lock (objSync)
            {
                string filePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, mobileSearchFilePath);

                XmlNode objFatherNode = GetXMLNodeFromConfigFile(sNodeName, filePath);
                rules = new List<URLRewriter.Config.RewriterRule>();
                foreach (XmlNode xmlNode in objFatherNode.ChildNodes)
                {
                    if (!xmlNode.HasChildNodes || xmlNode.ChildNodes.Count < 2)
                    {
                        continue;
                    }

                    URLRewriter.Config.RewriterRule objRule = new URLRewriter.Config.RewriterRule();

                    objRule.LookFor = xmlNode.ChildNodes[0].InnerText;
                    objRule.SendTo = xmlNode.ChildNodes[1].InnerText;

                    rules.Add(objRule);
                }
                return rules;
            }
        }


        private bool RedirectWithCheck(string sNewUrl, int nCode, bool bIsNeedCache = false)
        {
            if (String.IsNullOrWhiteSpace(sNewUrl))
                return false;
            if (("http://" + HttpContext.Current.Request.Url.Host).CombineUrl(HttpContext.Current.Request.RawUrl).ToLower() == sNewUrl.ToLower())
            {
                return false;
            }
            string sTmp = "302 Found";
            if (nCode == 301)
            {
                sTmp = "301 Moved Permanently";
            }
            HttpContext.Current.Response.Status = sTmp;
            if (!bIsNeedCache)
            {
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetNoStore();
            }

            HttpContext.Current.Response.AddHeader("Location", sNewUrl);
            HttpContext.Current.Response.End();
            return true;
        }
    }
}