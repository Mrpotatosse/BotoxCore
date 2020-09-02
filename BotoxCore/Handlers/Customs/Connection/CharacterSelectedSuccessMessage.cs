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
using System.Drawing;
using BotoxCore.Protocol;
using BotoxDofusProtocol.IO;
using BotoxSharedProtocol.Network.Fields;
using System.Diagnostics;
using BotoxUI.Views.Container.Character;
using BotoxSharedModel.Models.Actors;

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

        public async override void Handle()
        {
            Hooker<MessageInformation> hook = HookManager<MessageInformation>.Instance[LocalClient.localIp.Port];

            hook.Player = Content.FromBaseInformations();

            CustomClientPage page = UIManager.Instance[hook.Proxy.ProcessId];

            Test(hook, page);

            page?.Dispatcher.Invoke(() =>
            {
            });
        }

        public async void Test(Hooker<MessageInformation> hook, CustomClientPage page)
        {
            if (page is null) return;

            await Task.Run(() =>
            {
                page.Dispatcher.Invoke(() =>
                {
                    ProtocolJsonContent content = Content["infos"]["entityLook"];
                    Bitmap image = content.FromWeb();
                    SetModel(page.CharacterInformation, hook.Player, image);

                    if (UIManager.Instance.UI.Container[hook.Proxy.ProcessId] is CustomButton btn)
                    {
                        btn.ButtonText = $"{hook.Player.Name}";
                        UIManager.Instance.UI.Container[hook.Proxy.ProcessId, true] = btn;
                    }

                    if (UIManager.Instance.UI.SelectedId == hook.Proxy.ProcessId)
                    {
                        UIManager.Instance.UI.Navigate(page, hook.Proxy.ProcessId);
                    }
                });
            });
        }

        public override void Error(Exception e)
        {
            logger.Error(e);
        }

        private delegate void SetModelHandler(PlayerModel model, Bitmap img);
        private async void SetModel(CustomCharacterInformation ui, PlayerModel model, Bitmap img)
        {
            await ui.Dispatcher.BeginInvoke(new SetModelHandler(ui.FromModel), model, img);
        }
    }
}
