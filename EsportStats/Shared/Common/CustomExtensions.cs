using System;
using System.Collections.Generic;
using System.Text;

namespace EsportStats.Shared.Common
{
    public static class UlongExtension
    {
        public static int ToSteam32(this ulong value)
        {
            // You can get a Steam32 Id by substracting this number from the 64 bit Id
            ulong c = 76561197960265728;
            return Convert.ToInt32(value - c);
        }
    }
}
