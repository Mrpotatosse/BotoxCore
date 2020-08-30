using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BotoxDofusProtocol.Protocol
{
    public class BotofuProtocolManager
    {
        public static readonly string PROTOCOL_URL = "https://cldine.gitlab.io/-/protocol-autoparser/-/jobs/691246963/artifacts/protocol.json";
        public static readonly string PROTOCOL_JSON_LOCATION = "./protocol.json";

        public static bool FoundProtocol
        {
            get
            {
                return File.Exists(PROTOCOL_JSON_LOCATION);
            }
        }
        public static BotofuProtocol Protocol
        {
            get
            { 
                return JsonConvert.DeserializeObject<BotofuProtocol>(File.ReadAllText(PROTOCOL_JSON_LOCATION), new JsonSerializerSettings() { Formatting = Formatting.Indented }); 
            }
        }

        public static bool Download()
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(PROTOCOL_URL, PROTOCOL_JSON_LOCATION);
            }

            return FoundProtocol;
        }
    }
}
