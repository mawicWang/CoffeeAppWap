using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Business;
//using LitJson;

namespace Business.lib
{
    /// <summary>
    /// ΢��֧��Э��ӿ������࣬���е�API�ӿ�ͨ�Ŷ�����������ݽṹ��
    /// �ڵ��ýӿ�֮ǰ���������ֶε�ֵ��Ȼ����нӿ�ͨ�ţ�
    /// ������Ƶĺô��ǿ���չ��ǿ���û��������Э����и��Ķ���������������ݽṹ��
    /// ������������ϳ���ͬ��Э�����ݰ�������Ϊÿ��Э�����һ�����ݰ��ṹ
    /// </summary>
    public class WxPayData
    {
        public WxPayData()
        {

        }

        //���������Dictionary�ĺô��Ƿ�������ݰ�����ǩ����������ǩ��֮ǰ����һ������
        public SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        /**
        * ����ĳ���ֶε�ֵ
        * @param key �ֶ���
         * @param value �ֶ�ֵ
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * �����ֶ�����ȡĳ���ֶε�ֵ
        * @param key �ֶ���
         * @return key��Ӧ���ֶ�ֵ
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * �ж�ĳ���ֶ��Ƿ�������
         * @param key �ֶ���
         * @return ���ֶ�key�ѱ����ã��򷵻�true�����򷵻�false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /**
        * @��Dictionaryת��xml
        * @return ��ת���õ���xml��
        * @throws WxPayException
        **/
        public string ToXml()
        {
            //����Ϊ��ʱ����ת��Ϊxml��ʽ
            if (0 == m_values.Count)
            {
                WCFClient.LoggerService.Error(this.GetType().ToString(), "WxPayData����Ϊ��!");
                throw new WxPayException("WxPayData����Ϊ��!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //�ֶ�ֵ����Ϊnull����Ӱ���������
                if (pair.Value == null)
                {
                    WCFClient.LoggerService.Error(this.GetType().ToString(), "WxPayData�ڲ�����ֵΪnull���ֶ�!");
                    throw new WxPayException("WxPayData�ڲ�����ֵΪnull���ֶ�!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//����string��int���Ͳ��ܺ���������������
                {
                    WCFClient.LoggerService.Error(this.GetType().ToString(), "WxPayData�ֶ��������ʹ���!");
                    throw new WxPayException("WxPayData�ֶ��������ʹ���!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /**
        * @��xmlתΪWxPayData���󲢷��ض����ڲ�������
        * @param string ��ת����xml��
        * @return ��ת���õ���Dictionary
        * @throws WxPayException
        */
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                WCFClient.LoggerService.Error(this.GetType().ToString(), "���յ�xml��ת��ΪWxPayData���Ϸ�!");
                throw new WxPayException("���յ�xml��ת��ΪWxPayData���Ϸ�!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//��ȡ�����ڵ�<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//��ȡxml�ļ�ֵ�Ե�WxPayData�ڲ���������
            }
			
            try
            {
				//2015-06-29 ������û��ǩ��
                if (!"SUCCESS".Equals(m_values["return_code"]))
				{
					return m_values;
				}
                //CheckSign();//��֤ǩ��,��ͨ�������쳣
            }
            catch(WxPayException ex)
            {
                throw new WxPayException(ex.Message);
            }

            return m_values;
        }

        /**
        * @Dictionary��ʽ����Json
         * @return json������
        */
        public string ToJson()
        {
            //string jsonStr = JsonMapper.ToJson(m_values);
            //return jsonStr;
            string sJsonStr = string.Empty;
            if (m_values != null)
            {
                if (m_values.GetType() == typeof(String) || m_values.GetType() == typeof(Exception))
                {
                    sJsonStr = m_values.ToString();
                }
                else
                {
                    JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                    sJsonStr = JsSerializer.Serialize(m_values);
                    sJsonStr = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(sJsonStr));
                }
            }
            return sJsonStr;

        }

        /**
        * @��ȡDictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }
    }
}