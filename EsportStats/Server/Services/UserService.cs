using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetUserAsync(ulong steamId);

        public Task<IEnumerable<ApplicationUser>> GetUsersAsync(IEnumerable<ulong> steamIds);
    }


    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISteamService _steamService;

        public UserService(IUnitOfWork unitOfWork, ISteamService steamService)
        {
            _unitOfWork = unitOfWork;
            _steamService = steamService;
        }

        public async Task<ApplicationUser> GetUserAsync(ulong steamId)
        {
            var user = await _unitOfWork.Users.GetUserBySteamIdAsync(steamId);

            // if the user profile is older than 24 hours
            if (user.Timestamp < DateTime.Now.AddHours(-24))
            {
                var extProfile = await _steamService.GetSteamProfileExternalAsync(steamId);
                user.UpdateFromExternalProfile(extProfile);
                _unitOfWork.SaveChanges();
            }

            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync(IEnumerable<ulong> steamIds)
        {
            var users = await _unitOfWork.Users.GetUsersBySteamIdAsync(steamIds);

            foreach(var user in users)
            {
                // if the user profile is older than 24 hours
                if (user.Timestamp < DateTime.Now.AddHours(-24))
                {
                    var extProfile = await _steamService.GetSteamProfileExternalAsync(user.SteamId);
                    user.UpdateFromExternalProfile(extProfile);                    
                }
                _unitOfWork.SaveChanges();
            }

            return users;
        }
    }
}
