using System;
using System.Collections.Generic;
using System.Linq;

namespace Comm.XMS.QrCodeLib
{

    public class RestruantInfo
    {
        public string resId;
        public string name;
        public float longtitude;
        public float latitude;
        public int isDelete;
        public string createName;
        public DateTime createTime = new DateTime();
        public DateTime updateTime;
        public string updateName;
        public int? version;
        public int unity;
        public int cityId;
        public string alphabet;
        public string headId;
        public string shortKeywords;
        public double minDiscount;
        public double maxDiscount;
        public string remark;
        public int enablePeriod;
        public int enableTiming;
        public int enableQueued;
        public string preferebtualInfo;
        public string regionId;
        public string address;
        public int averageFee;
        public string newResstyle;
        public int isGoodDish;
        public int taste;
        public int service;
        public int smd;
        public int impressionScore;
        public string discountRemark;
        public int enable3D;

        public List<ResClassify> resClassify = new List<ResClassify>();
        public List<ResPeroid> resPeroid = new List<ResPeroid>();
        public List<Discount> discount = new List<Discount>();
    }

    public class ResClassify
    {
        public int minPerson;
        public int maxPerson;
        public string tableType;
        public string tableShape;
        public string resId;
        public string rcID;
        public string name;
        public int isDelete;
        public string createName;
        public DateTime createTime = new DateTime();
        public DateTime updateTime;
        public string updateName;
        public int? version;
    }

    public class ResPeroid
    {
        public string peroidId;
        public string startTime;
        public string endTime;
        public int isDelete;
        public string createName;
        public DateTime createTime;
        public string updateName;
        public DateTime updateTime;
        public string resId;
        public int? version;
    }

    public class Discount {
        public string rcid { get; set; }
        public int weekFlag { get; set; }
        public string discount { get; set; }
        public int isDelete { get; set; }
        public string createName { get; set; }
        public DateTime createTime { get; set; }
        public string updateName { get; set; }
        public DateTime updateTime { get; set; }
        public string resId { get; set; }
        public int version { get; set; }
        public int Unity { get; set; }
        public string PeroidId { get; set; }
        public string remark { get; set; }
        public string discountRemark { get; set; }
    }

    public class TimeingRestruant
    {
        public string resId { get; set; }
        public string name { get; set; }
        public double longtitude { get; set; }
        public double latitude { get; set;}
        public int isDelete { get; set; }
        public string createName { get; set; }
        public string updateName { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int version { get; set; }
        public  int unity { get; set; }
        public  int cityId { get; set; }
        public string alphabet { get; set; }
        public string headId { get; set; }
        public double mindiscount { get; set; }
        public double maxDiscont { get; set; }
    }
    public class MarketType
    {
        public string categoryNumber { get; set; }
        public string categoryName { get; set; }
        public string reMark { get; set; }
        public string resId { get; set; }
        public string startTime { get; set; }
        public  string endTime { get; set; }
    }
    public class TimingResource
    {
        public string resourceFlag { get; set; }
        public string resourceName { get; set; }
        public string resId { get; set; }
        public int isDelete { get; set; }
        public string version { get; set; }
    }
    public class CombinedResourceItem
    {
        public string combinedResourceNo { get; set; }
        public int resourceId { get; set; }
        public int resourceFlag { get; set; }
        public  string resId { get; set; }
        public int isDelete { get; set; }
        public int? version { get; set; }
    }

    public class EmployeeInfo
    {
        public string employeeName { get; set; }
        public string gender { get; set; }
        public string employeeNo { get; set; }
        public string employeeGuid { get; set; }
        public string employeePwd { get; set; }
        public int isDelete { get; set; }
        public string resId { get; set; }
        public int? version { get; set; }
    }
    public class DiscountIllustration
    {
        public string resId { get; set; }
        public string mgsCode { get; set; }
        public string mgsInfo { get; set; }

    }
    public class DiscountInfo
    {
        public string startTime { get; set; }
        public int timeSilde { get; set; }
        public double discountRate { get; set; }
        public int categoryId { get; set; }
        public  string resId { get; set; }
        public int combindeResId { get; set; }
        public int isDelete { get; set; }
        public int version { get; set; }
        public int unity { get; set; }
        public string combinedResourceNo { get; set; }
        public string marketType { get; set; }

    }
}
