using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;

namespace PublicResource
{
    public class MyPing
    {
        private static MyPing objInstance = null;
        public static MyPing Instance
        {
            get
            {
                if (objInstance == null)
                    objInstance = new MyPing();
                return objInstance;
            }
        }
        public bool PingIP(string sDestinationIP, int nTimeoutInMillSeconds)
        {
            return GenralPing(sDestinationIP, nTimeoutInMillSeconds);
        }
        public bool PingHostName(string sHostName, int nTimeoutInMillSeconds)
        {
            return GenralPing(sHostName, nTimeoutInMillSeconds);
        }
        public double MultiplePingIP(string sDestinationIP, int nTimeoutInMillSeconds, int nPingCount,int nPingIntervalInMS)
        {
            return GenralMultiplePing(sDestinationIP, nTimeoutInMillSeconds, nPingCount, nPingIntervalInMS);
        }
        public double MultiplePingHostName(string sHostName, int nTimeoutInMillSeconds, int nPingCount, int nPingIntervalInMS)
        {
            return GenralMultiplePing(sHostName, nTimeoutInMillSeconds, nPingCount, nPingIntervalInMS);
        }
        private double GenralMultiplePing(string sHostOrIP, int nTimeoutInMillSeconds, int nPingCount, int nPingIntervalInMS)
        {
            if (nPingCount <= 0)
                return 0;
            if (nPingIntervalInMS <= 0)
                nPingIntervalInMS = 1;
            int nSuc = 0;
            for (int i = 0; i < nPingCount; i++)
            {
                bool b = PingIP(sHostOrIP, nTimeoutInMillSeconds);

                System.Threading.Thread.Sleep(nPingIntervalInMS);
                if (b)
                    nSuc++;
            }
            return nSuc / (double)nPingCount;
        }
        private bool GenralPing(string sHostOrIP, int nTimeoutInMillSeconds)
        {
            try
            {
                Ping objPing = new Ping();
                PingReply objReply = objPing.Send(sHostOrIP, nTimeoutInMillSeconds);
                
                if (objReply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.ToString());
            }
            return false;
        }

    }
}
