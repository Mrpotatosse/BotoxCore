using BotoxCore.Protocol;
using BotoxSharedModel.Models.Looks;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Extensions
{
    public static class EntityLookExtension
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static Bitmap FromWeb(this ProtocolJsonContent content)
        {
            byte[] data = Encoding.ASCII.GetBytes(content.EntityLookToString());
            string data_str = data.ToHexString(false).ToLower();
            string url = $"https://static.ankama.com/dofus/renderer/look/{data_str}/full/1/150_220-10.png";

            using (WebClient client = new WebClient())
            {
                client.Headers.Set(HttpRequestHeader.Host, "static.ankama.com");
                client.Headers.Set(HttpRequestHeader.Accept, "image/webp,image/apng,image/*,*/*;q=0.8");

                try
                {
                    Stream stream = client.OpenRead(new Uri(url));
                    return new Bitmap(stream);
                }
                catch
                {
                    return null;
                }
            }            
        }

        public static string EntityLookToString(this ProtocolJsonContent content)
        {
            string bonesId = $"{content["bonesId"]}";
            string skins = "";
            for (int i = 0; i < content["skins"].Length; i++)
            {
                skins += $"{content["skins"][i]}{(i < content["skins"].Length - 1 ? "," : "")}";
            }

            string indexedColors;
            try
            {
                indexedColors = $"1={content["indexedColors"][0]},2={content["indexedColors"][1]},3={content["indexedColors"][2]},4={content["indexedColors"][3]},5={content["indexedColors"][4]}";
            }
            catch
            {
                indexedColors = "";
            }
            string scales = "";

            for(int i = 0; i < content["scales"].Length; i++)
            {
                scales += $"{content["scales"][i]}{(i < content["scales"].Length - 1 ? "," : "")}";
            }

            string result = "{" + bonesId;

            if (skins != "")
                result += $"|{skins}";
            if (indexedColors != "")
                result += $"|{indexedColors}";
            if (scales != "")
                result += $"|{scales}";

            return result + "}";
        }
    }
}
