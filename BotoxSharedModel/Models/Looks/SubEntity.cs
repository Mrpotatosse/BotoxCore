using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedModel.Models.Looks
{
    public class SubEntity
    {
        public byte BindingPointCategory { get; set; }
        public byte BindingPointIndex { get; set; }
        public EntityLook SubEntityLook { get; set; }
    }
}
