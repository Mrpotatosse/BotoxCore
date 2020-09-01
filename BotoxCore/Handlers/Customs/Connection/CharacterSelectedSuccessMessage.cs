using BotoxCore.Hooks;
using BotoxCore.Proxy;
using BotoxCore.UI;
using BotoxCore.Extensions;
using BotoxDofusProtocol.Protocol;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BotoxUI.Views.Container;
using BotoxUI.Views.Button;

namespace BotoxCore.Handlers.Customs.Connection
{
    [Handler(ProtocolId = 153)]
    public class CharacterSelectedSuccessMessage : MessageHandler
    {
        protected override Logger logger => LogManager.GetCurrentClassLogger();

        public CharacterSelectedSuccessMessage(CustomClient local, CustomClient remote, ProtocolJsonContent content)
            : base(local, remote, content)
        {

        }

        public override void Handle()
        {
            Hooker<MessageInformation> hook = HookManager<MessageInformation>.Instance[LocalClient.localIp.Port];

            hook.Player = Content.FromBaseInformations();

            CustomClientPage page = UIManager.Instance[hook.Proxy.ProcessId];
            page?.Dispatcher.Invoke(() =>
            {
                page.CharacterInformation.FromModel(hook.Player);
                if (UIManager.Instance.UI.SelectedId == hook.Proxy.ProcessId)
                {
                    if (UIManager.Instance.UI.Container[hook.Proxy.ProcessId] is CustomButton btn)
                    {
                        btn.ButtonText = $"{hook.Player.Name}";
                        UIManager.Instance.UI.Container[hook.Proxy.ProcessId, true] = btn;
                    }
                    UIManager.Instance.UI.Navigate(page, hook.Proxy.ProcessId);
                }
            });
        }

        public override void Error(Exception e)
        {
            logger.Error(e);
        }
    }
}
