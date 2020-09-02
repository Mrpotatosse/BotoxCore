using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedModel.Models.Looks
{
    public class EntityLook
    {
        public short BonesId { get; set; }
        public short[] Skins { get; set; }
        public int[] IndexedColors { get; set; }
        public short[] Scales { get; set; }
        public SubEntity[] SubEntityLook { get; set; }
    }
}
