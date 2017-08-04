using System;
using System.Web;
using System.IO;
using System.Text;
using XMS.Core;

namespace URLRewriter
{
    /// <summary>
    /// Provides utility helper methods for the rewriting HttpModule and HttpHandler.
    /// </summary>
    /// <remarks>This class is marked as internal, meaning only classes in the same assembly will be
    /// able to access its methods.</remarks>
    internal class RewriterUtils
    {
        #region RewriteUrl
        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl)
        {
            string x, y;
            RewriteUrl(context, sendToUrl, out x, out y);
        }

        private static string GetEncodedString(HttpContext context, string sValue)
        {
            try
            {
               
                if (context == null)
                    return sValue;
                

                //乱码
                if (regQueryString.IsMatch(sValue))
                {
                    string sAbsoluteURL = context.Request.Url.AbsoluteUri;
                    RewriterConfig.Instance.WriteWrongLog("原始：" + context.Request.Url.AbsoluteUri);

                    RewriterConfig.Instance.WriteWrongLog("值：" + sValue);
                    if (sAbsoluteURL.ToUpper().IndexOf("%EF%BF%BD") >= 0)
                    {
                        char[] cTmp = sValue.ToCharArray();
                        byte[] bTmp = new byte[cTmp.Length];
                        for (int z = 0; z < bTmp.Length; z++)
                        {
                            bTmp[z] = (byte)cTmp[z];
                        }
                        sValue = System.Text.Encoding.GetEncoding("GB2312").GetString(bTmp);
                        RewriterConfig.Instance.WriteWrongLog("key乱码后的值：" + sValue);
                    }
                    else
                    {
                        string sParas = sAbsoluteURL.Substring(sAbsoluteURL.IndexOf('?') + 1);
                        string[] sParaPairs = sParas.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        int nX = 0;
                        foreach (string s in sParaPairs)
                        {
                            nX = s.IndexOf("=");
                            if (nX < 1)
                            {
                                continue;
                            }
                            string sParamName = s.Substring(0, nX);
                            string sParamValue = s.Substring(nX + 1);
                            if ("key".Equals(sParamName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (!String.IsNullOrEmpty(sParamValue))
                                {
                                    sValue = sParamValue;
                                }
                                RewriterConfig.Instance.WriteWrongLog("key替换后的值：" + sValue);
                            }
                        }
                    }
                    
                }

                if (sValue.ContainsChinese()) //是否含有中文字符
                {
                    sValue = HttpUtility.UrlEncode(sValue);
                }
                else
                {
                    bool bFromOtherSite = false; //是否从站外进入
                    Uri referrer = context.Request.UrlReferrer;
                    if (referrer == null || referrer.Host.IndexOf(RewriterConfig.Instance.sCrntDomain, StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        bFromOtherSite = true;
                    }
                    if (bFromOtherSite)
                    {
                        try
                        {
                            Microsoft.JScript.GlobalObject.decodeURI(sValue);
                        }
                        catch
                        {
                            sValue = HttpUtility.UrlEncode(HttpUtility.UrlDecode(sValue, Encoding.GetEncoding("GB2312")), Encoding.UTF8);
                        }
                    }
                }
                return sValue;
            }
            catch (System.Exception e)
            {
                RewriterConfig.Instance.WriteWrongLog(e.ToString());
                return sValue;
            }
        }


        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        /// <param name="sendToUrlLessQString">Returns the value of sendToUrl stripped of the querystring.</param>
        /// <param name="filePath">Returns the physical file path to the requested page.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl, out string sendToUrlLessQString, out string filePath)
        {
            string queryString = String.Empty;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
                string[] sTmp = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                queryString = "";
                int nX = 0;
                foreach (string s in sTmp)
                {
                    nX = s.IndexOf("=");
                    if (nX < 1)
                    {
                        continue;
                    }
                    queryString += string.Format("&{0}={1}", s.Substring(0, nX),HttpUtility.UrlEncode(s.Substring(nX + 1)));
                }
                queryString = queryString.TrimEnd('&');
            }
            else
            {
                sendToUrlLessQString = sendToUrl;
            }

            bool bIsNeedLog = false;
           

            // see if we need to add any extra querystring information
            if (context.Request.QueryString.Count > 0)
            {
                string sRawURL = context.Request.RawUrl;
                string sParas = sRawURL.Substring(sRawURL.IndexOf('?') + 1);
                string[] sTmp = sParas.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                int nX = 0;
                foreach (string s in sTmp)
                {
                    nX = s.IndexOf("=");
                    if (nX < 1)
                    {
                        continue;
                    }
                    string sParamName = s.Substring(0, nX);
                    string sParamValue = s.Substring(nX + 1);
                    if ("key".Equals(sParamName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!String.IsNullOrEmpty(sParamValue))
                        {
                            string sOld = sParamValue;
                            sParamValue = GetEncodedString(context, sParamValue);
                            if(sOld!=sParamValue)
                                bIsNeedLog = true;
                        }
                    }
                    queryString += string.Format("&{0}={1}", sParamName, sParamValue);

                
                }
            }
            queryString = queryString.TrimStart('&');
            // first strip the querystring, if any

            // grab the file's physical path
            filePath = string.Empty;
            ///警告：下面的try很重要，不要删除

            try
            {
                if (allchar.IsMatch(sendToUrlLessQString))
                {
                    filePath = context.Server.MapPath(sendToUrlLessQString);
                }
                else
                {
                    RewriterConfig.Instance.WriteWrongLog(context.Request.RawUrl + "-sendto-" + sendToUrlLessQString);
                    return;
                }
                // rewrite the path...
                if (bIsNeedLog)
                {
                    Uri referrer = context.Request.UrlReferrer;
                    if (referrer != null)
                    {
                        RewriterConfig.Instance.WriteWrongLog("referer:"+referrer.AbsoluteUri);
                    }
                    RewriterConfig.Instance.WriteWrongLog("RawUrl:" + context.Request.RawUrl);
                    RewriterConfig.Instance.WriteWrongLog("参数：" + queryString);
                }
                context.RewritePath(sendToUrlLessQString, String.Empty, queryString);
            }
            catch
            {

            }

        }
        #endregion
        private static System.Text.RegularExpressions.Regex regQueryString = new System.Text.RegularExpressions.Regex(@"[^a-zA-Z#\d:/\?%\.=_&\+\s;\-\u4e00-\u9fa5]");

        private static System.Text.RegularExpressions.Regex allchar = new System.Text.RegularExpressions.Regex(@"^[a-z0-9\._\-/~]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        /// <summary>
        /// Converts a URL into one that is usable on the requesting client.
        /// </summary>
        /// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
        /// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
        /// <param name="appPath">The application path.</param>
        /// <param name="url">The URL, which might contain ~.</param>
        /// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
        /// value of the <b>appPath</b> parameter.</returns>
        internal static string ResolveUrl(string appPath, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }
}
