using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace URLRewriter
{
    internal class AppSettingHelper
    {
        public static AppSettingHelper Instance = new AppSettingHelper();

        public static string sCookieName_Platform = "fg114Platform"; //手机访问平台

        /// <summary>
        /// 是否多重写机制，不同的城市用各自的Rewriter配置
        /// </summary>
        public bool MultiRewriter
        {
            get { return XMS.Core.Container.ConfigService.GetAppSetting<bool>("MultiRewriter", false); }
        }
    }
}
