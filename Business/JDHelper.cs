using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class JDHelper
    {

    }

    /// <summary>
    /// 微信返回对象（获取OpenId）
    /// </summary>
    [Serializable]
    public class WeixinJson
    {
        public string access_token = string.Empty;
        public string expires_in = string.Empty;
        public string refresh_token = string.Empty;
        public string openid = string.Empty;
        public string scope = string.Empty;
    }


    /// <summary>
    /// 初始化信息
    /// </summary>
    [Serializable]
    public class ResInfo
    {
        public string resId { get; set; }
        public string currTime { get; set; }
        public bool isOneDiscount { get; set; }
    }

    /// <summary>
    /// 折扣列表
    /// </summary>
    [Serializable]
    public class DiscountListInfo
    {
        /// <summary>
        /// 开始时间-从1970年到现在的时间毫秒数
        /// </summary>
        public long startTime { get;set;}
        /// <summary>
        /// 当前时间-从1970年到现在的时间毫秒数
        /// </summary>
        public long currTime { get; set; }
        /// <summary>
        /// 服务员编号
        /// </summary>
        public string employeeNo { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public string memberWeiXinId { get; set; }
        /// <summary>
        /// 订单Id(0：表示没有进行中的订单；大于0表示有进行中的订单)
        /// </summary>
        public int orderId { get; set; }
        /// <summary>
        /// 折扣率一览表
        /// </summary>
        public List<DiscountList> discountList = new List<DiscountList>();
    }

    [Serializable]
    public class DiscountList
    {
        /// <summary>
        /// 时间长度
        /// </summary>
        public long timePeriod { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public double discountRate { get; set; }
    }

    //[Serializable]
    //public class MessageInfo
    //{
    //    public int code { get; set; }
    //    public string msg { get; set; }
    //    public object obj { get; set; }
    //}

    [Serializable]
    public class UrlInfo
    {
        public string url { get; set; }
    }

    [Serializable]
    public class HtmlInfo
    {
        public string html = string.Empty;
    }
}
