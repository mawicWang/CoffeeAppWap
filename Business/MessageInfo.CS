using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    [Serializable]
    public class MessageInfo
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
