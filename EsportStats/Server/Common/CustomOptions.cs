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

        /// <summary>
        /// App Id of the Dota 2 application within Steams database
        /// </summary>
        public int AppId { get; set; }
    }

    public class OpenDotaOptions
    {
        public const string OpenDota = "OpenDota";

        /// <summary>
        /// Api Key for the OpenDota Api.
        /// </summary>
        public string Key { get; set; }
    }
}
