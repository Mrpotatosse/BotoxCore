using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxDofusProtocol.IO
{
    public class Int64 
    {
        public long Low { get; set; }

        public long High { get; set; }

        public Int64() { }

        public Int64(long low, long high)
        {
            Low = low;
            High = high;
        }

        public static Int64 FromNumber(long @long)
        {
            return new Int64(@long, (long)Math.Floor(@long / 4294967296.0));
        }

        public long ToNumber()
        {
            return High * 4294967296 + Low;
        }
    }
}
