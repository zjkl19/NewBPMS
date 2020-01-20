using NewBPMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class UserRepository : GenericRepository<Models.ApplicationUser>, IRepository.IUserRepository
    {
        public UserRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
