using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class SteamOptions
    {

        public const string Steam = "Steam";

        /// <summary>
        /// Api Key for the Steam OpenID authentication and Api.
        /// </summary>
        public string Key { get; set; }
    }
}
