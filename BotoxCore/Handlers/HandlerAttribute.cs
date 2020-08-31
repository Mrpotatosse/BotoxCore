using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerAttribute : Attribute
    {
        public int ProtocolId { get; set; } = 0;
        public string ProtocolName { get; set; } = null;
    }
}
