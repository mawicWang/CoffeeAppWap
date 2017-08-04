using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace PublicResource
{
    public static class HttpHelper
    {
        static Random r = new Random();
        static object objLock = new object();
        public static string GetCoreRequest(string urlP, string encode)
        {
            if (null == urlP)
                return null;
            string StrRetP = null;
            Stream dataStream = null;
            try
            {

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(urlP);
                myHttpWebRequest.Timeout = 50000;
                HttpWebResponse Objresponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (Objresponse.StatusDescription == "OK")
                {
                    dataStream = Objresponse.GetResponseStream();
                    StreamReader r = new StreamReader(dataStream, Encoding.GetEncoding(encode));
                    StrRetP = r.ReadToEnd();
                }
            }
            catch (Exception exp)
            {
                PublicResource.Tools.WriteLog(urlP + "--" + exp.ToString());
            }
            finally
            {
                if (null != dataStream)
                    dataStream.Close();
            }
            return StrRetP;
        }



        public class FileVersionItem
        {
            public DateTime Last;
            public string AllString;
        }

        public static Dictionary<string, FileVersionItem> htFileDate = new Dictionary<string, FileVersionItem>();
        public static string GetVersion(string args)
        {
            return "20090819";
        }
        public static string GetGetRequest(string urlP, string encode)
        {
            return GetGetRequest(urlP, encode, true);
        }

        /// <summary>
        /// 获取某一url的文件内容
        /// </summary>
        /// <param name="urlP">url地址</param>
        /// <param name="encode">网页编码</param>
        /// <param name="bCached">是否缓存</param>
        /// <returns>文件内容</returns>
        public static string GetGetRequest(string urlP, string encode, bool bCached)
        {
           
            if (String.IsNullOrEmpty(urlP))
                return null;

            //如果是通用的头底部，加上携程的cookie参数请求
            bool bContainParams = urlP.Contains("?");
            if (Regex.IsMatch(urlP, @"(/fgheader(v\d)*\.aspx|fgfooter(v\d)*\.aspx)", RegexOptions.IgnoreCase))
            {
                bool bIsFromCtrip = HttpContext.Current.Request.Cookies["IsFromCtrip"] != null;
                if (HttpContext.Current.Request.UrlReferrer == null || String.IsNullOrEmpty(HttpContext.Current.Request.UrlReferrer.AbsolutePath))
                {
                    //如果是网站首页并且来源为空
                    string sUrl = HttpContext.Current.Request.RawUrl;
                    if (string.IsNullOrEmpty(sUrl) || sUrl.Equals("/"))
                    {
                        bIsFromCtrip = false;
                    }
                }
                if (bIsFromCtrip)
                {
                    urlP += (bContainParams ? "&" : "?") + "ctrip=1";
                }
            }

            string fileName = String.Format("{0}`{1}", urlP, encode ?? "gb2312");
            fileName = fileName.ToLower();

            //如果页面需要缓存的话            
            if (bCached && htFileDate.ContainsKey(fileName))
            {
                if (htFileDate[fileName].Last > DateTime.Now)
                {
                    return htFileDate[fileName].AllString;
                }
            }

            bool nerverNull = urlP.ToLower().IndexOf("head") > 0;
            string newargs = null;

            urlP += (bContainParams ? "&" : "?") + "timespan=" + r.Next(1, 100000000);
            newargs = GetCoreRequest(urlP, encode);
            if (nerverNull && String.IsNullOrEmpty(newargs))
            {
                int looptime = 10;
                int loopi = 0;
                while (loopi++ < looptime)
                {
                    newargs = GetCoreRequest(urlP, encode);
                    if (!String.IsNullOrEmpty(newargs))
                        break;
                    System.Threading.Thread.Sleep(1 * 500);
                }
                if (String.IsNullOrEmpty(newargs))
                {
                    return null;
                }
            }

            if (bCached) //将文件内容加入缓存
            {
                //防止多线程出错
                lock (objLock)
                {
                    if (!htFileDate.ContainsKey(fileName))
                    {
                        htFileDate.Add(fileName, new FileVersionItem());
                    }
                }
                htFileDate[fileName].AllString = newargs;
                htFileDate[fileName].Last = DateTime.Now.AddMinutes(5);
            }
            return newargs;
            //if ("Db".Equals(ConfigurationManager.AppSettings["IncludeFileFrom"]))
            //{
            //    return IncludeFileDb(fileName);
            //}
            //else
            //{
            //    return SynAPI.Client.FG.系统变量.IncludeFile(fileName);
            //}
        }

    }
}
