using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class HttpHelper
    {
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
