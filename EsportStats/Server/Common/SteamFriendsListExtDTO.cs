using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    // DTO based on: https://partner.steamgames.com/doc/webapi/ISteamUser#GetFriendList

    public class SteamFriendsListExtDTO
    {
        [JsonProperty("friendslist")]
        public SteamFriendsExtDTO FriendsList { get; set; }        
    }

    public class SteamFriendsExtDTO
    {

        [JsonProperty("friends")]
        public IEnumerable<SteamFriendExtDTO> Friends { get; set; }
    }

    public class SteamFriendExtDTO
    {
        [JsonProperty("steamid")]
        public ulong SteamId { get; set; }
        
        //... other served but unnecessary properties not implemented
    }
}
