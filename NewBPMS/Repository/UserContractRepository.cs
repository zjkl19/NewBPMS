using NewBPMS.Data;
using NewBPMS.IRepository;
using NewBPMS.Models;

namespace NewBPMS.Repository
{
    public class UserContractRepository : GenericRepository<UserContract>, IUserContractRepository
    {
        public UserContractRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
    }

}
