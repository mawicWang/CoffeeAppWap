using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace PublicResource
{
    public class TCPHelper
    {
        private static TCPHelper objInstance = null;
        public static TCPHelper Instance
        {
            get
            {
                if (objInstance == null)
                    objInstance = new TCPHelper();
                return objInstance;
            }
        }
        public bool IsPortExist(string sIP,int nPort)
        {
            TcpClient TcpScan = new TcpClient();
            try{
                TcpScan.Connect(sIP, nPort);
                return true;
            }
            catch{
                return false;
            }
        }
    }
}
