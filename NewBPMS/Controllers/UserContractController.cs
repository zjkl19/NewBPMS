using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels.UserContractViewModels;

namespace NewBPMS.Controllers
{
    public class UserContractController : Controller
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IUserContractService _userContractService;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public UserContractController(
            IUserManagerRepository userManager
             , IUserContractRepository userContractRepository
             , IUserContractService userContractService
             , IContractRepository contractRepository
            , IMapper mapper)
        {
            _userManager = userManager;
            _userContractRepository = userContractRepository;
            _userContractService = userContractService;
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public IActionResult SummaryUserProductValue([Bind(Prefix = "SummaryUserProductValueQueryViewModel")]SummaryUserProductValueQueryViewModel queryModel)
        {
            var startDateTime = queryModel.StartDateTime;
            var endDateTime = queryModel.EndDateTime;

            var l = _userContractService.GetSummaryUserProductValue(queryModel).ToList();

            return PartialView("_SummaryProductValueList", l);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserContractViewModel model)
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.ContractId).FirstOrDefault();

            if (user.Id != contract.UserId)
            {
                return PartialView("/Views/Account/AccessDenied.cshtml");    //?View
            }

            try
            {

                var userContractToCreate = _mapper.Map<UserContract>(model);

                await _userContractRepository.CreateAsync(userContractToCreate);

                StatusMessage = $"成功创建\"参与人员\"{model.StaffName}";


            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return RedirectToAction("Details","Contract",new {Id=model.ContractId });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult List(ListViewModel model)
        //{
        //    var startDateTime = model.StartDateTime;
        //    var endDateTime = model.EndDateTime;

        //    var listUserScoreViewModels = new List<ListUserScoreViewModel>();

        //    //积分查询人数最大不超过maxListUsers
        //    for (int i = 0; i < (model.ItemChosen.Count <= maxListUsers ? model.ItemChosen.Count : maxListUsers); i++)
        //    {
        //        if (model.ItemChosen[i] != "false")
        //        {
        //            listUserScoreViewModels.Add(new ListUserScoreViewModel
        //            {
        //                StaffNo = _userRepository.EntityItems.Where(x => x.Id == model.ItemChosen[i]).FirstOrDefault().StaffNo,
        //                StaffName = _userRepository.EntityItems.Where(x => x.Id == model.ItemChosen[i]).FirstOrDefault().StaffName,
        //                Score = _scoreRecordRepository.EntityItems.Where(x => x.UserId == model.ItemChosen[i] && x.ScoreTime >= startDateTime && x.ScoreTime <= endDateTime).Sum(x => x.Score),
        //            });
        //        }
        //    }

        //    return PartialView("ListUserScorePartial", listUserScoreViewModels);
        //}
    }
}