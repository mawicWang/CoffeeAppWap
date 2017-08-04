using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace PublicResource
{
    /// <summary>
    /// Constant 的摘要说明。
    /// </summary>

    public class Constant
    {
        public Constant()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public static DateTime tNULLDateTime = new System.DateTime(1900, 1, 1, 0, 0, 0);
        public static string sBookingCookieName = "BookInfo";
        public static readonly string ChineseRegular = @"\u4E00-\u9FA5";
     

    }

    [DataContract]
    public class ReturnValue
    {
        public ReturnValue()
        {
            nRslt = 200;
        }
        [DataMember]
        public virtual int nRslt
        {
            get;
            set;
        }
        [DataMember]
        public virtual string sMessage
        {
            get;
            set;
        }
        [DataMember]
        public virtual object objInfo
        {
            get;
            set;
        }
    }
}
