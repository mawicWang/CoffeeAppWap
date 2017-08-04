using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Tool
{
    /// <summary>
    /// 坐标处理工具
    /// </summary>
    public class CoordinateTool
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Longitude"></param>
        /// <param name="Latitude"></param>
        public CoordinateTool(double Longitude, double Latitude)
        {
            this.Longitude = Longitude;
            this.Latitude = Latitude;
        }

        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EARTH_RADIUS = 6378.137;//地球半径

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 实体对象
        /// </summary>
        public object obj { get; set; }

        /// <summary>
        /// 获取两个坐标的距离
        /// </summary>
        /// <param name="coordinateTool"></param>
        /// <returns></returns>
        public double Distance(CoordinateTool coordinateTool)
        {
            return GetDistance(this.Latitude, this.Longitude, coordinateTool.Latitude, coordinateTool.Longitude);
        }


        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
