using BotoxSharedProtocol.Network.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedProtocol.Network
{
    public class NetworkElement
    {
        public ClassField[] fields { get; set; }
        public string name { get; set; }
        public int protocolID { get; set; }
        public string super { get; set; }
        public bool super_serialize { get; set; }
        public string supernamespace { get; set; }
        public bool use_hash_function { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        ~NetworkElement()
        {
            fields = null;
            name = null;
            protocolID = -1;
            super = null;
            super_serialize = false;
            supernamespace = null;
            use_hash_function = false;
        }
    }
}
