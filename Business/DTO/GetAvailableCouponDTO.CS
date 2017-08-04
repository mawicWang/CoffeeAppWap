using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    [Serializable]
    public class GetAvailableCouponDTO
    {
        public int resId { get; set; }
        public int deliveryType { get; set; }
        public List<Commodity> commodity = new List<Commodity>();

        public long deliveryMinTime { get; set; }
        public long deliveryMaxTime { get; set; }
    }
}
