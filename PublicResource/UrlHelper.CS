using System;
using System.Collections.Generic;
using System.Text;

namespace PublicResource
{
    /// <summary>
    /// 常用地址辅助类
    /// </summary>
    public abstract class UrlHelper
    {
        /// <summary>
        /// 餐厅地址
        /// </summary>
        /// <param name="sResId">餐厅Id</param>
        /// <returns></returns>
        public static string GetResUrl(string sResId)
        {
            return string.Format(AppHelper.UrlRoot + "shop/{0}/", sResId);
        }

        /// <summary>
        /// 餐厅订座地址
        /// </summary>
        /// <param name="sResId">餐厅Id</param>
        /// <returns></returns>
        public static string GetBookingUrl(string sResId)
        {
            return string.Format(AppHelper.UrlRoot + "shop/{0}/book/", sResId);
        }

        /// <summary>
        /// 用户首页地址
        /// </summary>
        /// <param name="nUserId">用户Id</param>
        /// <returns></returns>
        public static string GetUserUrl(int nUserId)
        {
            if (nUserId < 0)
            {
                return "#";
            }
            return string.Format("http://www.xiaomishu.com/Member/{0}/", nUserId);
        }

        public static string GetUserPhotoUrl(int nUserId, string sSize)
        {
            return string.Format(AppHelper.PassportUrl + "face.aspx?uid={0}&size={1}", nUserId, sSize);
        }
    }
}