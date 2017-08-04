using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XMS.Core;

namespace Business
{
    /// <summary>
    /// 配置信息帮助类
    /// </summary>
    public class AppSettingHelper
    {
        //public static string DefaultImagePath
        //{
        //    get
        //    {
        //        return Container.ConfigService.GetAppSetting<string>("DefaultImagePath", "http://upload2.95171.cn/o/img/small/takeaway_foodnopic.png");
        //    }
        //}

        public static string AppId
        {
            get { return Container.ConfigService.GetAppSetting<string>("AppId", "wxa691be29e7554dcc"); }
        }

        //public static string SecretKey
        //{
        //    get { return Container.ConfigService.GetAppSetting<string>("secretkey", "246aefa9374e2f4e0725f00758f6e66b"); }

        //}
        public static string SecretKey
        {
            get { return Container.ConfigService.GetAppSetting<string>("secretkey", "19f042bcce790a9fd71e9174651e5a06"); }
        }

        public static string GetAuthorizeCodeFromUrl
        {
            get { return Container.ConfigService.GetAppSetting<string>("GetAuthorizeCodeFromUrl", "https://open.weixin.qq.com/connect/oauth2/authorize?"); }
        }

        public static string GetAccessTokeFromUrl
        {
            get { return Container.ConfigService.GetAppSetting<string>("GetAccessTokeFromUrl", "https://api.weixin.qq.com/sns/oauth2/access_token?"); }
        }

        public static string GetUserInfoFromUrl
        {
            get { return Container.ConfigService.GetAppSetting<string>("GetUserInfoFromUrl", "https://api.weixin.qq.com/sns/userinfo?"); }
        }

        public static string DomainName
        {
            get { return Container.ConfigService.GetAppSetting<string>("DomainName", "waimai.buzztimecoffee.com"); }
        }


        /// <summary>
        /// ProductId
        /// </summary>
        public static string ProductId
        {
            get
            {
                return Container.ConfigService.GetAppSetting<string>("ProductId", "wx");
            }
        }

        /// <summary>
        /// SaleChannelApplicationId
        /// </summary>
        public static string SaleChannelApplicationId
        {
            get
            {
                return Container.ConfigService.GetAppSetting<string>("SaleChannelApplicationId", "wx");
            }
        }
    }
}