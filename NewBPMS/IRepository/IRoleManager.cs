using Microsoft.AspNetCore.Identity;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.IRepository
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityResult> CreateAsync(ApplicationRole role);
    }
}
