using System;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;
using URLRewriter.Config;
using System.Data;
using System.Collections.Generic;
using XMS.Core;

namespace URLRewriter
{
    /// <summary>
    /// Provides a rewriting HttpModule.
    /// </summary>
    public class ModuleRewriter : BaseModuleRewriter
    {
        /// <summary>
        /// This method is called during the module's BeginRequest event.
        /// </summary>
        /// <param name="requestedRawUrl">The RawUrl being requested (includes path and querystring).</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected override void Rewrite(string requestedPath, System.Web.HttpApplication app)
        {


            string sIp = XMS.Core.Web.RequestHelper.GetIP(app.Context.Request);


            string sVisitHost = app.Context.Request.Url.Host.ToLower();

            // "shanghai" 开头的站点，一定是主站切换城市，写死，跳转www.xiaomishu.com,写safegohost cookie
            if (sVisitHost.StartsWith("shanghai"))
            {
                HttpCookie cookieTmp = app.Context.Request.Cookies[sCityCookieName];
                cookieTmp = GetCookie(RewriterConfig.Instance.sDefaultGoHost);
                app.Context.Response.Cookies.Add(cookieTmp);
                RedirectWithCheck(("http://" + RewriterConfig.Instance.sDefaultGoHost).CombineUrl(app.Context.Request.Url.Query), 301);
                return;
            }
            //// hostname不是二级域名或主域名，一定错了，跳转www.xiaomishu.com/citylist
            //if (sVisitHost.Split('.').Length != 3)
            //{
            //    RedirectWithCheck("http://" + RewriterConfig.Instance.sDefaultGoHost + "/citylist/  ", 301);
            //    return;
            //}
            ////主站，进行host判断，不存在的host，跳citylist页面
            //if (RewriterConfig.Instance.bIsXiaomishuSite)
            //{
            //    string sIsValidSite = RewriterConfig.Instance.GetSiteDefualtPage(sVisitHost);
            //    if (String.IsNullOrEmpty(sIsValidSite))
            //    {
            //        RedirectWithCheck("http://" + RewriterConfig.Instance.sDefaultGoHost + "/citylist/", 301);
            //        return;
            //    }
            //}




            /*移动客户端跳转App内置页，2016 04 22 暂时取消掉
            //解决m.xiaomishu.com 苹果端自动跳转到跳转页，跳转页跳到相应app内置页 by 天九
            if (sVisitHost.StartsWith("m.57hao.com") || sVisitHost.StartsWith("m.xiaomishu.com"))
            {
                if (app.Context.Request.Url.PathAndQuery.IndexOf("InnerTemp", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return;
                }
                //XMS.Core.Container.LogService.Error("current path:" + "http://" + sVisitHost + app.Context.Request.Url.PathAndQuery);
                if (app.Context.Request.Url.PathAndQuery.IndexOf("IsUrlRewriter=1", StringComparison.OrdinalIgnoreCase) <= 0 && app.Context.Request.HttpMethod != "POST")
                {
                    if (app.Context.Request.Url.PathAndQuery.ToLower().Contains("shop/detail") || app.Context.Request.Url.PathAndQuery.ToLower().Contains("appwap/weixin"))
                    {
                        if (HttpContext.Current != null)
                        {
                            XMS.Portal.Core.BrowserInfo browserInfo = new XMS.Portal.Core.BrowserInfo(HttpContext.Current);
                            if (browserInfo.OperationSystem == XMS.Portal.Core.BrowserInfo.OS.iPhone)
                            {
                                string fullUrl = "http://" + sVisitHost + app.Context.Request.Url.PathAndQuery;
                                //XMS.Core.Container.LogService.Error("fullUrl:" + fullUrl);
                                string toUrl = ("http://" + sVisitHost).CombineUrl("InnerTemp.aspx?path=" + HttpUtility.UrlEncode(fullUrl));
                                //XMS.Core.Container.LogService.Error("toUrl:" + toUrl);
                                RedirectWithCheck(toUrl, 301);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    XMS.Core.Container.LogService.Error("out");
                }

            }
            */

            string sTmpRequestPath = requestedPath.ToLower().Replace("/default.aspx", "").Replace("/index.aspx", "").Replace("/", "");

            //来源为移动搜索时特殊逻辑
            if (RewriterConfig.Instance.IsMobileSearchRedirect)
            {
                if (MobileSearchHelper.Instance.Location(app, requestedPath))
                {
                    return;
                }
            }


            #region  如果是网站首页，开始干活，虚拟目录不干活
            if (String.IsNullOrWhiteSpace(sTmpRequestPath) && RewriterConfig.Instance.bIsXiaomishuSite)
            {
                DeleteCtripCookie(app, sVisitHost);
                HttpCookie hco = app.Context.Request.Cookies[sCityCookieName];

                #region Ip 跳转以及 上海跳转
                //如果是spider 不进入下面的跳转
                if (!RewriterConfig.Instance.IsVistorRobot(app.Context.Request.UserAgent))
                {
                    //手机设备第一次访问跳转
                    if (RewriterConfig.Instance.GotoMobilePage(app))
                    {
                        return;
                    }
                    if (sVisitHost.StartsWith("www."))//输入的是主站
                    {
                        //如果有cookie，也就是说曾经访问过主站，按照cookie值跳转,错误的cookie也跳，前面会挡住的
                        if (hco != null)
                        {
                            string sGoHost = hco.Value.ToLower();
                            if (!sGoHost.Equals(sVisitHost))
                            {
                                app.Context.Response.Redirect("http://" + sGoHost);
                                return;
                            }

                        }
                        else //无Cookie
                        {
                            //if(app.Context.Request["TestIP"]!=null)
                            //{
                            //    sIp = app.Context.Request["TestIP"];
                            //}
                            ////如果是上海不跳转citylist，直接进入www.xiaomishu.com
                            //ReturnValue<XMS.Portal.Common.Model.CityHost> objRslt = APIClient.CommenService.GetCityByIP(sIp);
                            //if (objRslt.Code == 200 && objRslt.Value != null )
                            //{
                            //    if(objRslt.Value.CityId==200000)
                            //    {
                            //         RewriteDefaultPage(sVisitHost, app);
                            //    }
                            //    else
                            //    {
                            //        RedirectWithCheck("http://" + objRslt.Value.DefaultHost, 302);
                            //    }
                              
                            //    return;
                            //}
                            //string goHost = null;
                            //goHost = string.Format("http://{0}/citylist/", sVisitHost);
                            //app.Context.Response.Redirect(goHost, true);
                            return;
                        }
                    }

                }
                #endregion
                RewriteDefaultPage(sVisitHost, app);
                return;

            }

            #endregion 网站首页

            if (Process301(app, requestedPath, true))
                return;

            #region 种cookie
            if (RewriterConfig.Instance.bIsWriteSafeHost && RewriterConfig.Instance.bIsXiaomishuSite)
            {
                HttpCookie cookieTmp = app.Context.Request.Cookies[sCityCookieName];
                if (cookieTmp == null)
                {
                    cookieTmp = GetCookie(sVisitHost);
                    app.Context.Response.Cookies.Add(cookieTmp);
                }

            }
            #endregion

            List<Config.RewriterRule> rules = RewriterConfig.Instance.lstRules;
            // iterate through each rule...
            for (int i = 0; i < rules.Count; i++)
            {
                // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                string lookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, rules[i].LookFor) + "$";

                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                // See if a match is found
                if (re.IsMatch(requestedPath))
                {
                    if (!requestedPath.EndsWith("/") && requestedPath.IndexOf(".aspx") < 0)
                    {
                        string sGotoUrl = requestedPath + "/";
                        if ("GET".Equals(app.Context.Request.HttpMethod) && re.IsMatch(sGotoUrl))
                        {
                            try
                            {
                                sGotoUrl += app.Context.Request.Url.Query;
                            }
                            catch { }
                            RedirectWithCheck(sGotoUrl, 301, true);
                            return;
                        }
                    }

                    // match found - do any replacement needed
                    string sendToUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(requestedPath, rules[i].SendTo));
                    // Rewrite the URL
                    RewriterUtils.RewriteUrl(app.Context, sendToUrl);
                    break;		// exit the for loop
                }
            }

        }

        private bool RewriteDefaultPage(string sMyHost, System.Web.HttpApplication app)
        {

            ////若果网站首页，找到相应的真实页， rewrite,写cookie，
            //string sDefaultPage = RewriterConfig.Instance.GetSiteDefualtPage(sMyHost);
            //if (!String.IsNullOrEmpty(sDefaultPage))
            //{
            //    HttpCookie hco = GetCookie(sMyHost);
            //    app.Context.Response.Cookies.Add(hco);
            //    RewriterUtils.RewriteUrl(app.Context, "~/" + sDefaultPage.Trim('/', '~'));
            //    return true;
            //}
            //RewriterConfig.Instance.WriteWrongLog("default page is null: myhost=" + sMyHost);


            //string sNewUrl = "http://" + RewriterConfig.Instance.sDefaultGoHost;
            //RedirectWithCheck(sNewUrl, 301);
            return false;
        }
        private const string sCityCookieName = "safegohost";
        private HttpCookie GetCookie(string sHost)
        {

            HttpCookie hco = new HttpCookie(sCityCookieName);
            hco.Value = sHost;
            hco.Expires = System.DateTime.Now.AddDays(RewriterConfig.Instance.nCookieDayLength);
            hco.Domain = "." + RewriterConfig.Instance.sCrntDomain;
            return hco;
        }

        private void DeleteCtripCookie(System.Web.HttpApplication app, string myHost)
        {

            if (app.Context.Request.UrlReferrer == null || String.IsNullOrEmpty(app.Context.Request.UrlReferrer.AbsolutePath))
            {
                HttpCookie cookieCtrip = app.Context.Request.Cookies["IsFromCtrip"];
                if (cookieCtrip != null)
                {
                    cookieCtrip.Expires = System.DateTime.Now.AddDays(-100);
                    cookieCtrip.Domain = "." + RewriterConfig.Instance.sCrntDomain;
                    app.Context.Response.Cookies.Add(cookieCtrip);

                }
            }
        }







        public bool Process301(System.Web.HttpApplication app, string sRequestedPath, bool bIsMigration)
        {


            List<URLRewriter.Config.RewriterRule> rules = RewriterConfig.Instance.lstMigrationRules;
            if (!bIsMigration)
                rules = RewriterConfig.Instance.lstOldWebRules;
            if (rules != null)
            {
                foreach (URLRewriter.Config.RewriterRule objRule in rules)
                {
                    string sLookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, objRule.LookFor) + "$";
                    bool bLookForWithQueryString = sLookFor.IndexOf("\\?", StringComparison.CurrentCultureIgnoreCase) > 0;
                    if (bLookForWithQueryString)
                    {
                        sRequestedPath = app.Request.RawUrl;
                    }

                    Regex re = new Regex(sLookFor, RegexOptions.IgnoreCase);
                    if (re.IsMatch(sRequestedPath))
                    {
                        string sMoveTo = re.Replace(sRequestedPath, objRule.SendTo);
                        if (!sMoveTo.StartsWith("http"))
                        {
                            sMoveTo = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, sMoveTo);
                        }

                        //参数要带过去
                        if (!bLookForWithQueryString && app.Context.Request.QueryString.Count > 0)
                        {
                            if (sMoveTo.IndexOf('?') != -1)
                                sMoveTo += "&" + app.Context.Request.QueryString.ToString();
                            else
                                sMoveTo += "?" + app.Context.Request.QueryString.ToString();
                        }

                        if (sMoveTo.ToLower().IndexOf("/k") > 0 && sMoveTo.ToLower().IndexOf("/search/") > 0)
                        {
                            string s = sMoveTo.Substring(sMoveTo.ToLower().IndexOf("/k") + 2);
                            if (s.IndexOf("/") > 0)
                                s = s.Substring(0, s.IndexOf("/"));
                            string z = app.Server.UrlEncode(s);
                            sMoveTo = sMoveTo.Replace("/k" + s, "/k" + z);
                        }
                        return RedirectWithCheck(sMoveTo, 301);
                    }
                }
            }
            return false;



        }

        /// <summary>
        /// 合并Url
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// 

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
