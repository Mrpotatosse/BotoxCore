using BotoxUI.Extensions;
using BotoxUI.Views.Button;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BotoxUI.Views.Container
{
    /// <summary>
    /// Interaction logic for CustomContainer.xaml
    /// </summary>
    public partial class CustomContainer : UserControl
    {
        #region ContentHeight
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight",
                                                                                                      typeof(int),
                                                                                                      typeof(CustomContainer),
                                                                                                      new PropertyMetadata(1, OnContentHeightChanged));

        public int ContentHeight
        {
            get
            {
                return (int)GetValue(ContentHeightProperty);
            }
            set
            {
                SetValue(ContentHeightProperty, value);
            }
        }

        private static void OnContentHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomContainer container = obj as CustomContainer;
            container.OnContentHeightChanged(arg);
        }

        private void OnContentHeightChanged(DependencyPropertyChangedEventArgs arg)
        {
            if(arg.NewValue is int value)
            {

            }
        }
        #endregion

        public CustomContainer()
        {
            InitializeComponent();

            TopBtn.ButtonTxt.Background = new ImageBrush(Properties.Resources.tx_arrow_up.Source());
            BotBtn.ButtonTxt.Background = new ImageBrush(Properties.Resources.tx_arrow_down.Source());
        }

        private int topId { get; set; } = -1;
        private Dictionary<int, FrameworkElement> content { get; set; } = new Dictionary<int, FrameworkElement>(); 

        public FrameworkElement this[int id, bool erase = false]
        {
            get
            {
                if (content.ContainsKey(id))
                {
                    return content[id];
                }
                return null;
            }
            set
            {
                if(value is null)
                {
                    if (!content.Remove(id))
                    {
                        throw new Exception($"cannot remove , id ({id}) was not found");
                    }

                    if (content.Count == 0)
                        topId = -1;

                    ShowContent();
                    return;
                }

                value.SetValue(HeightProperty, (double)ContentHeight);
                if (content.ContainsKey(id))
                {
                    if (erase) 
                    {                        
                        content[id] = value;
                    }
                    else
                        throw new Exception($"content already contain key {id} ! set erase to true");
                }
                else
                {
                    content.Add(id, value);
                    if (topId == -1)
                        topId = id;
                }

                if(value is CustomButton btn)
                {
                    btn.OnClick += Btn_OnClick;
                }

                ShowContent();
            }
        }

        private void Btn_OnClick(CustomButton obj)
        {
            foreach(CustomButton btn in content.Select(x => x.Value).Cast<CustomButton>())
            {
                if(btn != null)
                {
                    btn.IsSelected = false;
                }
            }
            obj.IsSelected = true;
        }

        public int MaximumContentCount
        {
            get
            {
                return (int)(MainContainer.ActualHeight / ContentHeight);
            }
        }

        private void ShowContent()
        {
            MainContainer.Children.Clear();

            int max = MaximumContentCount > 0 && MaximumContentCount < content.Count ? MaximumContentCount : content.Count;
            int c = 0;
            int currentId = topId;
            while(c < max)
            {
                if (content.ContainsKey(currentId))
                {
                    content[currentId].VerticalAlignment = VerticalAlignment.Top;
                    DockPanel.SetDock(content[currentId], Dock.Top);
                    MainContainer.Children.Add(content[currentId]);
                    currentId = Next(currentId);
                    c++;
                }
                else
                {
                    currentId = content.FirstOrDefault().Key;
                }
            }

            if (content.Count > 0)
            {
                TopBtn.IsEnabled = content.FirstOrDefault().Key != topId;
                BotBtn.IsEnabled = content.LastOrDefault().Key != Next(topId, MaximumContentCount);
            }
        }

        private int Next(int id, int k = 1)
        {
            for(int i = 0; i < content.Count; i++)
            {
                if(content.ElementAt(i).Key == id)
                {
                    if(i + 1 >= content.Count)
                    {
                        return id;
                    }

                    int n_id = content.ElementAt(i + 1).Key;
                    if (k > 1)
                    {
                        return Next(n_id, k - 1);
                    }

                    return n_id;
                }
            }

            return id;
        }

        private int Previous(int id)
        {
            for (int i = 0; i < content.Count; i++)
            {
                if (content.ElementAt(i).Key == id)
                {
                    if (i - 1 < 0)
                        return id;

                    return content.ElementAt(i - 1).Key;
                }
            }

            return id;
        }

        private void TopBtn_OnClick(Button.CustomButton obj)
        {
            topId = Previous(topId);
            ShowContent();
        }

        private void BotBtn_OnClick(Button.CustomButton obj)
        {
            topId = Next(topId);
            ShowContent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShowContent();
        }
    }
}
