using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.IRepository
{
    public interface IRoleRepository : IBasicCRUDRepository<Models.ApplicationRole>
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}
