﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class ProxyModel
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 匿名 透明
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// HTTPS or HTTP
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 延迟
        /// </summary>

        public string TimeOut { get; set; }

        public bool Usable { get; set; } = false;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            var type = this.GetType();
            foreach (var property in type.GetProperties())
            {
                sb.AppendLine($"{property.Name}={property.GetValue(this)}");
            }
            sb.AppendLine("---------------------------------------------");
            return sb.ToString();
        }

    }
}
