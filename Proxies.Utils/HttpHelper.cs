using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            try
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
            catch (WebException)
            {
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
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

        public static async Task<ProxyModel> ProxyCheckAsync(ProxyModel model)
        {
            //try
            //{
            //    Console.WriteLine($"开始测试 {model.Ip}:{model.Port}");
            //    Stopwatch stopwatch = new Stopwatch();
            //    stopwatch.Start();
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SpeedUrl);
            //    var proxy = new WebProxy(model.Ip, model.Port);
            //    request.Timeout = 3000;
            //    request.Proxy = proxy;
            //    request.KeepAlive = false;
            //    using (var response = request.GetResponse())
            //    using (var responseStream = response.GetResponseStream())
            //    {
            //        stopwatch.Stop();
            //        model.TimeOut = stopwatch.ElapsedMilliseconds + "毫秒";
            //        Console.WriteLine($"结束测试 {model.Ip}:{model.Port}");
            //        return true;
            //    }
            //}
            //catch (Exception e)
            //{
            //    LogManager log = new LogManager();
            //    log.Info($"{model.Ip}:{model.Port}:{e}");
            //    Console.WriteLine($"{model.Ip}:{model.Port}:{e.Message}");
            //    return false;
            //}
            try
            {
                Console.WriteLine($"开始测试 {model.Ip}:{model.Port}");
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseProxy = true;

                HttpClient client = new HttpClient(handler, false);
                client.Timeout = TimeSpan.FromSeconds(5);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var proxy = new WebProxy(model.Ip, model.Port);
                proxy.Credentials = CredentialCache.DefaultCredentials;

                handler.Proxy = proxy;
                handler.PreAuthenticate = true;
                handler.UseDefaultCredentials = false;
                var res = await client.GetAsync(SpeedUrl);

                stopwatch.Stop();
                model.TimeOut = stopwatch.ElapsedMilliseconds + "毫秒";
                Console.WriteLine($"结束测试 {model.Ip}:{model.Port}");
                model.Usable = true;
                return model;

            }
            catch (Exception e)
            {
                LogManager log = new LogManager();
                log.Info($"{model.Ip}:{model.Port}:{e}");
                Console.WriteLine($"{model.Ip}:{model.Port}:{e.Message}");
                return model;
            }
        }


        public static async Task<ProxyModel> ProxyCheckAsync1(ProxyModel model)
        {
            try
            {

                ServicePointManager.DefaultConnectionLimit = 200;
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
    public class HttpClientSingle
    {
        private HttpClient client;
        public HttpClient getInstance()
        {
            if (client == null)
            {
                client = new HttpClient();
            }
            return client;
        }
    }
}
