using BotoxCore.Configurations;
using BotoxCore.Extensions;
using BotoxCore.Hooks;
using BotoxDofusProtocol.Protocol;
using BotoxUI;
using BotoxUI.Views.Button;
using BotoxUI.Views.Container;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BotoxCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MainWindow UI { get; private set; }
        private Dictionary<int, CustomClientPage> ClientsPages { get; set; } = new Dictionary<int, CustomClientPage>();
        private bool _isUsed { get; set; } = false;

        private Thread _uiThread { get; set; }

        private delegate void ExecuteActionUI();

        public CustomClientPage this[int processId]
        {
            get
            {
                if (ClientsPages.ContainsKey(processId))
                    return ClientsPages[processId];
                return null;
            }
        }

        public bool Init()
        {
            _isUsed = ConfigurationManager.Instance.Startup.show_ui;

            if (_isUsed)
            {
                _uiThread = CreateUIThread();
                _uiThread.Start();
                logger.Info("ui opened");
            }

            return _isUsed;
        }

        private Thread CreateUIThread()
        {
            Thread result = new Thread(new ThreadStart(CreateUI));

            result.SetApartmentState(ApartmentState.STA);

            return result;
        }

        private void CreateUI()
        {
            Application application = new Application();
            UI = new MainWindow();
            InitEvent();
            application.Run(UI);
        }

        private void InitEvent()
        {
            UI.Launch.OnClick += UI_OnLaunchRequested;
        }

        private async void UI_OnLaunchRequested(CustomButton btn)
        {
            Hooker<MessageInformation> hooker = HookManager<MessageInformation>.Instance.CreateHooker();

            hooker.OnProcessStarted += Hooker_OnProcessStarted;
            hooker.OnProcessExited += Hooker_OnProcessExited;

            await btn.Dispatcher.BeginInvoke(new ExecuteActionUI(hooker.Inject));
        }

        private void Hooker_OnProcessExited(Hooker<MessageInformation> obj)
        {
            SetBtnUI(obj, null);
        }

        private void Hooker_OnProcessStarted(Hooker<MessageInformation> obj)
        {
            CustomButton btn = new CustomButton()
            {
                BackgroundColor = new SolidColorBrush(Color.FromRgb(14, 154, 3)),
                SelectionColor = new SolidColorBrush(Color.FromRgb(213, 8, 8)),
                BackgroundEnterColor = new SolidColorBrush(Color.FromRgb(213, 8, 8)),
                ButtonText = $"Waiting ({obj.Proxy.ProcessId}) ..."
            };

            btn.OnClick += _btn =>
            {
                if(UI.SelectedId != obj.Proxy.ProcessId)
                {                    
                    if (obj.Player is null)
                    {
                        UI.Navigate(MainWindow.Default, obj.Proxy.ProcessId);
                    }
                    else
                    {
                        UI.Navigate(ClientsPages[obj.Proxy.ProcessId], obj.Proxy.ProcessId);
                    }
                }
            };

            SetBtnUI(obj, btn);

            ClientsPages.Add(obj.Proxy.ProcessId, new CustomClientPage());
        }

        private void SetBtnUI(Hooker<MessageInformation> hooker, CustomButton btn)
        {
            UI.Container.Dispatcher.Invoke(() =>
            {
                UI.Container[hooker.Proxy.ProcessId] = btn;
                if(btn is null && UI.SelectedId == hooker.Proxy.ProcessId)
                {
                    UI.Navigate(MainWindow.Default, -1);
                }
            });
        }
    }
}
