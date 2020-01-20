using NewBPMS.Data;
using NewBPMS.IRepository;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class RoleRepository : GenericRepository<ApplicationRole>, IRoleRepository
    {
        //private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IRoleManager roleManager;

        public RoleRepository(ApplicationDbContext _context, IRoleManager _roleManager) : base(_context)
        {
            context = _context;
            roleManager = _roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await roleManager.RoleExistsAsync(roleName);
        }

    }
}
