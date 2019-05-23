using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
        public List<ProxyModel> GetLiveData(List<ProxyModel> ls)
        {
            List<ProxyModel> LiveDatas = new List<ProxyModel>();
            Task[] tasks = new Task[ls.Count];
            int index = 0;
            ls.ForEach(item =>
           {
               tasks[index] = new Task(() =>
              {
                  if (HttpHelper.ProxyCheck(ref item))
                      LiveDatas.Add(item);
              });
               tasks[index++].Start();

           });
            Task.WaitAll(tasks.ToArray());
            return LiveDatas;
        }

    }
}
