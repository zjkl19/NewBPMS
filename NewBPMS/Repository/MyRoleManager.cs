using Microsoft.AspNetCore.Identity;
using NewBPMS.IRepository;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class MyRoleManager : IRoleManager
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public MyRoleManager(RoleManager<ApplicationRole> _roleManager)
        {
            roleManager = _roleManager;
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            return await roleManager.CreateAsync(role);
        }
    }
}
