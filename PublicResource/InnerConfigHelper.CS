using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicResource
{
    /// <summary>
    /// 避免内网项目中出现编译错误
    /// </summary>
    public class InnerConfigHelper
    {
        public static string StaticSourceUrl
        {
            get
            {
                string sHost = System.Configuration.ConfigurationManager.AppSettings["StaticSourceHost"];
                if (String.IsNullOrEmpty(sHost))
                {
                    sHost = "";
                }
                return sHost;
            }
        }
    }
}
