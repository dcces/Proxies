using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public interface IProxy
    {
        string GetHtml();
        List<ProxyModel> Parse(string data);

    }
}
