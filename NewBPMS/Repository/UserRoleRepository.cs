using NewBPMS.Data;
using NewBPMS.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class UserRoleRepository : GenericRepository<Models.ApplicationUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
