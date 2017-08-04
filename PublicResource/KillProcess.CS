using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;

namespace PublicResource
{
    public class KillProcess
    {
        private static KillProcess objInstance = null;
        public static KillProcess Instance
        {
            get
            {
                if (objInstance == null)
                    objInstance = new KillProcess();
                return objInstance;
            }
        }
        /// <summary>
        /// 杀掉内存中进程，进程名应同任务管理器中一样，不区分大小写，如内存中有多个同名进程，杀掉第一个满足要求的进程
        /// </summary>
        /// <param name="sProcessName"></param>
        /// <returns></returns>
        public bool KillProcessByProcessName(string sProcessName)
        {
            try
            {
                if (String.IsNullOrEmpty(sProcessName))
                    return false;

                foreach (Process clsProcess in Process.GetProcesses())
                {


                    sProcessName = sProcessName.ToLower().Replace(".exe", "");
                    //now we're going to see if any of the running processes
                    //match the currently running processes by using the StartsWith Method,
                    //this prevents us from incluing the .EXE for the process we're looking for.
                    //. Be sure to not
                    //add the .exe to the name you provide, i.e: NOTEPAD,
                    //not NOTEPAD.EXE or false is always returned even if
                    //notepad is running
                    if (clsProcess.ProcessName.ToLower() == sProcessName)
                    {
                        //since we found the proccess we now need to use the
                        //Kill Method to kill the process. Remember, if you have
                        //the process running more than once, say IE open 4
                        //times the loop thr way it is now will close all 4,
                        //if you want it to just close the first one it finds
                        //then add a return; after the Kill
                      
                        clsProcess.Kill();
                        clsProcess.WaitForExit(30000);
                      
                       
                       
                        //process killed, return true
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.ToString());
                throw e;
            }
            //process not found, return false
            return false;
        }
        /// <summary>
        /// 启动windows服务，只要服务当前状态不是已停止，就停止它，并等待至停止成功，返回true。停止失败，抛出Exception
        /// </summary>
        /// <param name="sServiceName"></param>
        /// <returns></returns>
        public bool StopWindowService(string sServiceName)
        {
            try
            {
                if (String.IsNullOrEmpty(sServiceName))
                    return false;
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(sServiceName);
                if (sc != null)
                {
                    if (sc.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                    {

                        sc.Stop();
                        sc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                        return true;

                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.ToString());
                throw e;
            }
            //process not found, return false
            return false;
          
        }
        /// <summary>
        /// 启动windows服务，只要服务当前状态不是已启动，就启动它，并等待至启动成功，返回true。启动失败，抛出Exception
        /// </summary>
        /// <param name="sServiceName"></param>
        /// <returns></returns>
        public bool StartWindowService(string sServiceName)
        {
            try
            {
                if (String.IsNullOrEmpty(sServiceName))
                    return false;
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(sServiceName);
                if (sc != null)
                {
                    if (sc.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                    {

                        sc.Start();
                        sc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                        return true;

                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.ToString());
                throw e;
            }
            //process not found, return false
            return false;

        }
        /// <summary>
        /// 启动windows服务，如该服务已经启动，该服务将被停止，并在停止成功后启动，如该服务已停止，直接启动。进程将等待至启动成功后，返回true。停止失败或启动失败，都将抛出Exception。
        /// </summary>
        /// <param name="sServiceName"></param>
        /// <returns></returns>
        public bool RestartWindowService(string sServiceName)
        {
            bool b = StopWindowService(sServiceName);
            if (!b)
                return b;
            return StartWindowService(sServiceName);
                       
        }


        public Process GetProcessByWindowTitle(string sTitleName)
        {
            if (String.IsNullOrEmpty(sTitleName))
                return null;
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.MainWindowTitle.ToLower() == sTitleName.ToLower())
                    return clsProcess;

            }
            return null;

        }
        public Process GetProcessByProcessName(string sProcessName)
        {
            if (String.IsNullOrEmpty(sProcessName))
                return null;
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.ToLower() == sProcessName.ToLower())
                    return clsProcess;

            }
            return null;

        }
       

        public ServiceStatusRslt GetWindowServiceStatus(string sServiceName)
        {
            ServiceStatusRslt obj = new ServiceStatusRslt();
            obj.bIsServiceExist = false;

            try
            {
                
               
                if (!String.IsNullOrEmpty(sServiceName))
                {
                    System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(sServiceName);
                    if (sc != null)
                    {
                        obj.bIsServiceExist = true;
                        obj.eServiceStatus = sc.Status;
                        return obj;
                    }
                }
                
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.ToString());
                throw e;
            }
            //process not found, return false
            return obj;
        }
    }
    public class ServiceStatusRslt
    {
        public bool bIsServiceExist;
        public System.ServiceProcess.ServiceControllerStatus eServiceStatus;
    }
    
}
