using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace PublicResource
{
    /// <summary>
    /// CTRIP会员开放登录接口
    /// </summary>
    public abstract class CtripHelper
    {
        private static readonly string sKey = "5pHEP5EVD2BW1zprShpAuEJ0uQH2SYxfYmOTLf13TK0zvw3J9cpIOuvC6IQBBb6O0+lFA/SpBQgIgVzoc6lJIQ=="; //加密密钥
        private static readonly string sPass = "wy310027";
        /// <summary>
        /// 渠道标识
        /// </summary>
        public static string SID { get { return "95171"; } }

        /// <summary>
        /// 取得加密后的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetEncPassword(string sUserId, DateTime dtTime)
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("\"password\":\"{0}\",", sPass);
            if (!string.IsNullOrEmpty(sUserId))
            {
                sb.AppendFormat("\"uid\":\"{0}\",", sUserId);
            }
            dtTime = dtTime == DateTime.MinValue ? DateTime.Now : dtTime;
            sb.AppendFormat("\"date\":\"{0}\"", dtTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append("}");
            byte[] bt = Encoding.ASCII.GetBytes(sKey);

            return Encode(sb.ToString(), bt);
        }

        /// <summary>
        /// 通用HMACSHA1加密
        /// </summary>
        ///<param name="dicSource">待加密字符串字典</param>
        /// <returns></returns>
        public static string GetEncPassword(Dictionary<string, string> dicSource)
        {
            if (dicSource == null && dicSource.Count <= 1)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> keyItem in dicSource)
            {
                sb.AppendFormat("\"{0}\":\"{1}\",", keyItem.Key, keyItem.Value);
            }
            string sContent = "{" + sb.ToString().Trim(',') + "}";
            return GetEncPassword(sContent);
        }

        /// <summary>
        /// 通用HMACSHA1加密
        /// </summary>
        /// <param name="sStr">待加密字符串</param>
        /// <returns></returns>
        public static string GetEncPassword(string sStr)
        {
            if (string.IsNullOrEmpty(sStr))
                return string.Empty;
            return Encode(sStr, Encoding.ASCII.GetBytes(sKey));
        }

        public static string GetEncPassword()
        {
            return GetEncPassword("", DateTime.MinValue);
        }

        private static string Encode(string strData, byte[] keyByte)
        {
            HMACSHA1 provider = new HMACSHA1();
            if (keyByte != null)
            {
                provider.Key = keyByte;
            }
            byte[] hashedPassword = provider.ComputeHash(Encoding.ASCII.GetBytes(strData));
            return Convert.ToBase64String(hashedPassword);
        }

        /// <summary>
        /// 请求携程用户信息地址
        /// </summary>
        public static string RequestUserUrl
        {
            get
            {
                return string.Format("http://accounts.ctrip.com/myctrip/ctripsharelogin.aspx?SID={0}&EncPassword={1}&Time={2}&callback=funCtripLoginCall", SID, GetEncPassword(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        /// <summary>
        /// 携程用户登录地址
        /// </summary>
        public static string LoginUrl
        {
            get
            {
                return string.Format("https://accounts.ctrip.com/myctrip/ctrippostlogin.aspx?SID={0}&EncPassword={1}&Time={2}&callbackurl=", SID, GetEncPassword(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}
