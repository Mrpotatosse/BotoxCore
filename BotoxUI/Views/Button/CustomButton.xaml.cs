using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BotoxUI.Views.Button
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {                
        public event Action<CustomButton> OnClick;

        #region ButtonText
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", 
                                                                                                   typeof(string), 
                                                                                                   typeof(CustomButton),
                                                                                                   new PropertyMetadata("", 
                                                                                                       new PropertyChangedCallback(OnButtonTextChanged)));

        public string ButtonText
        {
            get
            {
                return (string)GetValue(ButtonTextProperty);
            }
            set
            {
                SetValue(ButtonTextProperty, value);
            }
        }

        private static void OnButtonTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomButton btn = obj as CustomButton;
            btn.OnButtonTextChanged(arg);
        }

        private void OnButtonTextChanged(DependencyPropertyChangedEventArgs arg)
        {
            ButtonTxt.Text = arg.NewValue.ToString();
        }
        #endregion

        #region BackgroundColor
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", 
                                                                                                        typeof(SolidColorBrush), 
                                                                                                        typeof(CustomButton), 
                                                                                                        new PropertyMetadata(
                                                                                                            new SolidColorBrush(), 
                                                                                                            new PropertyChangedCallback(OnBackgroundColorChanged)));

        private Color? _baseBackgroundColor { get; set; } = null;
        private ColorAnimation _backgroundColorAnimation { get; set; } = null;
        
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return (SolidColorBrush)GetValue(BackgroundColorProperty);
            }
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        private static void OnBackgroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomButton btn = obj as CustomButton;
            btn.OnBackgroundColorChange(arg);
        }

        private void OnBackgroundColorChange(DependencyPropertyChangedEventArgs arg)
        {
            if(arg.NewValue is SolidColorBrush brush)
            {
                if(_baseBackgroundColor is null)
                {
                    _baseBackgroundColor = brush.Color;
                }

                if(MainBorder.Background is SolidColorBrush _brush)
                    _backgroundColorAnimation = new ColorAnimation(_brush.Color, brush.Color, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);
                else
                    _backgroundColorAnimation = new ColorAnimation(brush.Color, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);
                
                MainBorder.Background = new SolidColorBrush();
                MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, _backgroundColorAnimation);
            }
        }
        #endregion

        #region IsSelected
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", 
                                                                                                   typeof(bool), 
                                                                                                   typeof(CustomButton), 
                                                                                                   new PropertyMetadata(false, 
                                                                                                       new PropertyChangedCallback(OnIsSelectedChanged)));

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        private static void OnIsSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomButton btn = obj as CustomButton;
            btn.OnIsSelectedChanged(arg);
        }

        private void OnIsSelectedChanged(DependencyPropertyChangedEventArgs arg)
        {
            if(arg.NewValue is bool value)
            {
                SelectionPanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region SelectionColor
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor",
                                                                                                        typeof(SolidColorBrush),
                                                                                                        typeof(CustomButton),
                                                                                                        new PropertyMetadata(
                                                                                                            new SolidColorBrush(),
                                                                                                            new PropertyChangedCallback(OnSelectionColorChanged)));

        private ColorAnimation _selectionColorAnimation { get; set; } = null;
        public SolidColorBrush SelectionColor
        {
            get
            {
                return (SolidColorBrush)GetValue(SelectionColorProperty);
            }
            set
            {
                SetValue(SelectionColorProperty, value);
            }
        }

        private static void OnSelectionColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomButton btn = obj as CustomButton;
            btn.OnSelectionColorChange(arg);
        }

        private void OnSelectionColorChange(DependencyPropertyChangedEventArgs arg)
        {
            if (arg.NewValue is SolidColorBrush brush)
            {
                if (SelectionBorder.Background is SolidColorBrush _brush)
                    _selectionColorAnimation = new ColorAnimation(_brush.Color, brush.Color, new Duration(TimeSpan.FromMilliseconds(150)), FillBehavior.HoldEnd);
                else
                    _selectionColorAnimation = new ColorAnimation(brush.Color, new Duration(TimeSpan.FromMilliseconds(150)), FillBehavior.HoldEnd);

                SelectionBorder.Background = new SolidColorBrush();
                SelectionBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, _selectionColorAnimation);

                SelectionPanel.Visibility = IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region BackgroundEnterColor
        public static readonly DependencyProperty BackgroundEnterColorProperty = DependencyProperty.Register("BackgroundEnterColor", 
                                                                                                             typeof(SolidColorBrush), 
                                                                                                             typeof(CustomButton), 
                                                                                                             new PropertyMetadata(
                                                                                                                 new SolidColorBrush(Color.FromRgb(18, 82, 180)),
                                                                                                                 new PropertyChangedCallback(OnBackgroundEnterColorChanged)));

        public SolidColorBrush BackgroundEnterColor
        {
            get
            {
                return (SolidColorBrush)GetValue(BackgroundEnterColorProperty);
            }
            set
            {
                SetValue(BackgroundEnterColorProperty, value);
            }
        }

        private static void OnBackgroundEnterColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            CustomButton btn = obj as CustomButton;
            btn.OnBackgroundEnterColorChanged(arg);
        }

        private void OnBackgroundEnterColorChanged(DependencyPropertyChangedEventArgs arg)
        {
            if(arg.NewValue is SolidColorBrush brush)
            {

            }
        }
        #endregion

        public CustomButton()
        {
            InitializeComponent();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BackgroundColor = BackgroundEnterColor;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BackgroundColor = new SolidColorBrush(_baseBackgroundColor.HasValue ? _baseBackgroundColor.Value : throw new Exception("_baseBackgroundColor cannot be null"));
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color color = BackgroundColor.Color;
            Color transparent_color = Color.FromArgb(100, color.R, color.G, color.B);

            BackgroundColor = new SolidColorBrush(transparent_color);

            if(OnClick != null)
                Dispatcher.Invoke(OnClick, this);

            Task.Delay(300).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (BackgroundColor.Color == transparent_color)
                    {
                        BackgroundColor = new SolidColorBrush(color);
                    }
                });
            });
        }
    }
}
