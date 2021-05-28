using System;
using System.Collections.Generic;
using System.Text;

namespace EsportStats.Shared.Common
{
    static class Constants
    {
        // You can get a Steam32 Id by substracting this number from the 64 bit Id
        public const ulong c = 76561197960265728;        
    }

    public static class UlongExtension
    {        
        public static int ToSteam32(this ulong value)
        {
            return Convert.ToInt32(value - Constants.c);
        }
    }

    public static class Int32Extension
    {
        public static ulong ToSteam64(this int value)
        {
            return Constants.c + Convert.ToUInt64(value);
        }
    }
}
