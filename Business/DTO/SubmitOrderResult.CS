using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    [Serializable]
    public class SubmitOrderResult
    {
        public int id { get; set; }
        public Dictionary<string, string> payInfo { get; set; }
    }
}
