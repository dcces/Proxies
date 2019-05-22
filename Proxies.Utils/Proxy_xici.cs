using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class Proxy_xici : IProxy
    {
        public string GetHtml()
        {
            return HttpHelper.Get("https://www.xicidaili.com/nn/2");
        }

        public List<ProxyModel> Parse(string data)
        {
            List<ProxyModel> list = new List<ProxyModel>();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(data);
            var ip_list = document.DocumentNode.SelectNodes("//*[@id=\"ip_list\"]");
            var datas = ip_list.Nodes().ToList().Where(a => a.Name == "tr").ToList();
            datas.RemoveAt(0);
            foreach (var item in datas)
            {
                var itemDatas = item.ChildNodes.Nodes().ToList();
                string ip;
                int port;
                string address;
                string state;
                string type;
                string timeout;
                if (itemDatas.Count == 16)
                {
                    ip = itemDatas[1].InnerHtml;//0
                    int.TryParse(itemDatas[2].InnerHtml, out port);//1
                    address = itemDatas[4].InnerHtml;//
                    state = itemDatas[6].InnerHtml;//3
                    type = itemDatas[7].InnerHtml;//4
                    timeout = itemDatas[9].Attributes.Where(a => a.Name == "title").ToList().SingleOrDefault().Value;//
                }
                else
                {
                    ip = itemDatas[0].InnerHtml;//0
                    int.TryParse(itemDatas[1].InnerHtml, out port);
                    //address = "";//
                    state = itemDatas[3].InnerHtml;//3
                    type = itemDatas[4].InnerHtml;//4
                    timeout = itemDatas[9].Attributes.Where(a => a.Name == "title").ToList().SingleOrDefault().Value;//
                }
                ProxyModel model = new ProxyModel();
                model.Address = address;
                model.Ip = ip;
                model.Port = port;
                model.State = state;
                model.TimeOut = timeout;
                model.Type = type;
                list.Add(model);
            }
            return list;
        }
    }
}
