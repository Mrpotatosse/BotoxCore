using BotoxSharedModel.Models.Looks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedModel.Models.Actors
{
    public abstract class ActorModel
    {
        public double Id { get; set; }
        public double MapId { get; set; }
        public EntityLook Look { get; set; }
    }
}
