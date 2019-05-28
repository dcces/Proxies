using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class Proxy_kuai : IProxy
    {
        public string GetHtml()
        {
            //return HttpHelper.Get("https://www.kuaidaili.com/free/inha/2/");
            string str = HttpHelper.Get("https://www.kuaidaili.com/free/inha/1/");
            Thread.Sleep(2000);
            str += HttpHelper.Get("https://www.kuaidaili.com/free/inha/2/");
            Thread.Sleep(2300);
            str += HttpHelper.Get("https://www.kuaidaili.com/free/inha/3/");
            //透明
            Thread.Sleep(2050);
            str += HttpHelper.Get("https://www.kuaidaili.com/free/intr/");
            Thread.Sleep(2400);
            str += HttpHelper.Get("https://www.kuaidaili.com/free/intr/2/");
            Thread.Sleep(2000);
            str += HttpHelper.Get("https://www.kuaidaili.com/free/intr/3/");
            return str;

        }

        public List<ProxyModel> Parse(string data)
        {
            List<ProxyModel> ls = new List<ProxyModel>();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(data);
            var tbodys = document.DocumentNode.SelectNodes("//*[@id=\"list\"]/table/tbody").ToList();
            foreach (var tbody in tbodys)
            {
                var tr = tbody.ChildNodes.Where(a => a.Name == "tr").ToList();
                foreach (var item in tr)
                {
                    ProxyModel model = new ProxyModel();
                    var tds = item.ChildNodes.Where(a => a.Name == "td").ToList();
                    var ip = tds[0].InnerHtml;
                    int.TryParse(tds[1].InnerHtml, out int port);
                    var state = tds[2].InnerHtml;
                    var type = tds[3].InnerHtml;
                    var address = tds[4].InnerHtml;
                    var timeout = tds[5].InnerHtml;
                    model.Ip = ip;
                    model.Port = port;
                    model.State = state;
                    model.Type = type;
                    model.Address = address;
                    model.TimeOut = timeout;
                    ls.Add(model);
                }
            }
            return ls;
        }
    }
}
