using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels.ContractViewModels;
using NewBPMS.ViewModels.UserContractViewModels;

namespace NewBPMS.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);

            //查询项目负责人
            var userContract = _userContractRepository.EntityItems.Where(x => x.Id == Id).FirstOrDefault();

            //if (user.Id != contract.UserId)
            //{
            //    return PartialView("/Views/Account/AccessDenied.cshtml");    //?View
            //}

            //TODO:AutoMapper重构
            //var userContractToEdit = _mapper.Map<EditUserContractViewModel>(userContract);
            var userContractToEdit = new EditUserContractViewModel
            {
                Id=userContract.Id,
                Labor=(Labor)userContract.Labor,
                Ratio=userContract.Ratio,
                ContractId=userContract.ContractId,
                UserId = userContract.UserId,
            };

            return PartialView("Edit", userContractToEdit );
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserContractViewModel model)
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.ContractId).FirstOrDefault();

            if (user.Id != contract.UserId)
            {
                return PartialView("/Views/Account/AccessDenied.cshtml");    //?View
            }

            try
            {

                var userContractToEdit = _mapper.Map<UserContract>(model);

                await _userContractRepository.EditAsync(userContractToEdit);

                //StatusMessage = $"成功编辑\"参与人员\"{model.StaffName}";
                StatusMessage = $"成功编辑\"参与人员\"";

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return RedirectToAction("Details", "Contract", new { Id = model.ContractId });
        }

        [HttpGet]
        public IActionResult Create(Guid ContractId)
        {
            return PartialView("Create", new CreateUserContractViewModel { ContractId = ContractId});
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


        /// <summary>
        /// 删除测点
        /// </summary>
        /// <param name="Id">测点Id</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid Id)
        {
            UserContract varDeleted;
            if (Id == null)
            {
                return NotFound();
            }

            try
            {
                varDeleted = await _userContractRepository.DeleteAsync(Id);
                if (varDeleted != null)
                {
                    //StatusMessage = @$"{varDeleted.ApplicationUser.StaffName}信息已成功删除!";
                    StatusMessage = $"信息已成功删除!";
                }
                else
                {
                    StatusMessage = $"已成功删除!";
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

            return RedirectToAction("Details", "Contract", new { Id =varDeleted.ContractId });

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