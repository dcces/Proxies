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
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                if (type.Name.StartsWith("Proxy_"))
                {
                    var obj = Activator.CreateInstance(type) as IProxy;
                    ls.AddRange(obj.Parse(obj.GetHtml()));
                }
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
            ConcurrentBag<ProxyModel> LiveDatas = new ConcurrentBag<ProxyModel>();
            Task[] tasks = new Task[ls.Count];
            int index = 0;
            ls.ForEach(item =>
           {
               //if (index != 0)
               //{
               //    tasks[index++] = Task.Run(() => { });
               //    return;
               //}
               tasks[index++] = Task.Run(() =>
             {
                 LogManager log = new LogManager();
                 try
                 {
                     if (HttpHelper.ProxyCheck(ref item))
                     {
                         LiveDatas.Add(item);
                     }

                 }
                 catch (Exception e)
                 {
                     log.Info($"{e.Message}{Environment.NewLine}{e}");
                 }
             });

           });
            Task.WaitAll(tasks.ToArray());
            return LiveDatas;
        }

    }
}
