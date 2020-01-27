using AutoMapper;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using NewBPMS.ViewModels.UserContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewBPMS.ControllerServices
{
    public class UserContractService : IUserContractService
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

        public IEnumerable<SummaryUserProductValueViewModel> GetSummaryUserProductValue(SummaryUserProductValueQueryViewModel queryModel)
        {
            string staffNoString = queryModel.StaffNoString;

            string[] staffNoArray = staffNoString.Split(',');

            var staffNoList = staffNoArray.ToList();

            var staffNoIntList = staffNoList.Select<string, int>(q => Convert.ToInt32(q));


            //            var query = from c in dt.AsEnumerable()
            //                        group c by new { id = c.Field<int>("ID") }
            //into s
            //                        select new
            //                        {
            //                            ID = s.Key.id,
            //                            Math = s.Sum(p => p.Field<decimal>("Math")),
            //                            Chinese = s.Sum(p => p.Field<decimal>("Chinese"))
            //                        };

            var connTable = (from p in _userContractRepository.EntityItems
                             join q in _contractRepository.EntityItems on p.ContractId equals q.Id
                             join r in _userRepository.EntityItems on p.UserId equals r.Id
                             where q.FinishDateTime >= queryModel.StartDateTime
                             && q.FinishDateTime <= queryModel.EndDateTime
                             && q.FinishStatus==(int)FinishStatus.Finished
                             select new
                             {
                                 r.StaffName,
                                 r.StaffNo,
                                 q.FinishDateTime,
                                 Amount = p.Ratio * q.Amount
                             });
            var query = from p in connTable
                        group p by new { p.StaffName, p.StaffNo }
                        into s
                        select new SummaryUserProductValueViewModel
                        {
                            StaffName = s.Key.StaffName,
                            StaffNo = s.Key.StaffNo,
                            Amount = s.Sum(p => p.Amount)
                        };

            //var linqVar = (from p in _userContractRepository.EntityItems
            //               join q in _contractRepository.EntityItems on p.ContractId equals q.Id
            //               join r in _userRepository.EntityItems on p.UserId equals r.Id
            //               //TODO:合同完成时间
            //               select new SummaryUserProductValueViewModel
            //               {
            //                   StaffName = r.StaffName,
            //                   StaffNo = r.StaffNo,
            //                   Amount = p.Ratio * q.Amount
            //               });

            if (queryModel.SummaryPdtValueOrderType == SummaryPdtValueOrderType.PdtValue)
            {
                query = query.OrderByDescending(x => x.Amount);
            }

            return query;
        }
    }
}
