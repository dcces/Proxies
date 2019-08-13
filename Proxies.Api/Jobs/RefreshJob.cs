using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Proxies.Utils;
using Proxies.Utils.Proxy;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Proxies.Api.Jobs
{
    public class RefreshJob : Job
    {
        public static ConcurrentBag<ProxyModel> Cache = new ConcurrentBag<ProxyModel>();
        public static DateTime HandleTime;
        public static int TotalCount = 0;
        public static int ValidCount = 0;

        public bool lockObject = false;
        [Invoke(Interval = 1000 * 60 * 10, Begin = "2019-05-27 17:07")]

        public void Collect(ProxyService proxyService)
        {
            LogManager log = new LogManager();
            Stopwatch sw = new Stopwatch();
            if (!lockObject)
            {
                lockObject = true;
                sw.Start();
                log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 开始执行任务{Environment.NewLine}");
                var RemoteData = proxyService.GetListByRemote();

                log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 获取ip总数:{RemoteData.Count}{Environment.NewLine}");
                Cache = proxyService.GetLiveData(RemoteData);
                log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 有效ip总数:{Cache.Count}{Environment.NewLine}");
                File.AppendAllText(Environment.CurrentDirectory + "cache.json", JsonConvert.SerializeObject(Cache));
                HandleTime = DateTime.Now;
                TotalCount = RemoteData.Count;
                ValidCount = Cache.Count;
                sw.Stop();
                log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 总耗时:{sw.Elapsed}{Environment.NewLine}");
                log.Info($"----------------------------------------------------{Environment.NewLine}");
                lockObject = false;

            }

            File.WriteAllText(Environment.CurrentDirectory + "log.txt", "4\r\n");
        }
    }
}
