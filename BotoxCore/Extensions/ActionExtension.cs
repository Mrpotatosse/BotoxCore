using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Extensions
{
    public static class ActionExtension
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Run(this Action action, Action onCompleted = null, Action<Exception> onError = null)
        {
            try
            {
                action();
                onCompleted?.Invoke();
            }
            catch(Exception e)
            {
                logger.Error(e);
                onError?.Invoke(e);
            }
        }
    }
}
