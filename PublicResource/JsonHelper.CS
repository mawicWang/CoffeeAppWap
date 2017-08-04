using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Script.Serialization;

namespace PublicResource
{
    public class JsonHelper
    {
        public static T ConvertJsonStringToObject<T>(string strJson)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Deserialize<T>(strJson);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static T ConvertJsonTypeToObject<T>(object objJson)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.ConvertToType<T>(objJson);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static string ConvertObjectToJsonString(object obj)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                StringBuilder sb = new StringBuilder();
                js.Serialize(obj, sb);
                return sb.ToString();
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
    }
}
