﻿using AutoMapper;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<ContractViewModel> GetContractIndexlinqVar(string ContractNo = "", string ContractName = "")
        {
            //var contracts = _contractRepository.ContextSet.Include(x => x.ApplicationUser).Select(x => _mapper.Map<ContractViewModel>(x));
            var linqVar = _contractRepository.EntityItems
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
                .Where(p => p.Name.Contains(ContractName ?? "") && (p.No.Contains(ContractNo ?? "")));

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
                           where q.Id == Id
                           select new UserProductValueViewModel
                           {
                               Id = p.Id,
                               Labor = (Labor)p.Labor,
                               Ratio = p.Ratio,
                               Amount = (p.Ratio) * q.Amount,
                               StaffName = r.StaffName,
                               ContractId = q.Id
                           }).OrderBy(p => p.Labor);

            return linqVar;
        }
        /// <summary>
        /// 所有未完成且延期的合同
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractWarningViewModel> GetDelayContract()
        {

            var linqVar = _contractRepository.EntityItems
                .Where(x => (DateTime.Now.Date.Subtract(x.SignedDate).TotalDays >= x.Deadline)
                && x.FinishStatus == (int)FinishStatus.NotFinished)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractWarningViewModel>(p))
                .Take(100);

            return linqVar;
        }

        /// <summary>
        /// 所有未完成且延期预警的合同
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContractWarningViewModel> GetDelayWarningContract()
        {
            int warnDays = 7;
            var linqVar = _contractRepository.EntityItems
                .Where(x => (x.Deadline - DateTime.Now.Date.Subtract(x.SignedDate).TotalDays <= warnDays)
                && (x.Deadline - DateTime.Now.Date.Subtract(x.SignedDate).TotalDays >= 0)
                && x.FinishStatus == (int)FinishStatus.NotFinished)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractWarningViewModel>(p))
                .Take(100);

            return linqVar;
        }
    }
}
