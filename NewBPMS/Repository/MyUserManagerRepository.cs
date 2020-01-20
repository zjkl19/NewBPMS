using Microsoft.AspNetCore.Identity;
using NewBPMS.IRepository;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class MyUserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MyUserManagerRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal User)
        {
            return await _userManager.GetUserAsync(User);
        }

        public string GetUserId(ClaimsPrincipal User)
        {
            return _userManager.GetUserId(User);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return (await _userManager.IsInRoleAsync(user, role));
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return (await _userManager.AddToRoleAsync(user, role));
        }

    }
}
