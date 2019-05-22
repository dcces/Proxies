using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = Get("https://www.xicidaili.com/nn/2");
            //File.AppendAllText(@"C:\Users\92999\Desktop\img\1.txt", res);
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(res);

            var ip_list = document.DocumentNode.SelectNodes("//*[@id=\"ip_list\"]");
            var datas = ip_list.Nodes().ToList().Where(a => a.Name == "tr").ToList();
            datas.RemoveAt(0);
            foreach (var item in datas)
            {
                var itemDatas = item.ChildNodes.Nodes().ToList();
                if (itemDatas.Count == 16)
                {
                    var ip = itemDatas[1].InnerHtml;//0
                    var port = itemDatas[2].InnerHtml;//1
                    var address = itemDatas[4].InnerHtml;//
                    var State = itemDatas[6].InnerHtml;//3
                    var Type = itemDatas[7].InnerHtml;//4
                    var timeout = itemDatas[15].Attributes.Where(a => a.Name == "title").ToList().SingleOrDefault();//
                }
                else
                {
                    var ip = itemDatas[0].InnerHtml;//0
                    var port = itemDatas[1].InnerHtml;//1
                    var address = "";//
                    var State = itemDatas[3].InnerHtml;//3
                    var Type = itemDatas[4].InnerHtml;//4
                    var timeout = itemDatas[15].Attributes.Where(a => a.Name == "title").ToList().SingleOrDefault().Value;//

                }
            }
            Console.WriteLine(ip_list.Count);
        }

        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.KeepAlive = false;
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
        }
    }
}
