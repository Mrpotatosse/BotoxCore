using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Extensions
{
    public static class ByteArrayExtension
    {
        public static string ToHexString(this byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", " ");
        }
    }
}
