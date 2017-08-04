using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class WCFClient
    {
        /// <summary>
        /// 初始化logger实例
        /// </summary>
        public static XMS.Core.Logging.ILogService LoggerService
        {
            get
            {
                return XMS.Core.Container.Instance.Resolve<XMS.Core.Logging.ILogService>();
            }
        }

        public static XMS.Inner.Coffee.Service.ICoffeeService CoffeeService
        {
            get
            {
                return XMS.Core.Container.Instance.Resolve<XMS.Inner.Coffee.Service.ICoffeeService>();
            }
        }
    }
}