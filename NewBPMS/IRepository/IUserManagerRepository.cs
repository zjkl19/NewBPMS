using Microsoft.AspNetCore.Identity;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewBPMS.IRepository
{
    public interface IUserManagerRepository
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal User);

        string GetUserId(ClaimsPrincipal User);

        Task<ApplicationUser> FindByIdAsync(string userId);

        Task<bool> IsInRoleAsync(ApplicationUser user, string role);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}
