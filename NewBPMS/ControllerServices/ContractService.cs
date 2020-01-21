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
        private readonly IUserContractRepository _userContractRepository;
        private readonly IMapper _mapper;

        public ContractService(
            IMapper mapper,
            IContractRepository contractRepository
            , IUserRepository userRepository
            , IUserContractRepository userContractRepository)
        {
            _mapper = mapper;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _userContractRepository = userContractRepository;
        }

        public IEnumerable<ContractViewModel> GetContractIndexlinqVar(string ContractName = "")
        {

            var linqVar = _contractRepository.EntityItems
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
                .Where(p => p.Name.Contains(ContractName ?? ""));

            return linqVar;
        }

        public IEnumerable<UserProductValueViewModel> GetUserProductValue(Guid Id)
        {
            //var linqVar = _userContractRepository.EntityItems
            //    .Join(_contractRepository.EntityItems, p => p.ContractId, q => q.Id, (p, q) => _mapper.Map<UserProductValueViewModel>(p))
            //    .Where(p => p.ContractId==Id).OrderBy(p=>p.Labor);

            //TODO：用lambda表达式及AutoMapper重构
            var linqVar = (from p in _userContractRepository.EntityItems
                          join q in _contractRepository.EntityItems on p.ContractId equals q.Id
                          join r in _userRepository.EntityItems on p.UserId equals r.Id
                          where q.Id==Id
                          select new UserProductValueViewModel
                          {
                              Labor=(Labor)p.Labor,
                              Ratio=p.Ratio,
                              Amount=(p.Ratio)*q.Amount,
                              StaffName=r.StaffName,
                          }).OrderBy(p=>p.Labor);

            return linqVar;
        }
    }
}
