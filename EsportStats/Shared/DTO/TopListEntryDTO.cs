using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsportStats.Shared.DTO
{
    public class TopListEntryDTO
    {
        public SteamUserDTO Friend { get; set; }
        public Hero Hero { get; set; }
        public ulong? MatchId { get; set; }
        public int Value{ get; set; }
    }
}
