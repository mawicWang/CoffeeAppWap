using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace PublicResource
{
    public static class IOHelper
    {
        /// <summary>
        /// 得到一个分页后的SQL语句
        /// </summary>
        /// <param name="表名"></param>
        /// <param name="需要返回的列"></param>
        /// <param name="排序的字段名"></param>
        /// <param name="总条数">请传进来总条数哦</param>
        /// <param name="每页条数"></param>
        /// <param name="当前页数">请保证大于总页数</param>
        /// <param name="排序条件">请不要加Order by语句</param>
        /// <param name="Where条件">请不要再以Where开头</param>
        /// <returns></returns>
        public static string GetPagerSql(string 表名, string 需要返回的列, string 主键名, int 总条数, int 每页条数, int 当前页数, string 排序条件, string Where条件)
        {
            if (String.IsNullOrEmpty(主键名) || String.IsNullOrEmpty(表名) || 总条数 < 1 || 每页条数 < 1) return null;
            当前页数 = Math.Max(1, 当前页数);
            需要返回的列 = 需要返回的列 ?? "*";
            排序条件 = 排序条件 ?? 主键名;
            if (String.IsNullOrEmpty(Where条件)) Where条件 = "1=1";

            if (!Regex.IsMatch(排序条件, @"\s(asc|desc)$", RegexOptions.IgnoreCase | RegexOptions.Compiled)) 排序条件 += " desc";
            if (当前页数 * 每页条数 > 总条数) 当前页数 = (int)Math.Ceiling(((double)总条数) / ((double)每页条数));
            string sql = String.Empty;
            if (当前页数 == 1)
            {
                sql = String.Format("select top {2} {0} from {1}  {4}  order by {3} ", 需要返回的列, 表名, 每页条数, 排序条件, String.IsNullOrEmpty(Where条件) ? "" : (" where " + Where条件));
            }
            else
            {
                sql = String.Format("select top {0} {1} from {2}  where {3} not in(select top " + (每页条数 * (当前页数 - 1)) + "  {3} from {2} where {4} order by {5} ) and  {4}  order by {5}",
                    每页条数,
                    需要返回的列,
                    表名,
                    主键名,
                   Where条件,
                    排序条件
                    );
            }
            return sql;
        }

        public static string GetPagerSqlFor2008(string 表名, string 需要返回的列, string 主键名, int 总条数, int 每页条数, int 当前页数, string 排序条件, string Where条件)
        {
            if (String.IsNullOrEmpty(主键名) || String.IsNullOrEmpty(表名) || 总条数 < 1 || 每页条数 < 1) return null;
            当前页数 = Math.Max(1, 当前页数);
            需要返回的列 = 需要返回的列 ?? "*";
            排序条件 = 排序条件 ?? 主键名;
            if (String.IsNullOrEmpty(Where条件)) Where条件 = "1=1";

            if (!Regex.IsMatch(排序条件, @"\s(asc|desc)$", RegexOptions.IgnoreCase | RegexOptions.Compiled)) 排序条件 += " desc";
            if (当前页数 * 每页条数 > 总条数) 当前页数 = (int)Math.Ceiling(((double)总条数) / ((double)每页条数));
            string sql = String.Empty;
            if (当前页数 == 1)
            {
                sql = String.Format("select top {2} {0} from {1}  {4}  order by {3} ", 需要返回的列, 表名, 每页条数, 排序条件, String.IsNullOrEmpty(Where条件) ? "" : (" where " + Where条件));
            }
            else
            {
                sql = String.Format("select * from (select  " + 需要返回的列 + ",(row_number() over (order by " + 排序条件 + ")) r from " + 表名 + " " + (String.IsNullOrEmpty(Where条件) ? "" : (" where " + Where条件)) + "    ) t where r between {0} and {1}", (当前页数 - 1) * 每页条数 + 1, 当前页数 * 每页条数);
            }
            return sql;
        }

        /// <summary>
        /// 得到一个合理的文件名称
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public static string GetValidFileName(string sFileName)
        {
            if (String.IsNullOrEmpty(sFileName)) return null;
            foreach (char lDisallowed in Path.GetInvalidFileNameChars())
            {
                sFileName = sFileName.Replace(lDisallowed.ToString(), "");
            }
            foreach (char lDisallowed in Path.GetInvalidPathChars())
            {
                sFileName = sFileName.Replace(lDisallowed.ToString(), "");
            }
            return sFileName;
        }

        /* - - - - - - - - - - - - - - - - - - - - - - - - 
         * Stream 和 byte[] 之间的转换
         * - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        /* - - - - - - - - - - - - - - - - - - - - - - - - 
         * Stream 和 文件之间的转换
         * - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary>
        /// 将 Stream 写入文件
        /// </summary>
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        public static Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static string GetSqlWhere(Hashtable ht, Hashtable htWhere)
        {
            string str = string.Empty;
            if ((ht != null) && (htWhere != null))
            {
                string pattern = @"^\w+(?<sql>.*)$";
                int j = 0;
                foreach (DictionaryEntry entry in ht)
                {
                    j++;
                    string input = entry.Key.ToString();
                    Match match = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(input);
                    string str4 = "=";
                    if (match.Success)
                    {
                        str4 = match.Groups["sql"].Value;
                    }
                    if (string.IsNullOrEmpty(str4)) str4 = "=";
                    string str5 = Regex.Replace(input, @"[^\w].*$", "");
                    htWhere[str5 + j.ToString()] = entry.Value;
                    string str6 = str4;
                    if (str6 == null)
                    {
                        goto Label_011C;
                    }
                    if (!(str6 == "%like"))
                    {
                        if ((str6 == "like") || (str6 == "%like%"))
                        {
                            goto Label_00F2;
                        }
                        if (str6 == "like%")
                        {
                            goto Label_0107;
                        }
                        goto Label_011C;
                    }
                    str = str + string.Format("{0} like '%'+@{0}{1} and ", str5, j);
                    continue;
                Label_00F2:
                    str = str + string.Format("{0} like '%'+@{0}{1}+'%'  and ", str5, j);
                    continue;
                Label_0107:
                    str = str + string.Format("{0} like @{0}{1}+'%'  and ", str5, j);
                    continue;
                Label_011C:
                    str = str + string.Format("{0} {1} @{0}{2}  and ", str5, str4, j);
                }
            }
            str = str.Trim();
            if (str.EndsWith(" and"))
            {
                str = str.Substring(0, str.Length - 4);
            }
            return str;
        }



    }
}
