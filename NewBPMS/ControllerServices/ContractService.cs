using AutoMapper;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ControllerServices
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ContractService(
            IMapper mapper,
            IContractRepository contractRepository
            , IUserRepository userRepository)
        {
            _mapper = mapper;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<ContractViewModel> GetContractIndexlinqVar(string ContractName = "")
        {
            //方法4
            var linqVar = _contractRepository.EntityItems
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
                .Where(p => p.Name.Contains(ContractName ?? ""));

            return linqVar;
        }
    }
}
