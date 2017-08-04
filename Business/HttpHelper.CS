using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JD.Portal.JD.Business
{
    public class HttpHelper
    {
        private static string sendMessUrl = "http://www.qybor.com:8500/shortMessage?1=1";
        private static string sendParam = "&username=shchangqi&passwd=13641866625&phone={0}&msg={1}&needstatus=true&port={2}&sendtime={3}";
        public static bool SendSms(string phone, string message)
        {
            string postData = "ReplyMessage=" + HttpUtility.UrlEncode(message);
            try
            {
                sendMessUrl = sendMessUrl + string.Format(sendParam, phone,message,"123",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string ddd = "uuu";
                if (ddd.Equals("uuu"))
                {
                    string result = HttpHelper.RequestURLGet(sendMessUrl);

                    string ddddfdsdf = "sdfsdfsdfsdf";

                    if (ddddfdsdf.Equals(""))
                        ddddfdsdf = "sdfsdf";
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static string RequestURL(string url, string postData)
        {
            if (string.IsNullOrEmpty(postData))
                return RequestURLGet(url);
            else
                return RequestURLPost(url, postData);
        }

        /// <summary>
        /// Get方式访问HTTP
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string RequestURLGet(string url)
        {
            string responseData = "";
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 30000;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd().ToString();
                        reader.Close();
                    }
                }
            }
            catch { throw; }
            finally
            {
                try
                {
                    response.Close();
                    GC.Collect();
                }
                catch { }
            }

            return responseData;
        }

        /// <summary>
        /// POST方式访问HTTP
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private static string RequestURLPost(string url, string postData)
        {
            string responseData = "";
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;

                byte[] bs = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Encoding encoding = Encoding.UTF8;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        responseData = reader.ReadToEnd().ToString();
                        reader.Close();
                    }
                }
            }
            catch { throw; }
            finally
            {
                try
                {
                    response.Close();
                    GC.Collect();
                }
                catch { }
            }

            return responseData;
        }
    }
}
