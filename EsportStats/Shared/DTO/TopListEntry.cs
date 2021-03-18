﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EsportStats.Shared.DTO
{
    public class TopListEntry
    {
        public SteamFriend Friend { get; set; }
        public string HeroThumbnail { get; set; }
        public int MatchId { get; set; }
        public int Value{ get; set; }
    }
}
