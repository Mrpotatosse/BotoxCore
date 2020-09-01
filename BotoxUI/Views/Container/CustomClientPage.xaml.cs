using BotoxUI.Views.Container.Character;
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
    /// Interaction logic for CustomClientPage.xaml
    /// </summary>
    public partial class CustomClientPage : Page
    {
        public CustomClientPage()
        {
            InitializeComponent();
        }

        public CustomCharacterInformation CharacterInformation => MainCharacterInformation;
    }
}
