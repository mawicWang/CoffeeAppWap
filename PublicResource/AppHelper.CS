using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;

namespace PublicResource
{
    public static class AppHelper
    {
        private readonly static Random r = new Random();
        /// <summary>
        /// 文件服务器散列化地址
        /// </summary>
        public static string StaticUploadUrl
        {
            get
            {
                string sUrl = ConfigurationManager.AppSettings["StaticUploadUrl"];
                if (string.IsNullOrEmpty(sUrl))
                {
                    return string.Empty;
                }
                sUrl = string.Format(sUrl, r.Next(1, 5));
                return sUrl.TrimEnd('/') + "/";
            }
        }

        /// <summary>
        /// 餐厅图片服务器，以/结尾
        /// </summary>
        public static string ResPicSiteUrl
        {
            get
            {
                string sUrl = ConfigurationManager.AppSettings["ResPicSiteUrl"];
                if (string.IsNullOrEmpty(sUrl))
                {
                    return string.Empty;
                }
                sUrl = string.Format(sUrl, r.Next(1, 5));
                return sUrl.TrimEnd('/') + "/";
            }
        }

        ///<summary>
        /// 放置静态上传的资源的URL路径
        /// </summary>
        public static string StaticUploadFile
        {
            get
            {
                string s = ConfigurationManager.AppSettings["StaticUploadFile"];
                if (String.IsNullOrEmpty(s)) s = UrlRealRoot.ToLower();
                return s.TrimEnd('/') + "/";
            }
        }


        /// <summary>
        /// 获取应用程序的URL完整基本路径，以 / 结尾
        /// </summary>
        public static string UrlRealRoot
        {
            get
            {
                string ts = System.Web.HttpContext.Current.Request.Url.Scheme + @"://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/";
                if (ts.EndsWith("//"))
                    ts = ts.Substring(0, ts.Length - 1);
                return ts.ToLower();
            }

        }

        /// <summary>
        /// 获取应用程序的URL基本路径，以 / 结尾
        /// </summary>
        public static string UrlRoot
        {
            get
            {
                string ts = HttpContext.Current.Request.Url.Scheme + @"://" + HttpContext.Current.Request.Url.Host + "/";
                if (ts.EndsWith("//"))
                    ts = ts.Substring(0, ts.Length - 1);
                return ts.ToLower();
            }

        }

        /// <summary>
        /// 网站通用头部、底部站点Url
        /// </summary>
        public static string CommonUrl
        {
            get
            {
                string sCommonPath = ConfigurationManager.AppSettings["StaticConfig_CommonUrl"].TrimStart('/');
                if (!sCommonPath.StartsWith("http://"))
                {
                    sCommonPath = AppHelper.UrlRoot.TrimEnd('/') + '/' + sCommonPath;
                }
                return sCommonPath;
            }
        }

        /// <summary>
        /// 登录站点
        /// </summary>
        public static string PassportUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["StaticConfig_Passport"];
            }
        }

        /// <summary>
        /// 论坛
        /// </summary>
        public static string BBSUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["StaticConfig_BBSUrl"];
            }
        }

        /// <summary>
        /// 婚礼小秘书站点地址
        /// </summary>
        public static string WeddingUrl
        {
            get { return ConfigurationManager.AppSettings["StaticConfig_WeddingUrl"]; }

        }


        /// <summary>
        /// 用户
        /// </summary>
        public static string UCHomeUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["StaticConfig_UCHomeUrl"];
            }
        }

        /// <summary>
        /// 警告：静态文件的服务器，只用于放置餐馆图片，无他用！，以 / 结尾 ！！！
        /// </summary>
        public static string StaticUrl
        {
            get
            {
                string s = ConfigurationManager.AppSettings["StaticUrl"];
                if (String.IsNullOrEmpty(s)) s = UrlRealRoot.ToLower();
                return s.TrimEnd('/') + "/";
            }
        }

        /// <summary>
        /// 主站地址
        /// </summary>
        public static string MainSiteUrl
        {
            get
            {
                string s = ConfigurationManager.AppSettings["MainSiteUrl"];
                if (String.IsNullOrEmpty(s)) s = UrlRealRoot.ToLower();
                return s.TrimEnd('/') + "/";
            }
        }

        /// <summary>
        /// 传入一个标识ID，返回对应的服务器地址，以 / 结尾 ！！！
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string StaticUrlById(object id)
        {
            if (null == id) return string.Format(StaticUrl, "");
            int myid = 0;
            if ((id.GetType().Equals(typeof(string))))
            {
                string args = Regex.Replace(id.ToString(), "[^0-9]", "");
                myid = Tools.CInt(args);

            }
            if ((id.GetType().Equals(typeof(int))))
            {
                myid = (int)id;
            }
            int uid = myid % 4;
            string appends = uid.Equals(0) ? "" : (uid.ToString());
            return string.Format(StaticUrl, appends);
        }

        /// <summary>
        /// 警告：WWW.57575777.COM的 静态文件的服务器，不要修改这个变量 。以 / 结尾 ！！！
        /// </summary>
        public static string FG114StaticUrl
        {
            get
            {
                string s = ConfigurationManager.AppSettings["FG114StaticUrl"];
                if (String.IsNullOrEmpty(s)) s = UrlRealRoot.ToLower();
                return s.TrimEnd('/') + "/";
            }
        }

        /// <summary>
        /// 携程美食订餐频道地址
        /// </summary>
        public static string CtripDiningUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["StaticConfig_CtripUrl"];
            }
        }
    }
}
