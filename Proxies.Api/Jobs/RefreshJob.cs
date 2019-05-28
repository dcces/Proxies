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
        public static int count = 0;
        public bool lockObject = false;
        [Invoke(Interval = 1000 * 60 * 30, Begin = "2019-05-27 17:07")]

        public void Collect(ProxyService proxyService)
        {
            if (!lockObject)
            {
                lockObject = true;
                var RemoteData = proxyService.GetListByRemote();
                Cache = proxyService.GetLiveData(RemoteData);
                File.AppendAllText(Environment.CurrentDirectory + "cache.json", JsonConvert.SerializeObject(Cache));
                lockObject = false;

            }

            File.WriteAllText(Environment.CurrentDirectory + "log.txt", "4\r\n");
        }
    }
}
