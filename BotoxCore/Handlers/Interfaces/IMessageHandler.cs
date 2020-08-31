using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers.Interfaces
{
    public interface IMessageHandler
    {
        void Handle();
        void EndHandle();
        void Error(Exception e);
    }
}
