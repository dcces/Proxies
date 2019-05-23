using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class Proxy_89ip : IProxy
    {
        public string GetHtml()
        {
            string res = string.Empty;
            for (int i = 0; i < 20; i++)
            {
                res += HttpHelper.Get($"http://www.89ip.cn/index_{i}.html");
            }
            return res;
        }

        public List<ProxyModel> Parse(string data)
        {
            List<ProxyModel> ls = new List<ProxyModel>();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(data);
            var tbodys = document.DocumentNode.SelectNodes("//table[@class=\"layui-table\"]/tbody").ToList();
            foreach (var tbody in tbodys)
            {
                var tr = tbody.ChildNodes.Where(a => a.Name == "tr").ToList();
                foreach (var item in tr)
                {
                    var tds = item.ChildNodes.Where(a => a.Name == "td").ToList();
                    var ip = tds[0].InnerHtml;
                    var port = tds[1].InnerHtml;
                    var address = tds[2].InnerHtml + tds[3].InnerHtml;
                    ip = Regex.Replace(ip, "[\r\n\t]*", "");
                    port = Regex.Replace(port, "[\r\n\t]*", "");
                    address = Regex.Replace(address, "[\r\n\t ]*", "");
                    int.TryParse(port, out var porti);
                    ProxyModel model = new ProxyModel();
                    model.Ip = ip;
                    model.Port = porti;
                    model.Address = address;
                    ls.Add(model);
                }
            }
            return ls;
        }
    }
}
