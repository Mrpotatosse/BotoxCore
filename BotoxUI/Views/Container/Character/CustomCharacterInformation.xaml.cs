using BotoxSharedModel.Models.Actors;
using BotoxUI.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace BotoxUI.Views.Container.Character
{
    /// <summary>
    /// Interaction logic for CustomCharacterInformation.xaml
    /// </summary>
    public partial class CustomCharacterInformation : UserControl
    {
        public CustomCharacterInformation()
        {
            InitializeComponent();
        }

        public void FromModel(PlayerModel model, Bitmap image)
        {
            Level = model.Level;
            CharName = model.Name;

            CharacterImageBox.Source = image?.Source();
        }

        private int _level { get; set; } = 0;
        private int Level 
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                NameLvTxt.Text = NameLvStr;
            } 
        }

        private string _name { get; set; } = "";
        private string CharName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NameLvTxt.Text = NameLvStr;
            }
        } 

        private string NameLvStr => $"{CharName} Lv.{Level}";
    }
}
