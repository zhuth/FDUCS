using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;

namespace Fudantong.Infrastructure
{
    /// <summary>
    /// 常用的操作系统工作，类型转换，字符串处理。
    /// </summary>
    class Common
    {


        /// <summary>
        /// 安全截取字符串
        /// </summary>
        /// <param name="p">字符串</param>
        /// <param name="offset">起始字符下标</param>
        /// <param name="length">长度</param>
        /// <returns>返回字符串</returns>
        public static string SafeSubstring(string p, int offset, int length)
        {
            p = "" + p;
            if (length < 0) length = p.Length;
            if (offset >= p.Length) return "";
            if (length + offset >= p.Length) return p.Substring(offset);
            return p.Substring(offset, length);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="p">原字符串</param>
        /// <param name="start">起始点下标</param>
        /// <param name="end">终止点下标</param>
        /// <returns>截取后的字符串</returns>
        public static string SafeSubstring2(string p, int start, int end)
        {
            return SafeSubstring(p, start, end - start + 1);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static string SafeSubstring2(string p, string p1, string p2)
        {
            if (p2 == "") return SafeSubstring2(p, p.IndexOf(p1)+1, p.Length);
            return SafeSubstring2(p, p.IndexOf(p1) + 1, p.IndexOf(p2) - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static int SafeCompare(string str1, string str2, bool ignoreCase)
        {
            str1 = (str1 == null) ? "" : str1;
            str2 = (str2 == null) ? "" : str2;
            return string.Compare(str1, str2, ignoreCase);
        }

        /// <summary>
        /// 安全地比较两个字符串，null 作为空字符串处理
        /// </summary>
        /// <param name="strobj">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <param name="ignoreCase">是否忽略大小写，true则不区分大小写</param>
        /// <returns></returns>
        public static int SafeCompare(object strobj, string str2, bool ignoreCase)
        {
            string str1 = (strobj == null) ? "" : strobj.ToString();
            return string.Compare(str1, str2, ignoreCase);
        }

        /// <summary>
        /// 判断字符串str1中是否包含了字符串str2
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>判断结果</returns>
        public static bool SafeContain(string str1, string str2, bool ignoreCase)
        {
            if (ignoreCase) { str1 = str1.ToLower(); str2 = str2.ToLower(); }
            return str1.Contains(str2);
        }

        /// <summary>
        /// 安全地对字符串作到布尔值的转换
        /// </summary>
        /// <param name="p">源字符串，非True均视作False</param>
        /// <returns>返回值</returns>
        public static bool SafeBoolParse(string p)
        {
            bool tmp = false;
            if (bool.TryParse(p, out tmp)) return tmp;
            return false;
        }

        /// <summary>
        /// 安全地将字符串转换为整数
        /// </summary>
        /// <param name="p">源字符串</param>
        /// <returns>若出错则返回-1</returns>
        public static int SafeIntParse(string p)
        {
            int tmp = -1;
            if (int.TryParse(p, out tmp)) return tmp;
            return -1;
        }

        /// <summary>
        /// 返回一个 HttpWebRequest 对象。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="cc"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static HttpWebRequest Create(string url, string method, CookieContainer cc, WebProxy proxy)
        {
            HttpWebRequest hwr;
            hwr = (HttpWebRequest)HttpWebRequest.Create(url);
            hwr.CookieContainer = cc;
            hwr.Method = method.ToUpper();
            if (method.ToUpper() == "POST")
                hwr.ContentType = "application/x-www-form-urlencoded";
            hwr.Accept = "application/x-shockwave-flash, image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            hwr.Headers.Add("Accept-Language", "zh-cn");
            hwr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1;)";
            hwr.KeepAlive = false;
            //hwr.Referer = hwr.RequestUri.ToString();
            hwr.Proxy = proxy;
            return hwr;
        }
    }
}
