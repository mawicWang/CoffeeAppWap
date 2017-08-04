using System;
using System.Collections.Generic;
using XMS.Core;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using PublicResource;

namespace Business
{
    /// <summary>
    /// Summary description for WXHelper
    /// </summary>
    public class WXHelper
    {
        public static string ToUrl(SortedDictionary<string, object> m_values)
        {
            string buff = ""; 
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    Container.LogService.Error("调用ToUrl参数内部含有值为null的字段!");
                    throw new Exception("调用ToUrl参数内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        public static string GetRandomCode()
        {
            return Guid.NewGuid().ToString("N").Substring(24, 8).ToLower();
        }

        public static string DateFormat(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.ToString("yyyy-MM-dd"));
            string sWeek = GetWeek(dt);
            if (!string.IsNullOrEmpty(sWeek))
            {
                sb.Append("(");
                sb.Append(sWeek);
                sb.Append(")");
            }
            sb.Append(" ");
            sb.Append(dt.ToString("HH:mm"));
            return sb.ToString();

        }

        private static string GetWeek(DateTime dt)
        {
            try
            {
                string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string week = Day[Convert.ToInt32(dt.DayOfWeek.ToString("d"))].ToString();
                return week;
            }
            catch
            {
            }

            return "";

        }

        public static int GetLeftDays(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                return 0;
            }
            else
            {
                TimeSpan diff = to - from;
                return diff.Days;
            }
        }
        public static int GetLeftHours(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                return 0;
            }
            else
            {
                TimeSpan diff = to - from;
                return diff.Hours;
            }
        }
        public static int GetLeftMins(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                return 0;
            }
            else
            {
                TimeSpan diff = to - from;
                return diff.Minutes;
            }
        }
        public static int GetLeftSeconds(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                return 0;
            }
            else
            {
                TimeSpan diff = to - from;
                return diff.Seconds;
            }
        }

        /// <summary>
        /// 获得微信授权第一步获得code
        /// </summary>
        /// <param name="isbase">是否静默授权,</param>
        /// <param name="backurl">跳转地址</param>
        public static string GetAuthorizeCodeUrl(bool isbase, string backurl)
        {
            try
            {
                //第一步
                string redirect_uri = HttpUtility.UrlEncode(backurl);
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
                dic.Add("appid", AppSettingHelper.AppId);
                dic.Add("redirect_uri", redirect_uri);
                dic.Add("response_type", "code");
                dic.Add("scope", isbase ? "snsapi_base" : "snsapi_userinfo");
                dic.Add("state", "STATE" + "#wechat_redirect");//任意值
                string tempurlstr = WXHelper.ToUrl(dic);
                string url = AppSettingHelper.GetAuthorizeCodeFromUrl + tempurlstr;
                Container.LogService.Info("GetAuthorizeCodeUrl: " + url);
                return url;
            }
            catch (Exception ex)
            {
                Container.LogService.Error("获取微信授权code错误。\n" + ex.ToString());
                return "";
            }
        }

        public static GetOpenIdResponse GetOpenidInfoFromCode(string code)
        {
            try
            {
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
                dic["appid"] = AppSettingHelper.AppId;
                dic["secret"] = AppSettingHelper.SecretKey;
                dic["code"] = code;
                dic["grant_type"] = "authorization_code";
                string tempurl = WXHelper.ToUrl(dic);
                string url = AppSettingHelper.GetAccessTokeFromUrl + tempurl;
                WCFClient.LoggerService.Info("UrlInfo:" + url);
                string result = HttpService.Get(url);

                Container.LogService.Error("GetOpenidInfoFromCode: " + result);
                GetOpenIdResponse objResponse = PublicResource.JsonHelper.ConvertJsonStringToObject<GetOpenIdResponse>(result);
                return objResponse;
            }
            catch (Exception ex)
            {
                Container.LogService.Error(ex.ToString());
                return null;
            }
        }
    }
}
