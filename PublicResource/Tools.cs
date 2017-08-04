using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Security.Cryptography;
namespace PublicResource
{
    /// <summary>
    /// Class1 的摘要说明。
    /// </summary>
    /// 
    public class BookingInfo
    {
        public BookingInfo()
        {

        }
        public string sDingTime;
        public string sBookerName = "";
        public int nNoOfDiner;
        public string sMobile = "";
        public string sTel = "";
        public int nFromType = 0;
    };
    public class Tools
    {
        public Tools()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private static Regex regScript = new Regex(@"<script>.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static Regex regIframe = new Regex(@"<iframe>.*?</iframe>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static Regex regEvent = new Regex(@"(javascript:*|onabort|onblur|onchange|onclick|ondblclick|onerror|onfocus|onkeydown|onkeypress|onkeyup|onload|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|onreset|onresize|onselect|onsubmit|onunload)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex regSpace = new Regex("[　 ]+");

        public static object objLock = new object();

        public static string TrimSpace_Multi_Head_Tail(string sIn)
        {
            if (String.IsNullOrEmpty(sIn))
                return sIn;
            sIn = sIn.Trim();
            return regSpace.Replace(sIn, " ");
        }

        public static DataTable MergeTables(DataTable dt1, DataTable dt2)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Merge(dt2);
            return ds.Tables[0];
        }
        public static DataTable GenerateDtFromDrs(DataRow[] drs)
        {
            if (drs.Length == 0)
                return null;

            DataTable newdt = drs[0].Table.Clone();
            foreach (DataRow dr in drs)
            {

                newdt.ImportRow(dr);
            }
            return newdt;
        }
        public static object GetXmlDeSerializer(string sInfo, object obj)
        {
            XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
            System.Xml.XmlReader myReader = new System.Xml.XmlTextReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sInfo)));
            try
            {
                object objRslt = mySerializer.Deserialize(myReader);
                return objRslt;
            }
            catch
            {

                return null;
            }


        }


        private static Hashtable _htSurname = null;
        public static Hashtable htSurname
        {
            get
            {
                if (_htSurname == null)
                {
                    _htSurname = new Hashtable();
                    _htSurname["司马"] = 1;
                    _htSurname["欧阳"] = 1;
                    _htSurname["司徒"] = 1;
                    _htSurname["上官"] = 1;
                    _htSurname["诸葛"] = 1;
                    _htSurname["慕容"] = 1;
                    _htSurname["皇甫"] = 1;
                    _htSurname["公孙"] = 1;
                    _htSurname["重光"] = 1;
                    _htSurname["德宫"] = 1;
                    _htSurname["纳兰"] = 1;
                    _htSurname["夏侯"] = 1;
                    _htSurname["令狐"] = 1;
                    _htSurname["尉迟"] = 1;
                    _htSurname["长孙"] = 1;
                    _htSurname["宇文"] = 1;
                }
                return _htSurname;
            }
        }

        public static string GetFirstLetterFromSingleHanzi(string sIn)
        {
            if (String.IsNullOrWhiteSpace(sIn))
                return sIn;
            sIn = sIn.Substring(0, 1);

            //将汉字转化为ASNI码,二进制序列
            byte[] arrCN = Encoding.Default.GetBytes(sIn);

            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = {45217,45253,45761,46318,46826,47010,47297,47614,48119,48119,49062,
            49324,49896,50371,50614,50622,50906,51387,51446,52218,52698,52698,52698,52980,53689,
            54481};
                for (int i = 0; i < 26; i++)
                {
                    int max = 58290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
            }
            return sIn;
        }


        //sIn = sIn.Substring(0, 1);
        //if (sIn.CompareTo("吖") < 0) return sIn;
        //if (sIn.CompareTo("八") < 0) return "A";
        //if (sIn.CompareTo("嚓") < 0) return "B";
        //if (sIn.CompareTo("哒") < 0) return "C";
        //if (sIn.CompareTo("妸") < 0) return "D";
        //if (sIn.CompareTo("发") < 0) return "E";
        //if (sIn.CompareTo("旮") < 0) return "F";
        //if (sIn.CompareTo("铪") < 0) return "G";
        //if (sIn.CompareTo("讥") < 0) return "H";
        //if (sIn.CompareTo("咔") < 0) return "J";
        //if (sIn.CompareTo("垃") < 0) return "K";
        //if (sIn.CompareTo("妈") < 0) return "L";
        //if (sIn.CompareTo("拏") < 0) return "M";
        //if (sIn.CompareTo("哦") < 0) return "N";
        //if (sIn.CompareTo("妑") < 0) return "O";
        //if (sIn.CompareTo("七") < 0) return "P";
        //if (sIn.CompareTo("然") < 0) return "Q";
        //if (sIn.CompareTo("仨") < 0) return "R";
        //if (sIn.CompareTo("他") < 0) return "S";
        //if (sIn.CompareTo("哇") < 0) return "T";
        //if (sIn.CompareTo("夕") < 0) return "W";
        //if (sIn.CompareTo("丫") < 0) return "X";
        //if (sIn.CompareTo("匝") < 0) return "Y";
        //if (sIn.CompareTo("咗") < 0) return "Z";

        // }
        public static string GetFirstLetterFromHanziString(string sIn)
        {
            if (String.IsNullOrWhiteSpace(sIn))
                return sIn;
            string sRslt = "";
            foreach (char c in sIn.ToCharArray())
            {
                sRslt += GetFirstLetterFromSingleHanzi(c.ToString());
            }

            return sRslt;
        }

        public static string GetSurnameFromName(string sName)
        {
            if (sName.Length < 2)
                return sName;
            string sTmpName = sName.Substring(0, 2);
            if (htSurname[sTmpName] != null)
                return sTmpName;
            return sName.Substring(0, 1);
        }




        public static string GetXmlSerializerString(object obj)
        {
            if (obj == null)
                return "";
            XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
            System.IO.MemoryStream objIo = new System.IO.MemoryStream();

            mySerializer.Serialize(objIo, obj);

            byte[] buffer = objIo.ToArray();


            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// 通过手机号生成密码
        /// </summary>
        /// <param name="sMobile"></param>
        /// <returns></returns>
        public static string GetCodeByMobile(string sMobile)
        {
            char[] a1 = sMobile.ToCharArray();
            int a = Convert.ToInt32(a1[4]);
            int b = Convert.ToInt32(a1[7]);
            int c = Convert.ToInt32(a1[9]);
            return ((a + b + c + 1) * 9).ToString() + (a * 5).ToString();
        }

        public static bool IsValidMobile(string sMobile)
        {
            if (String.IsNullOrEmpty(sMobile))
                return false;
            return Regex.IsMatch(sMobile, @"^1[3458]\d{9}$") || Regex.IsMatch(sMobile, @"^[0][2][1]\d{8}$");
        }

        public static bool IsChinaTelcomMobile(string sMobile)
        {
            if (sMobile.StartsWith("021") ||
                sMobile.StartsWith("189") ||
                sMobile.StartsWith("133") ||
                sMobile.StartsWith("180") ||
                sMobile.StartsWith("153")
                )
                return true;
            return false;
        }
        public static bool IsChinaUnicomMobile(string sMobile)
        {
            if (sMobile.StartsWith("130")
                || sMobile.StartsWith("131")
                || sMobile.StartsWith("132")
                || sMobile.StartsWith("156")
                || sMobile.StartsWith("155")
                || sMobile.StartsWith("145")
                || sMobile.StartsWith("185")
                || sMobile.StartsWith("186"))
                return true;
            return false;
        }
        /// <summary>
        /// 在当前运行程序路径下写入日志  默认wronglog文件夹下 
        /// </summary>
        /// <param name="sMessage">要写入的信息</param>
        public static void WriteLog(string sMessage)
        {
            WriteLog(sMessage, "", "");
        }
        /// <summary>
        /// 在当前运行程序路径下写入日志 
        /// </summary>
        /// <param name="sMessage">要写入的信息</param>
        /// <param name="sDirectoryName">文件夹名，没有则自动创建</param>
        public static void WriteLog(string sMessage, string sDirectoryName)
        {
            WriteLog(sMessage, sDirectoryName, "");
        }
        /// <summary>
        /// 在当前运行程序路径下写入日志 
        /// </summary>
        /// <param name="sMessage">要写入的信息</param>
        /// <param name="sDirectoryName">文件夹名，没有则自动创建</param>
        /// <param name="sFileName">自定义文件名,无需后缀，默认.config结尾</param>
        public static void WriteLog(string sMessage, string sDirectoryName, string sFileName)
        {
            lock (objLock)
            {
                try
                {
                    if (String.IsNullOrEmpty(sDirectoryName))
                    {
                        sDirectoryName = "Wronglog";
                    }
                    if (String.IsNullOrEmpty(sFileName))
                    {
                        string sDateTag = System.DateTime.Now.ToString("yyyyMMdd");
                        sFileName = "wrong" + sDateTag + ".config";
                    }
                    else
                    {
                        sFileName += ".config";
                    }
                    string sBase = System.AppDomain.CurrentDomain.BaseDirectory;
                    string sFolderPath = Directory.GetParent(sBase).FullName + "/" + sDirectoryName;
                    if (!Directory.Exists(sFolderPath))
                        Directory.CreateDirectory(sFolderPath);


                    string sPath = sFolderPath + "/" + sFileName;
                    using (StreamWriter sw = new StreamWriter(sPath, true))
                    {
                        sw.Write(sMessage + "	" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff" + "\r\n"));
                        sw.Flush();
                    }
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// 将对象转成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToJson(object obj)
        {
            string sJsonStr = string.Empty;
            if (obj != null)
            {
                if (obj.GetType() == typeof(String) || obj.GetType() == typeof(Exception))
                {
                    sJsonStr = obj.ToString();
                }
                else
                {
                    JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                    sJsonStr = JsSerializer.Serialize(obj);
                    sJsonStr = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(sJsonStr));
                }
            }
            return sJsonStr;
        }
        public static void WriteJassonLogByObject(object obj, string sFolder)
        {
            try
            {

                if (obj != null)
                {
                    if (obj.GetType() == typeof(String) || obj.GetType() == typeof(Exception))
                    {
                        WriteLog(obj.ToString(), sFolder);
                    }
                    else
                    {
                        JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                        string _sTemp = JsSerializer.Serialize(obj);
                        _sTemp = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(_sTemp));
                        WriteLog(_sTemp, sFolder);
                    }
                }
            }
            catch (System.Exception e)
            {
                WriteLog(e.ToString());
            }
        }
        public static void WriteJassonLogByObject(object obj)
        {
            WriteJassonLogByObject(obj, null);
        }
        public static void WriteXMSLogByObject(object obj)
        {

            try
            {
                if (obj != null)
                {
                    if (obj.GetType() == typeof(String))
                    {
                        WriteLog(obj.ToString());
                    }
                    else
                    {
                        WriteLog(GetXmlSerializerString(obj));
                    }

                }

            }
            catch (System.Exception e)
            {
                WriteLog(e.ToString());
            }



        }
        public static bool IsValidEmail(string sEmail)
        {
            if (sEmail.Length > 50)
                return false;
            //	string sPartern=@"^[a-z0-9]+([._\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){2,63}[a-z0-9]+$";
            string sPartern = @"^([a-zA-Z0-9_\-\.]+[^\.])@((\[[0-9]{1,3}\.[0-9]" +
                @"{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+)" +
                @")([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(sEmail, sPartern);
        }
        public static string GetGreetingWordByTime()
        {
            DateTime tNow = System.DateTime.Now;
            if (tNow.Hour > 17)
                return "晚上好";
            if (tNow.Hour >= 13)
                return "下午好";
            if (tNow.Hour >= 11)
                return "中午好";
            return "早上好";

        }
        public static string GetGreetingWordByBirthday(DateTime tBirthday)
        {
            DateTime tNewBirthday = new DateTime(1924, tBirthday.Month, tBirthday.Day, 0, 0, 0);
            DateTime tNow = System.DateTime.Now;
            DateTime tNewNow = new DateTime(1924, tNow.Month, tNow.Day, 0, 0, 0);
            TimeSpan ts = tNewNow - tNewBirthday;
            if (ts.Days <= 3 && ts.Days >= -3)
                return "生日快乐";
            return "";

        }
        public static string ConvertSex(string sInSex)
        {
            if (sInSex != "先生")
                return "小姐";
            return sInSex;
        }
        public static int FormatInt(string sIn)
        {
            return FormatInt((object)sIn);
        }

        public static int FormatInt(object objIn)
        {
            try
            {
                return Convert.ToInt32(objIn);
            }
            catch
            {
                return 0;

            }
        }

        public static int CInt(object oIn)
        {
            if (oIn == null)
                return 0;
            try
            {
                return Convert.ToInt32(FormatStr(oIn));
            }
            catch
            {
                return 0;

            }
        }
        public static long FormatLong(string sIn)
        {
            try
            {
                return Convert.ToInt64(sIn);
            }
            catch
            {
                return 0;

            }
        }
        public static double FormatDouble(string sIn)
        {
            try
            {
                return Convert.ToDouble(sIn);
            }
            catch
            {
                return 0;

            }
        }

        public static string FormatStr(object oIn)
        {
            return FormatStr(oIn, string.Empty);
        }

        public static string FormatStr(object oIn, string sDefault)
        {
            try
            {
                if (oIn == null)
                    return sDefault;
                return oIn.ToString();
            }
            catch
            {
                return sDefault;
            }
        }

        public static string ReplaceHTML(string sIn)
        {
            Regex obj = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.IgnoreCase);
            string sRslt = obj.Replace(sIn, "");
            return sRslt;


        }
        public static string GetSummaryFromWords(string sIn)
        {
            if (sIn.IndexOf("。") < 0)
                return sIn;

            return sIn.Substring(0, sIn.IndexOf("。"));
        }
        public static void SetBaseForSon(object objParent, object objSon)
        {
            Type MyType = objParent.GetType();
            foreach (PropertyInfo pi in MyType.GetProperties())
            {

                string sName = pi.Name.ToUpper();
                if (!pi.CanWrite)
                    continue;
                object objT1 = pi.GetValue(objParent, null);
                pi.SetValue(objSon, objT1, null);
            }

        }

        public static DateTime GetOrderAddTimeByOrderId(string sOrderId)
        {
            if (sOrderId == "" || sOrderId == null)
                return Constant.tNULLDateTime;
            Regex obj = new Regex(@"[A-Z]");
            if (obj.IsMatch(sOrderId))
            {
                string sRslt = "";


                Match obj3 = obj.Match(sOrderId);
                int nIndex = 0;
                while (obj3 != null)
                {
                    char[] a = obj3.Value.ToCharArray();
                    int n = a[0] - 65;
                    if (nIndex == 0)
                        n = 2008 + n;
                    else
                        n = 1 + n;
                    sRslt += n + "-";
                    if (nIndex == 1)
                    {
                        int nt = obj3.Index;
                        obj3 = obj3.NextMatch();
                        int nt1 = obj3.Index;
                        sRslt += sOrderId.Substring(nt + 1, nt1 - nt - 1);
                        break;

                    }
                    else
                    {
                        nIndex++;
                        obj3 = obj3.NextMatch();
                    }


                }

                return Convert.ToDateTime(sRslt);


            }
            else
            {
                string sTmp = sOrderId.Substring(0, 4) + "-" + sOrderId.Substring(4, 2) + "-" + sOrderId.Substring(6, 2);
                return Convert.ToDateTime(sTmp);
            }
        }
        public static string GetNewSex(string sSex)
        {
            if (sSex == "女士")
                return "小姐";
            return sSex;
        }
        public static string GetTypeNameFromXMSType(string sType)
        {
            switch (sType)
            {
                case "DCY":
                    return "订餐员";

                case "XMS":
                    return "小秘书";

                case "DCXMS":
                    return "小秘书";

                case "HFY":
                    return "回访员";

                case "JFY":
                    return "积分员";

            }
            return "";
        }

        public static string GetDayOfWeekString(DateTime tDate)
        {
            string sWeekDays = tDate.DayOfWeek.ToString("d");
            if (sWeekDays == "0")
                sWeekDays = "日";
            sWeekDays = "周" + sWeekDays;
            return sWeekDays;
        }
        public static bool GetArrayByEnumType(Type obj, out string[] sNames, out int[] nValues)
        {

            if (!obj.IsEnum)
            {
                sNames = null;
                nValues = null;
                return false;
            }

            sNames = Enum.GetNames(obj);


            nValues = (int[])Enum.GetValues(obj);
            if (sNames.Length == 0)
                return false;
            return true;
        }
        public static string ToDBC(string input)
        {
            if (input == null) return "";
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }


            return System.Text.RegularExpressions.Regex.Replace(new string(c), @"[^\u001E-\uffff]+", "");
        }

        public static object GetSumOfObject(object[] objs)
        {
            object obj1 = null;
            int nIndex = 0;
            foreach (object obj in objs)
            {
                Type MyType = obj.GetType();
                if (nIndex == 0)
                {
                    obj1 = Activator.CreateInstance(MyType);
                    nIndex++;
                }


                foreach (FieldInfo pi in MyType.GetFields())
                {
                    if (pi.FieldType == typeof(Int32))
                    {
                        int n = (int)pi.GetValue(obj1);
                        int n1 = (int)pi.GetValue(obj);
                        pi.SetValue(obj1, n + n1);
                    }
                    else
                        if (pi.FieldType == typeof(double))
                        {
                            double n = (double)pi.GetValue(obj1);
                            double n1 = (double)pi.GetValue(obj);
                            pi.SetValue(obj1, n + n1);
                        }

                }
                foreach (PropertyInfo pi in MyType.GetProperties())
                {
                    if (pi.PropertyType == typeof(Int32))
                    {
                        int n = (int)pi.GetValue(obj1, null);
                        int n1 = (int)pi.GetValue(obj, null);
                        pi.SetValue(obj1, n + n1, null);
                    }
                    else
                        if (pi.PropertyType == typeof(double))
                        {
                            double n = (double)pi.GetValue(obj1, null);
                            double n1 = (double)pi.GetValue(obj, null);
                            pi.SetValue(obj1, n + n1, null);
                        }

                }
            }
            return obj1;

        }

        public static string GetSafeSqlPara(string sInSqlPara)
        {
            if (String.IsNullOrEmpty(sInSqlPara))
                return "";
            return sInSqlPara.Replace("[", "").Replace("]", "").Replace("'", "''").Replace("%", "");
        }
        public static bool IsValidSex(string sSex)
        {
            if (String.IsNullOrEmpty(sSex))
                return false;
            if (sSex == "先生" || sSex == "女士")
                return true;
            return false;
        }

        public static bool IsValidDateTime(string sid)
        {
            bool isTrue = false;
            try
            {
                DateTime Uptime = DateTime.Parse(sid);
                isTrue = true;
            }
            catch
            {
                isTrue = false;
            }
            return isTrue;
        }
        public static DateTime? ConvertToDateTime(string sDate)
        {
            string ShortFormat = "yyyy-MM-dd HH:mm:ss";
            string LongFormat = "yyyy-MM-dd HH:mm:ss:fff";
            string sFormat = "yyyy-MM-dd HH:mm";
            string sFormatDate = "yyyy-MM-dd";
            string[] sList = new string[] { ShortFormat, LongFormat, sFormat, sFormatDate };
            return ConvertToDateTime(sDate, sList);
        }

        public static DateTime? ConvertToDateTime(string sDate, string[] sFormat)
        {
            try
            {
                CultureInfo inf = CultureInfo.InvariantCulture;
                DateTime dt = DateTime.ParseExact(sDate, sFormat, inf, DateTimeStyles.AssumeLocal);
                return dt;
            }
            catch
            {
            }
            return null;
        }

        public static bool isChinese(string s)
        {

            Regex r4 = new Regex(@"[\u4e00-\u9fa5]+");
            return r4.IsMatch(s);
        }

        public static bool IsEnumValid(string sValue, Type objType)
        {
            try
            {
                Enum.Parse(objType, sValue, false);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static object GetEnumByString(string sEnumString, System.Type objType)
        {
            try
            {
                return Enum.Parse(objType, sEnumString, false);

            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 取得一段汉字的首字母
        /// </summary>
        /// <param name="sChineseCharacter">汉字</param>
        /// <returns>返回字母</returns>
        public static string GetGbkX(string sChineseCharacter)
        {
            if (string.IsNullOrEmpty(sChineseCharacter))
                return string.Empty;
            if (sChineseCharacter.CompareTo("吖") < 0)
            {
                return sChineseCharacter.Substring(0, 1).ToUpper();
            }
            if (sChineseCharacter.CompareTo("八") < 0) return "A";
            if (sChineseCharacter.CompareTo("嚓") < 0) return "B";
            if (sChineseCharacter.CompareTo("咑") < 0) return "C";
            if (sChineseCharacter.CompareTo("妸") < 0) return "D";
            if (sChineseCharacter.CompareTo("发") < 0) return "E";
            if (sChineseCharacter.CompareTo("旮") < 0) return "F";
            if (sChineseCharacter.CompareTo("铪") < 0) return "G";
            if (sChineseCharacter.CompareTo("讥") < 0) return "H";
            if (sChineseCharacter.CompareTo("咔") < 0) return "J";
            if (sChineseCharacter.CompareTo("垃") < 0) return "K";
            if (sChineseCharacter.CompareTo("嘸") < 0) return "L";
            if (sChineseCharacter.CompareTo("拏") < 0) return "M";
            if (sChineseCharacter.CompareTo("噢") < 0) return "N";
            if (sChineseCharacter.CompareTo("妑") < 0) return "O";
            if (sChineseCharacter.CompareTo("七") < 0) return "P";
            if (sChineseCharacter.CompareTo("亽") < 0) return "Q";
            if (sChineseCharacter.CompareTo("仨") < 0) return "R";
            if (sChineseCharacter.CompareTo("他") < 0) return "S";
            if (sChineseCharacter.CompareTo("哇") < 0) return "T";
            if (sChineseCharacter.CompareTo("夕") < 0) return "W";
            if (sChineseCharacter.CompareTo("丫") < 0) return "X";
            if (sChineseCharacter.CompareTo("帀") < 0) return "Y";
            if (sChineseCharacter.CompareTo("咗") < 0) return "Z";
            return sChineseCharacter.Substring(0, 1).ToUpper(); ;
        }

        public static ReturnValue Get500ErrorAndWriteLog(System.Exception e)
        {
            if (e == null)
                return null;
            WriteLog(e.ToString());
            ReturnValue objRslt = new ReturnValue();
            objRslt.nRslt = 500;
            objRslt.objInfo = e.ToString();
            objRslt.sMessage = e.ToString();
            return objRslt;
        }
        public static ReturnValue Get404Error(string sMessage, object objErrorCode)
        {
            //被坑爹后的修改...
            //if (String.IsNullOrEmpty(sMessage))
            //    return null;
            ReturnValue objRslt = new ReturnValue();
            objRslt.nRslt = 404;
            objRslt.objInfo = objErrorCode;
            objRslt.sMessage = sMessage;
            return objRslt;
        }
        public static ReturnValue Get1000Error(string sMessage, object objErrorCode)
        {
            if (String.IsNullOrEmpty(sMessage))
                return null;
            ReturnValue objRslt = new ReturnValue();
            objRslt.nRslt = 1000;
            objRslt.objInfo = objErrorCode;
            objRslt.sMessage = sMessage;
            return objRslt;
        }
        public static string GetVelocityTag(string sTagName, string sTagValue)
        {
            if (String.IsNullOrEmpty(sTagName) || String.IsNullOrEmpty(sTagValue))
                return null;
            return sTagName.ToLower() + "^" + sTagValue.ToLower();
        }
        public static List<string> GetVelocityTagList(Hashtable ht)
        {
            if (ht == null || ht.Count == 0)
            {
                return null;
            }
            List<string> lstRslt = new List<string>();
            foreach (DictionaryEntry e in ht)
            {
                lstRslt.Add(GetVelocityTag(e.Key.ToString(), e.Value.ToString()));
            }
            return lstRslt;
        }
        public static object Convert2Object(DataRow dr, System.Type objType)
        {
            if (dr != null)
            {
                object objRslt = Activator.CreateInstance(objType);
                foreach (PropertyInfo pi in objType.GetProperties())
                {

                    string name = pi.Name;
                    if (!pi.CanWrite)
                        continue;

                    if (dr.Table.Columns.Contains(name))
                    {
                        if (!dr[name].Equals(System.DBNull.Value))
                        {
                            if (pi.PropertyType.IsEnum)
                            {
                                pi.SetValue(objRslt, Enum.ToObject(pi.PropertyType, dr[name]), null);
                            }
                            else
                            {
                                if (pi.PropertyType == typeof(int))
                                {
                                    pi.SetValue(objRslt, Convert.ToInt32(dr[name]), null);
                                }
                                else
                                    if (pi.PropertyType == typeof(double))
                                    {
                                        pi.SetValue(objRslt, Convert.ToDouble(dr[name]), null);
                                    }
                                    else if (pi.PropertyType == typeof(decimal))
                                    {
                                        pi.SetValue(objRslt, Convert.ToDecimal(dr[name]), null);
                                    }
                                    else if (pi.PropertyType == typeof(float))
                                    {
                                        pi.SetValue(objRslt, Convert.ToSingle(dr[name]), null);
                                    }
                                    else
                                        if (pi.PropertyType == typeof(string))
                                            pi.SetValue(objRslt, dr[name].ToString(), null);
                                        else
                                            if (pi.PropertyType == typeof(long))
                                            {
                                                pi.SetValue(objRslt, Convert.ToInt64(dr[name]), null);
                                            }
                                            else
                                                pi.SetValue(objRslt, dr[name], null);
                            }
                        }
                        else
                        {

                            if (pi.PropertyType == typeof(string))
                                pi.SetValue(objRslt, "", null);
                        }

                    }


                }


                foreach (FieldInfo pi in objType.GetFields())
                {

                    string name = pi.Name;


                    if (dr.Table.Columns.Contains(name))
                    {
                        if (!dr[name].Equals(System.DBNull.Value))
                        {
                            if (pi.IsPrivate)
                            {
                                continue;
                            }
                            else
                            {
                                if (pi.FieldType == typeof(Int32))
                                {

                                    pi.SetValue(objRslt, Convert.ToInt32(dr[name]));
                                }
                                else
                                    if (pi.FieldType == typeof(double))
                                    {
                                        pi.SetValue(objRslt, Convert.ToDouble(dr[name]));
                                    }
                                    else
                                        if (pi.FieldType == typeof(string))
                                        {
                                            string s = dr[name].ToString();
                                            // s=Regex.Replace(s, @"[^a-zA-Z0-9_\-\u4e00-\u9fa5]", "", RegexOptions.Compiled);
                                            pi.SetValue(objRslt, s);
                                        }
                                        else
                                            if (pi.FieldType == typeof(long))
                                            {
                                                pi.SetValue(objRslt, Convert.ToInt64(dr[name]));
                                            }
                                            else
                                                pi.SetValue(objRslt, dr[name]);
                            }
                        }
                        else
                        {

                            if (pi.FieldType == typeof(string))
                                pi.SetValue(objRslt, "");
                        }

                    }


                }
                return objRslt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// MD5 一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            return Md5(str, 32);
        }
        /// <summary>
        /// MD5 一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="code">结果的长度，只能是32 或者 16 ， 默认32 </param>
        /// <returns></returns>
        public static string Md5(string str, int code)
        {
            if (String.IsNullOrEmpty(str))
                str = String.Empty;
            if (code == 16)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }

        /// <summary>
        /// 根据配置文件给对象赋值(不要改变原对象的值!!)
        /// </summary>
        /// <param name="oSource"></param>
        /// <param name="oDestinate"></param>
        /// <param name="sObjectName"></param>
        public static void CopyObject(object oSource, object oDestinate)
        {
            if (oSource == null || oDestinate == null) return;
            Type typeSource, typeDestinate;
            typeSource = oSource.GetType();
            typeDestinate = oDestinate.GetType();
            PropertyInfo[] sourceProperties = typeSource.GetProperties();
            PropertyInfo[] destinateProperties = typeDestinate.GetProperties();
            foreach (PropertyInfo destinateProperty in destinateProperties)
            {
                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    if (sourceProperty.Name.ToLower().Equals(destinateProperty.Name.ToLower()) && sourceProperty.PropertyType.Equals(destinateProperty.PropertyType))
                    {
                        if (destinateProperty.CanWrite)
                        {
                            try
                            {
                                object oValue = sourceProperty.GetValue(oSource, null);
                                oValue = ReturnObjectType(destinateProperty.PropertyType, oValue);
                                destinateProperty.SetValue(oDestinate, oValue, null);
                            }
                            catch
                            {
                                WriteLog(string.Format("赋值出错，字段名:{0},原类型:{1},目标类型:{2}"
                                   , sourceProperty.Name, sourceProperty.PropertyType.ToString(), destinateProperty.PropertyType.ToString()));
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 对象类型进行转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="oValue"></param>
        /// <returns></returns>
        private static object ReturnObjectType(Type type, object oValue)
        {
            if (oValue == null) return null;
            if (type == (typeof(int)))
            {
                try
                {
                    oValue = Convert.ToInt32(oValue);
                }
                catch
                {
                    oValue = 0;
                }
            }
            else if (type == (typeof(DateTime))) oValue = Convert.ToDateTime(oValue);
            else if (type == (typeof(Guid))) oValue = new Guid(oValue.ToString());
            else if (type == (typeof(double))) oValue = Convert.ToDouble(oValue);
            else if (type == (typeof(Enum))) oValue = Enum.ToObject(type, oValue);
            else if (type == (typeof(string))) oValue = oValue.ToString();
            else if (type == (typeof(bool))) oValue = Convert.ToBoolean(oValue);
            return oValue;
        }

        /// <summary>
        /// 剪字(以中文字长度为准)
        /// </summary>
        /// <param name="soustrP"></param>
        /// <param name="lenP"></param>
        /// <param name="AppendP"></param>
        /// <returns></returns>
        public static string CutStr(string soustrP, int lenP, string AppendP)
        {
            lenP = lenP * 2;
            if (String.IsNullOrEmpty(soustrP))
                return String.Empty;
            if (lenP >= Encoding.Default.GetByteCount(soustrP))
                return soustrP;
            else
            {
                string ret = String.Empty;
                int len = 0;
                for (int i = 0; i < soustrP.Length; i++)
                {
                    if (len + Encoding.Default.GetByteCount(soustrP[i].ToString()) <= lenP)
                    {
                        ret += soustrP[i].ToString();
                        len += Encoding.Default.GetByteCount(soustrP[i].ToString());
                    }
                    else
                        break;
                }
                if (String.IsNullOrEmpty(AppendP))
                    return ret;
                else
                    return ret + AppendP;
            }
        }

        /// <summary>
        /// 将HTML字符变成普通字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtmlString(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("'", "’");
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            return str;
        }

        public static string ToCommonText(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;
            str = str.Replace("<br/>", "\r\n");
            return str;

        }

        /// <summary>
        /// 不精确的移除全部的HTML标签
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RemoveHtml(string args)
        {
            if (String.IsNullOrEmpty(args))
                return "";
            return (new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>")).Replace(args, "");
        }

        /// <summary>
        /// 去除内容中的script代码
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
        public static string RemoveScript(string sContent)
        {
            if (string.IsNullOrEmpty(sContent))
            {
                return string.Empty;
            }
            sContent = regScript.Replace(sContent, "");
            sContent = regIframe.Replace(sContent, "");
            sContent = regEvent.Replace(sContent, "");
            return sContent;
        }

        /// <summary>
        /// 格式化Json字符串
        /// </summary>
        /// <param name="sContent">待处理的字符串</param>
        /// <returns></returns>
        public static string FormatJsonStr(string sContent)
        {
            if (string.IsNullOrEmpty(sContent))
            {
                return string.Empty;
            }
            sContent = Regex.Replace(sContent, @"[\r\n]", "<br />");
            sContent = sContent.Replace("\"", "\\\"");
            return sContent;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string tempIp = HttpContext.Current.Request.Headers["Cdn-Src-Ip"] ?? HttpContext.Current.Request.Headers["X-Forwarded-For"] ?? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ?? HttpContext.Current.Request.UserHostAddress ?? "0.0.0.0";
            string[] args = System.Text.RegularExpressions.Regex.Split(tempIp, @"[^\d\.:a-zA-Z]");
            foreach (string s in args)
            {
                if (!String.IsNullOrEmpty(s) && !s.ToLower().Equals("unkown"))
                {
                    tempIp = s;
                    break;
                }
            }
            return tempIp;
        }


        /// <summary>
        /// 输入字符过滤
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetChineseString(string strRow)
        {
            if (String.IsNullOrEmpty(strRow))
                return String.Empty;
            string str = strRow;

            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("'", "’");
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            System.Collections.ArrayList ar = BadWords();
            if (ar != null)
            {
                for (int i = 0; i < ar.Count; i++)
                {
                    str = str.Replace(ar[i].ToString().Trim(), "*");
                }
            }

            return str;
        }

        private static ArrayList BadWords()
        {
            string hashKeyId = "PagesHtmlBadWords";
            ArrayList ArrBad = PublicResource.CacheHelper.objInstance.GetCachedItem(hashKeyId) as ArrayList;
            if (ArrBad == null)
            {
                ArrBad = new ArrayList();
                try
                {
                    string fileName = Path.Combine(ConfigurationManager.AppSettings["Fg114WebsiteRoot"], "StaticHtml/badwords/items.txt");
                    if (File.Exists(fileName))
                    {
                        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
                        {
                            string argsLine;
                            while ((argsLine = reader.ReadLine()) != null)
                            {
                                if (!String.IsNullOrEmpty(argsLine) && !ArrBad.Contains(argsLine.Trim()))
                                    ArrBad.Add(argsLine.Trim());
                            }
                        }
                        PublicResource.CacheHelper.objInstance.SetCachedItem(hashKeyId, 0, ArrBad, fileName);
                    }
                }
                catch
                {
                    ;
                }
            }

            return ArrBad;
        }
        /// <summary>
        /// 如果全汉字或空格，true，else false
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static bool IsStringOnlyChinese(string sInput)
        {
            if (String.IsNullOrEmpty(sInput))
                return false;
            Regex objReg = new Regex("^[" + PublicResource.Constant.ChineseRegular + @"\s]+$");
            return objReg.IsMatch(sInput);
        }

        /// <summary>
        /// 如果全汉字或空格or数字，true，else false
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static bool IsStringOnlyChineseAndDigital(string sInput)
        {
            if (String.IsNullOrEmpty(sInput))
                return false;
            Regex objReg = new Regex("^[" + PublicResource.Constant.ChineseRegular + @"\s0-9]+$");
            return objReg.IsMatch(sInput);
        }
        /// <summary>
        /// 如果包含汉字
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static bool IsStringIncludeChinese(string sInput)
        {
            if (String.IsNullOrEmpty(sInput))
                return false;
            Regex objReg = new Regex("[" + PublicResource.Constant.ChineseRegular + "]+");
            return objReg.IsMatch(sInput);
        }
        /// <summary>
        /// UBB转html标签
        /// </summary>
        /// <param name="sUbbText"></param>
        /// <returns></returns>
        public static string UbbToHtml(string sUbbText)
        {
            if (string.IsNullOrEmpty(sUbbText))
            {
                return string.Empty;
            }
            //暂时只替换常用标签
            sUbbText = Regex.Replace(sUbbText, @"\[url=(?<url>.+?)](?<text>.+?)\[/url]", @"<a href=""${url}"" target=""_blank"">${text}</a>", RegexOptions.IgnoreCase);
            sUbbText = Regex.Replace(sUbbText, @"\[img](?<url>.+?)\[/img]", @"<img src=""${url}"" border='0'>", RegexOptions.IgnoreCase);
            sUbbText = Regex.Replace(sUbbText, @"\[b](?<text>.+?)\[/b]", "<b>${text}</b>", RegexOptions.IgnoreCase);
            sUbbText = Regex.Replace(sUbbText, @"\[color=(?<color>.+?)](?<text>.+?)\[/color]", @"<font color=""${color}"">${text}</font>", RegexOptions.IgnoreCase);

            return sUbbText;
        }


        /// <summary>
        /// 取中英文名（返回长度为2的数组，分别是中英文名）
        /// </summary>
        /// <param name="sResName"></param>
        /// <returns></returns>
        public static string[] GetNationName(string sResName)
        {
            string[] arrResName = new string[] { sResName, "" };

            if (!string.IsNullOrEmpty(sResName))
            {
                //如果是全英文
                if (Regex.IsMatch(sResName, @"^[A-Za-z\d\s'&]+$"))
                {
                    arrResName[0] = sResName;
                    arrResName[1] = sResName;
                }
                else
                {
                    Regex regName = new Regex(@"(?<chinese>.+?)\s+(?<english>[A-Za-z\s,.'\d&]+)$", RegexOptions.IgnoreCase);
                    Match m = regName.Match(sResName);
                    if (m.Success)
                    {
                        arrResName[0] = m.Groups["chinese"].Value;
                        arrResName[1] = m.Groups["english"].Value;
                    }
                }

            }
            return arrResName;
        }

        /// <summary>
        /// 获取Byte数组的MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(byte[] input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(input);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }
    }
}