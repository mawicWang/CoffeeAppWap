using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.lib
{
    public class LockObject
    {
        private LockObject() { }
        public static readonly LockObject Instance = new LockObject();
    }
}
