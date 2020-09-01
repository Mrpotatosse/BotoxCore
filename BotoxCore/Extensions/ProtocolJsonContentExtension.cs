using BotoxSharedModel.Models.Actors;
using BotoxSharedProtocol.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Extensions
{
    public static class ProtocolJsonContentExtension
    {
        public static ActorModel FromActorRolePlayInformation(this ProtocolJsonContent content)
        {
            if(content["protocol_id"] == 36 || content["protocol_id"] == 129)
            {
                ProtocolJsonContent alignmentInfos = content["alignmentInfos"];
                short level = (short)(alignmentInfos is null ? 0 : (alignmentInfos["characterPower"] - content["contextualId"]));

                return new PlayerModel()
                {
                    Id = content["contextualId"],
                    Level = level,
                    Name = content["name"],
                    IsMerchant = content["protocol_id"] == 129
                };
            }

            return null;
        }

        public static PlayerModel FromBaseInformations(this ProtocolJsonContent content)
        {
            return new PlayerModel()
            {
                Id = content["infos"]["id"],
                Name = content["infos"]["name"],
                Level = content["infos"]["level"],
                IsMerchant = false
            };
        }
    }
}
