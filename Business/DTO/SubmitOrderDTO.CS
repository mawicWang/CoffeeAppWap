using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    [Serializable]
    public class SubmitOrderDTO
    {
        public int resId { get; set; }
        public int addrssId { get; set; }
        public int? couponId { get; set; }
        public int deliveryType { get; set; }
        public double deliveryMinTime { get; set; }
        public double deliveryMaxTime { get; set; }
        public decimal serverFee { get; set; }
        public string remark { get; set; }
        public int? outOfTime { get; set; }
        public List<Commodity> commodity = new List<Commodity>();
    }

    [Serializable]
    public class Commodity
    {
        public int commodityId { get; set; }
        public int quantity { get; set; }
        public List<int> skuList { get; set; }
    }
}
