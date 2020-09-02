using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedModel.Models.Actors
{
    public class PlayerModel : ActorModel
    {
        public string Name { get; set; }
        public short Level { get; set; }
        public bool IsMerchant { get; set; }
        public byte BreedId { get; set; }
        public bool Sex { get; set; }
    }
}
