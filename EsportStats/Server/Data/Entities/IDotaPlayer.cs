using EsportStats.Server.Common;
using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    public interface IDotaPlayer
    {
        void UpdateFromExternalProfile(SteamProfileExtDTO dto);
        SteamUserDTO ToDTO(bool isCurrentUser);
        void SetPlaytime(int minutes);
    }
}
