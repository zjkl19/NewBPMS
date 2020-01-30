using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels;
using NewBPMS.ViewModels.ContractViewModels;
using NewBPMS.ViewModels.UserContractViewModels;

namespace NewBPMS.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserContractController : Controller
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IUserContractService _userContractService;
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public UserContractController(
            IUserManagerRepository userManager
             , IUserContractRepository userContractRepository
             , IUserContractService userContractService
             , IUserRepository userRepository
             , IContractRepository contractRepository
            , IMapper mapper)
        {
            _userManager = userManager;
            _userContractRepository = userContractRepository;
            _userContractService = userContractService;
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        private const int maxListUsers = 1000;    //产值详情最大查询记录数量

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            var model = new UserContractIndexViewModel
            {
                SummaryUserProductValueQueryViewModel = new SummaryUserProductValueQueryViewModel
                {
                    StaffNoString =
                     string.Join(",", _userRepository.EntityItems.Select(x => new { x.StaffNo }).OrderBy(x => x.StaffNo)
                     .ToList().Select(u => u.StaffNo.ToString(CultureInfo.InvariantCulture)).ToList<string>())
                }
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult SummaryUserProductValue([Bind(Prefix = "SummaryUserProductValueQueryViewModel")]SummaryUserProductValueQueryViewModel queryModel)
        {
            var startDateTime = queryModel.StartDateTime;
            var endDateTime = queryModel.EndDateTime;

            var l = _userContractService.GetSummaryUserProductValue(queryModel).ToList();

            return PartialView("_SummaryProductValueList", l);
        }

        [HttpGet]
        public IActionResult ListOwn()
        {
            var model = new ListViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListOwn([Bind(include: "StartDateTime,EndDateTime")]ListViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var startDateTime = model.StartDateTime;
            var endDateTime = model.EndDateTime;

            var listViewModels = _userContractRepository.EntityItems
                    .Where(p => p.UserId == user.Id)
                    .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => (p, q))
                    .Join(_contractRepository.EntityItems, s => s.p.ContractId, r => r.Id, (s, r) => (s, r))
                    .Where(x => x.r.FinishStatus == (int)FinishStatus.Finished
                    && x.r.FinishDateTime>=startDateTime && x.r.FinishDateTime<=endDateTime)
                    .Select(x => _mapper.Map<UserProductValueDetailsViewModel>(x.s.p)).ToList();


            return PartialView("_ListUserProductValueDetailsPartial", listViewModels);
        }

        [Authorize(Roles = "PowerManager,Admin")]
        [HttpGet]
        public IActionResult List()
        {
            var userSelectListViewModel = _userRepository.EntityItems.Select(x => new UserSelectViewModel
            {
                Id = x.Id,
                No = x.StaffNo,
                Name = x.StaffName,
            }).OrderBy(x => x.No).ToList();

            var model = new ListViewModel
            {
                UserSelectListViewModel = userSelectListViewModel,
            };
            return View(model);
        }

        [Authorize(Roles = "PowerManager,Admin")]
        [HttpPost]
        public IActionResult List(ListViewModel model)
        {
            var startDateTime = model.StartDateTime;
            var endDateTime = model.EndDateTime;

            var listViewModels = new List<UserProductValueDetailsViewModel>();

            //        ContractViewModel = _contractRepository.EntityItems
            //.Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
            //.Where(p => p.Id == Id).FirstOrDefault(),

            //积分查询人数最大不超过maxListUsers
            for (int i = 0; i < (model.ItemChosen.Count <= maxListUsers ? model.ItemChosen.Count : maxListUsers); i++)
            {
                if (model.ItemChosen[i] != "false")
                {
                    //方法1
                    //List<UserProductValueDetailsViewModel> k = (from p in _userContractRepository.EntityItems
                    //         join q in _userRepository.EntityItems
                    //         on p.UserId equals q.Id
                    //         join r in _contractRepository.EntityItems
                    //         on p.ContractId equals r.Id
                    //         where p.UserId==model.ItemChosen[i]
                    //         select new UserProductValueDetailsViewModel
                    //         {
                    //             Labor = (Labor)p.Labor,
                    //             Ratio = p.Ratio,
                    //             StaffNo =q.StaffNo,
                    //             StaffName=q.StaffName,
                    //             ContractNo=r.No,
                    //             ContractName=r.Name,
                    //             Amount=(p.Ratio)*r.Amount
                    //         }).ToList();

                    //方法2
                    //var k = _userContractRepository.EntityItems
                    //    .Where(p=>p.UserId == model.ItemChosen[i])
                    //    .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => (p, q))
                    //    .Join(_contractRepository.EntityItems, s => s.p.ContractId, r => r.Id, (s, r) => 
                    //    new UserProductValueDetailsViewModel
                    //    {
                    //        Labor = (Labor)s.p.Labor,
                    //        Ratio = s.p.Ratio,
                    //        StaffNo = s.q.StaffNo,
                    //        StaffName = s.q.StaffName,
                    //        ContractNo = r.No,
                    //        ContractName = r.Name,
                    //        Amount = (s.p.Ratio) * r.Amount
                    //    }).ToList();

                    //方法3
                    var k = _userContractRepository.EntityItems
                            .Where(p => p.UserId == model.ItemChosen[i])
                            .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => (p, q))
                            .Join(_contractRepository.EntityItems, s => s.p.ContractId, r => r.Id, (s, r) => (s, r))
                            .Where(x => x.r.FinishStatus == (int)FinishStatus.Finished
                    && x.r.FinishDateTime >= startDateTime && x.r.FinishDateTime <= endDateTime)
                            .Select(x => _mapper.Map<UserProductValueDetailsViewModel>(x.s.p));


                    listViewModels = listViewModels.Union(k).ToList();
                }
            }


            return PartialView("_ListUserProductValueDetailsPartial", listViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);

            //查询项目负责人
            var userContract = _userContractRepository.EntityItems.Where(x => x.Id == Id).FirstOrDefault();

            var userContractToEdit = (from p in _userContractRepository.EntityItems
                                      join q in _userRepository.EntityItems
                                      on p.UserId equals q.Id
                                      where p.Id == Id
                                      select new EditUserContractViewModel
                                      {
                                          Id = userContract.Id,
                                          Labor = (Labor)userContract.Labor,
                                          Ratio = userContract.Ratio,
                                          ContractId = userContract.ContractId,
                                          UserId = userContract.UserId,
                                          StaffName = q.StaffName
                                      }).FirstOrDefault();

            //if (user.Id != contract.UserId)
            //{
            //    return PartialView("/Views/Account/AccessDenied.cshtml");    //?View
            //}

            //TODO:AutoMapper重构
            //var userContractToEdit = _mapper.Map<EditUserContractViewModel>(userContract);

            return PartialView("Edit", userContractToEdit);
        }

        public IActionResult IsTotalRatioNotGreaterThenOneForCreate([Bind(include: "ContractId,Ratio")]CreateUserContractViewModel model)
        {
            string message = null;
            var result = IsTotalRatioNotGreaterThenOne(model.ContractId, model.Ratio);
            if (result)
            {
                return Json(true);

            }
            else
            {
                message = $"产值分配比例之和不能大于1";
                return Json(message);
            }

        }

        public IActionResult IsTotalRatioNotGreaterThenOneForEdit([Bind(include: "Id,ContractId,Ratio")]EditUserContractViewModel model)
        {
            string message = null;
            var result = IsTotalRatioNotGreaterThenOne(model.Id, model.ContractId, model.Ratio);
            if (result)
            {
                return Json(true);

            }
            else
            {
                message = $"产值分配比例之和不能大于1";
                return Json(message);
            }

        }

        public bool IsTotalRatioNotGreaterThenOne(Guid ContractId, decimal ratio)
        {

            decimal itemRatio = _userContractRepository.EntityItems.Where(p => p.ContractId == ContractId).Sum(p => p.Ratio);

            decimal totalRatio = itemRatio + ratio;

            if (totalRatio > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //重载
        public bool IsTotalRatioNotGreaterThenOne(Guid Id, Guid ContractId, decimal ratio)
        {

            var itemEdit = _userContractRepository.EntityItems.Where(p => p.Id == Id).FirstOrDefault();

            decimal itemRatio = _userContractRepository.EntityItems
                .Where(p => p.ContractId == ContractId && p.Id != Id).Sum(p => p.Ratio);

            decimal totalRatio = itemRatio + ratio;    //正在编辑的比例要扣掉

            if (totalRatio > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserContractViewModel model)
        {
            if (!IsTotalRatioNotGreaterThenOne(model.Id, model.ContractId, model.Ratio))
            {
                ModelState.AddModelError("Ratio", "产值总比例之和不能大于1");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.ContractId).FirstOrDefault();

            if (user.Id != contract.UserId)
            {
                return View("AccessDenied");    //?View
            }

            try
            {

                //var userContractToEdit = _mapper.Map<UserContract>(model);
                var userContractToEdit = _userContractRepository.EntityItems.Where(x => x.Id == model.Id).FirstOrDefault();
                userContractToEdit.Labor = (int)model.Labor;
                userContractToEdit.Ratio = model.Ratio;

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
            return PartialView("Create", new CreateUserContractViewModel { ContractId = ContractId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserContractViewModel model)
        {
            if (!IsTotalRatioNotGreaterThenOne(model.ContractId, model.Ratio))
            {
                ModelState.AddModelError("Ratio", "产值总比例之和不能大于1");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.ContractId).FirstOrDefault();

            if (user.Id != contract.UserId)
            {
                return View("AccessDenied");    //?View
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

            return RedirectToAction("Details", "Contract", new { Id = model.ContractId });
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

            return RedirectToAction("Details", "Contract", new { Id = varDeleted.ContractId });

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