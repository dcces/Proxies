using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.Utils
{
    public class LogManager
    {
        public string RootPath = "Log";
        private static string PARTITION = "\\";
        private readonly static object objHelper = new object();
        static LogManager()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                PARTITION = "/";
            }
        }
        public void Info(string msg)
        {
            Write(msg);
        }
        public void Write(string msg)
        {
            lock (objHelper)
            {
                var file = getFileName();
                //File.AppendAllLines(file, new List<string>() { msg }, Encoding.UTF8);
                File.AppendAllText(file, msg, Encoding.UTF8);
            }
        }

        public string getFileName()
        {

            var y = DateTime.Now.ToString("yyyy");
            var m = DateTime.Now.ToString("MM");
            var d = DateTime.Now.ToString("dd");
            var path = $"{RootPath}{PARTITION}{y}{PARTITION}{m}{PARTITION}{d}.log";
            DirectoryInfo dd = new DirectoryInfo(path);
            if (!Directory.Exists(dd.Parent.FullName))
            {
                Directory.CreateDirectory(dd.Parent.FullName);
            }

            return path;
        }
    }
}
