using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class HttpHelper
    {
        public static string SpeedUrl = "http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js";
        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
        }

        public static async Task<string> GetAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";
            using (var response = await request.GetResponseAsync())
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                return await reader.ReadToEndAsync();
            }
        }

        //[DllImport("ZT_ID.dll", CallingConvention = CallingConvention,en)]
        //public static void aa([MarshalAs(UnmanagedType.VBByRefStr)]ref string CardInfo)
        //{

        //}
        public static bool ProxyCheck(ref ProxyModel model)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SpeedUrl);
                var proxy = new WebProxy(model.Ip, model.Port);
                request.Timeout = 3000;
                request.Proxy = proxy;
                request.KeepAlive = false;
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    stopwatch.Stop();
                    model.TimeOut = stopwatch.ElapsedMilliseconds + "毫秒";
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<ProxyModel> ProxyCheckAsync(ProxyModel model)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SpeedUrl);
                var proxy = new WebProxy(model.Ip, model.Port);
                request.Timeout = 3000;
                request.Proxy = proxy;
                request.KeepAlive = false;
                using (var response = await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                {
                    stopwatch.Stop();
                    model.TimeOut = stopwatch.ElapsedMilliseconds + "毫秒";
                    return model;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
