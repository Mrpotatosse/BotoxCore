using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Configurations.Customs
{
    public class StartupConfiguration : IConfiguration
    {
        public string LOCATION => "./startup.json";

        public string dofus_location { get; set; }
        public string dll_location { get; set; }

        public int default_proxy_port { get; set; }

        public bool show_log { get; set; }
        public bool save_log { get; set; }

        public StartupConfiguration()
        {
            dofus_location = "D:/DofusApp/Dofus.exe";
            dll_location = "./SocketHook.dll";

            default_proxy_port = 666;

            show_log = true;
            save_log = false;
        }
    }
}
