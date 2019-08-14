using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxies.Utils.Proxy
{
    public class ProxyService
    {
        public List<ProxyModel> GetListByRemote()
        {
            List<ProxyModel> ls = new List<ProxyModel>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetExportedTypes();
                for (int i = 0; i < types.Length; i++)
                //foreach (var type in types)
                {
                    var type = types[i];
                    if (type.Name.StartsWith("Proxy_"))
                    {
                        var obj = Activator.CreateInstance(type) as IProxy;
                        ls.AddRange(obj.Parse(obj.GetHtml()));
                    }
                }
            }
            catch (Exception e)
            {
                LogManager log = new LogManager();
                log.Info($"获取IP出错:{e.Message}{Environment.NewLine}{e.ToString()}{Environment.NewLine}");
            }
            return ls;
        }
        /// <summary>
        /// 获得能用的ip
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public ConcurrentBag<ProxyModel> GetLiveData(List<ProxyModel> ls)
        {
            //ls = new List<ProxyModel>();
            //ls.Add(new ProxyModel() { Ip = "182.92.233.137", Port = 8118, Type = "HTTP" });
            ConcurrentBag<ProxyModel> LiveDatas = new ConcurrentBag<ProxyModel>();
            Task[] tasks = new Task[ls.Count];
            int index = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                var item = ls[i];
                tasks[index++] = Task.Run(async () =>
                {
                    LogManager log = new LogManager();
                    try
                    {
                        var m = await HttpHelper.ProxyCheckAsync(item);

                        if (m.Usable)
                        {
                            LiveDatas.Add(item);
                        }
                    }
                    catch (Exception e)
                    {
                        log.Info($"{e.Message}{Environment.NewLine}");
                    }
                });
            }
            Task.WaitAll(tasks.ToArray());
            return LiveDatas;
        }

    }
}
