using BotoxUI.Views.Button;
using BotoxUI.Views.Container;
using BotoxUI.Views.Default;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace BotoxUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(Default);
        }

        public int SelectedId { get; private set; } = -1;

        public void Navigate(Page page, int id)
        {
            if (!MainFrame.Navigate(page))
            {
                MessageBox.Show("navigation was canceled", "Navigation canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SelectedId = id;
            }
        }

        public CustomContainer Container => MainContainer;
        public CustomButton Launch => LaunchButton;
        public Frame Frame => MainFrame;

        public static readonly DefaultPage Default = new DefaultPage();
    }
}
