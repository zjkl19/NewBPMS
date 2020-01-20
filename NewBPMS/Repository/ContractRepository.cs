using NewBPMS.Data;
using NewBPMS.IRepository;
using NewBPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
