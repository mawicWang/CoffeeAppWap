using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace PublicResource
{
    /// <summary>
    /// 网站通用分页类
    /// </summary>
    public abstract class PagerHelper
    {
        /// <summary>
        /// 获取Page参数，当前页
        /// </summary>
        /// <returns></returns>
        public static int CurrentPage
        {
            get
            {
                return Math.Max(PublicResource.Tools.FormatInt(HttpContext.Current.Request["page"]), 1);
            }
        }

        static Regex regFirstPage = new Regex(@"(/p1(/?)$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 生成分页的HTML代码
        /// </summary>
        /// <param name="nTotal">总条数</param>
        /// <param name="nCurrentPage">当前页</param>
        /// <param name="nMaxShowPage">最多展示多少页</param>
        /// <param name="nPageSize">每页数</param>
        /// <param name="sUrlBase">地址</param>
        /// <returns></returns>
        public static string GetPager(int nTotal, int nCurrentPage, int nMaxShowPage, int nPageSize, string sUrlBase)
        {
            if (nTotal <= 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            if (nCurrentPage > 1)
            {
                sb.Append("<a href=\"" + string.Format(sUrlBase, nCurrentPage - 1) + "\" class=\"page_able\" rel=\"nofollow\">«上一页</a>");
            }
            else
            {
                sb.Append("<span class=\"page_off\">«</span>");
            }
            int nPages = 0;
            if (nTotal % nPageSize > 0)
                nPages = (nTotal / nPageSize) + 1;
            else
                nPages = (nTotal / nPageSize);
            if (nMaxShowPage > 0)
            {
                nPages = Math.Min(nPages, nMaxShowPage);
            }

            if (nCurrentPage > nPages)
                nCurrentPage = nPages;
            if (nCurrentPage < 1)
                nCurrentPage = 1;

            if (nCurrentPage > 5)
            {
                sb.Append(LoopStr(1, 2, nCurrentPage, sUrlBase));
                sb.Append("<span class=\"page_on\">...</span>");
                if (nPages > nCurrentPage + 3)
                {
                    sb.Append(LoopStr(nCurrentPage - 2, nCurrentPage + 2, nCurrentPage, sUrlBase));
                    sb.Append("<span class=\"page_on\">...</span>");
                    sb.Append(LoopStr(nPages - 1, nPages, nCurrentPage, sUrlBase));
                }
                else
                {
                    sb.Append(LoopStr(nCurrentPage - 2, nPages, nCurrentPage, sUrlBase));
                }
            }
            else
            {
                if (nPages < nCurrentPage + 5)
                {
                    sb.Append(LoopStr(1, nPages, nCurrentPage, sUrlBase));
                }
                else
                {
                    sb.Append(LoopStr(1, nCurrentPage + 2, nCurrentPage, sUrlBase));
                    sb.Append("<span class=\"page_on\">...</span>");
                    sb.Append(LoopStr(nPages - 1, nPages, nCurrentPage, sUrlBase));
                }
            }

            if (nCurrentPage < nPages)
            {
                sb.Append(" <a href=\"" + string.Format(sUrlBase, nCurrentPage + 1) + "\" class=\"page_able\" rel=\"nofollow\">下一页»</a>");
            }
            else
            {
                sb.Append("<span class=\"page_off\">»</span>");
            }
            return sb.ToString();
        }

        public static string GetPager(int nTotal, int nCurrentPage, int nPageSize, string sUrlBase)
        {
            return GetPager(nTotal, nCurrentPage, 0, nPageSize, sUrlBase);
        }


        //生成HTML代码用
        private static StringBuilder LoopStr(int nMin, int nMax, int nCurrentPage, string sUrlBase)
        {
            StringBuilder sb = new StringBuilder();
            string sUrl = string.Empty;
            for (int i = nMin; i <= nMax; i++)
            {
                if (i == nCurrentPage)
                    sb.Append(string.Format("<strong class=\"page_on\">{0}</strong>", i));
                else
                {
                    sUrl = string.Format(sUrlBase, i);
                    if (i == 1) //万恶的seo？？
                    {
                        sUrl = regFirstPage.Replace(sUrl, "$2");
                    }
                    sb.Append(string.Format("<a href=\"{0}\" class=\"page_able\"{1}>{2}</a>", sUrl, i > 1 ? " rel=\"nofollow\"" : string.Empty, i));
                }
            }
            return sb;
        }
    }
}