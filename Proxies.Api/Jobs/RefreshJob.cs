using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Proxies.Utils;
using Proxies.Utils.Proxy;
using System.Threading;

namespace Proxies.Api.Jobs
{
    public class RefreshJob : Job
    {
        public static ConcurrentBag<ProxyModel> Cache = new ConcurrentBag<ProxyModel>();
        public static int count = 0;
        [Invoke(Begin = "2019-5-23 14:00:00", Interval = 1000 * 60 * 10)]
        public bool lockObject = false;
        public void Collect(ProxyService proxyService)
        {
            if (!lockObject)
            {
                lockObject = true;
                var RemoteData = proxyService.GetListByRemote();
                Cache = proxyService.GetLiveData(RemoteData);
                lockObject = false;
            }

        }
    }
}
