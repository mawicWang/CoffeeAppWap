using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace PublicResource
{
    public class XMlHelper
    {
        public static XMlHelper objInstance = new XMlHelper();

        private string GetCacacheName(string sItemName)
        {
            return "CONFIGCACHE" + sItemName;
        }
        private string sConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
        public object GetConfigByItemName(string sItemName)
        {
          
           
            try
            {
                object obj = PublicResource.CacheHelper.objInstance.GetCachedItem(GetCacacheName(sItemName));
                if (obj == null)
                {
                    XmlDocument xdoc = new XmlDocument();
                    if (!File.Exists(sConfigFile))
                        return null;
                    xdoc.Load(sConfigFile);
                    obj= xdoc.SelectSingleNode("root/" + sItemName).InnerXml;
                    PublicResource.CacheHelper.objInstance.SetCachedItem(GetCacacheName(sItemName), 0, obj, sConfigFile);
                }
                return obj;
            }
            catch
            {
                return null;
            }
            
          
        }
        public List<string> GetConfigListByItemName(string sItemName)
        {


            try
            {
                string sInnerText= GetConfigByItemName(sItemName) as string;
                if(String.IsNullOrEmpty(sInnerText))
                    return null;
                XmlDocument objDoc=new XmlDocument();
                objDoc.LoadXml("<root>"+sInnerText+"</root>");
                
                XmlNodeList objNodeList=objDoc.ChildNodes;
                if(objNodeList!=null&& objNodeList.Count>0)
                {
                    List<string> objList = new List<string>();
                    foreach (XmlNode objNode in objNodeList)
                    {
                        objList.Add(objNode.InnerText);
                    }
                    return objList;
                }
                return null;
            }
            catch
            {
                return null;
            }


        }


    }
   
}
