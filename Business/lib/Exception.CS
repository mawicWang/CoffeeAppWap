using System;
using System.Collections.Generic;
using System.Web;

namespace Business.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}