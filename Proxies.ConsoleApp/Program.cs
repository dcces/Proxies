using HtmlAgilityPack;
using Proxies.Utils;
using Proxies.Utils.Proxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Proxies.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = typeof(ProxyService).GetTypeInfo().Assembly.GetName().Name;


            ServicePointManager.DefaultConnectionLimit = 200;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProxyService proxyService = new ProxyService();
            var models = proxyService.GetListByRemote();
            Console.WriteLine("远程获取IP总数:" + models.Count);
            var newmodels = proxyService.GetLiveData(models);
            stopwatch.Stop();
            Console.WriteLine("总数:" + newmodels.Count + "   耗时:" + stopwatch.Elapsed);
            foreach (var item in newmodels)
            {
                Console.WriteLine(item.ToString());
            }
            Console.Read();


            #region 延迟测试
            //Proxy_89ip p = new Utils.Proxy_89ip();
            //var ls = p.Parse(p.GetHtml());
            //ls.ForEach(item =>
            //{
            //    Task.Run(() =>
            //    {
            //        Console.WriteLine(HttpHelper.ProxyCheck(ref item)); ;
            //    });
            //});

            #endregion
            Console.Read();

        }


    }
}
