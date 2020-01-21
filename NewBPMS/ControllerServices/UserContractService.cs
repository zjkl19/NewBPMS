using AutoMapper;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.UserContractViewModels;
using System.Collections.Generic;
using System.Linq;

namespace NewBPMS.ControllerServices
{
    public class UserContractService: IUserContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IMapper _mapper;

        public UserContractService(
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

        public IEnumerable<SummaryUserProductValueViewModel> GetSummaryUserProductValue()
        {
            //
            var linqVar = from p in _userContractRepository.EntityItems
                          join q in _contractRepository.EntityItems on p.ContractId equals q.Id
                          join r in _userRepository.EntityItems on p.UserId equals r.Id
                          select new SummaryUserProductValueViewModel
                          {
                              StaffName=r.StaffName,
                              StaffNo=r.StaffNo,
                              Amount=p.Ratio*q.Amount
                          };

            return linqVar;
        }
    }
}
