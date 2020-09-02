using BotoxSharedModel.Models.Actors;
using BotoxSharedModel.Models.Looks;
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
                ProtocolJsonContent humanoidInfos = content["humanoidInfo"];
                short level = (short)(alignmentInfos is null ? 0 : (alignmentInfos["characterPower"] - content["contextualId"]));

                return new PlayerModel()
                {
                    Id = content["contextualId"],
                    Level = level,
                    Name = content["name"],
                    IsMerchant = content["protocol_id"] == 129,
                    Sex = humanoidInfos["sex"],

                    BreedId = 0,
                    Look = content["look"]
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
                IsMerchant = false,
                Sex = content["infos"]["sex"],

                BreedId = content["infos"]["breed"],
                //Look = content["infos"]["look"].FromEntityLook()
            };
        }

        public static EntityLook FromEntityLook(this ProtocolJsonContent content)
        {
            EntityLook look = new EntityLook()
            {
                BonesId = content["bonesId"],
                Skins = content["skins"],
                IndexedColors = content["indexedColors"],
                Scales = content["scales"],
                SubEntityLook = new SubEntity[content["subEntityLook"].Length]
            };

            for(int i = 0;i < look.SubEntityLook.Length; i++)
            {
                look.SubEntityLook[i] = content["subEntityLook"][i].FromSubEntity();
            }

            return look;
        }

        public static SubEntity FromSubEntity(this ProtocolJsonContent content)
        {
            return new SubEntity()
            {
                BindingPointCategory = content["bindingPointCategory"],
                BindingPointIndex = content["bindingPointIndex"],
                SubEntityLook = content["subEntityLook"].FromEntityLook()
            };
        }
    }
}
