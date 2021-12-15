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
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace NewBPMS.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ContractController : Controller
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IContractService _contractService;
        private readonly IContractRepository _contractRepository;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ContractController(
            IMapper mapper,
             IContractService contractService
           , IContractRepository contractRepository
           , IUserContractRepository userContractRepository
           , IUserRepository userRepository
            , IUserManagerRepository userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _contractService = contractService;
            _contractRepository = contractRepository;
            _userContractRepository = userContractRepository;
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult VerifyContractNo([Bind(include: "No")]CreateContractViewModel model)
        {
            string message = null;
            var result = IsUnique(model.No);
            if (!result)
            {
                message = "已存在合同编号：" + model.No;
                return Json(message);
            }
            else
            {
                return Json(true);
            }

        }

        public IActionResult VerifyContractNoForEdit([Bind(include: "Id,No")]EditContractViewModel model)
        {
            string message = null;
            var result = IsUnique(model.Id, model.No);
            if (!result)
            {
                message = "已存在合同编号：" + model.No;
                return Json(message);
            }
            else
            {
                return Json(true);
            }

        }
        public bool IsUnique(string No)
        {
            var cnt = _contractRepository.QueryEntity<Contract>(x => x.No == No);
            if (cnt.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsUnique(Guid Id, string No)
        {
            var cnt = _contractRepository.QueryEntity<Contract>(x => x.No == No && x.Id != Id);
            if (cnt.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<IActionResult> Index(int? page, bool OnlyMe, string ContractNo = "", string ContractName = "")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            var linqVar = _contractService.GetContractIndexlinqVar(ContractNo, ContractName);
            if (OnlyMe == true)
            {
                linqVar = linqVar.Where(x => x.UserId == user.Id);
            }
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            //var onePageOflinqVar =await linqVar.OrderByDescending(x => x.No).ToPagedList(pageNumber, 10).ToListAsync(); // will only contain 25 products max because of the pageSize
            IPagedList<ContractViewModel> onePageOflinqVar = linqVar.OrderByDescending(x => x.No).ToPagedList(pageNumber, 30);
            var model = new ContractIndexViewModel
            {
                ContractNo = ContractNo,
                ContractName = ContractName,
                StatusMessage = StatusMessage,
                ContractViewModels = onePageOflinqVar,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Restore(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var p = await _contractRepository.QueryByIdAsync(Id);

            p.SubmitStatus = (int)SubmitStatus.NotSubmitted;
            p.CheckStatus = (int)CheckStatus.NotChecked;
            p.ReviewStatus = (int)ReviewStatus.NotReviewed;
            p.FinishStatus = (int)FinishStatus.NotFinished;

            try
            {
                await _contractRepository.EditAsync(p);
            }
            catch (DbUpdateException ex)
            {

                throw;
            }

            return RedirectToAction(nameof(Details), new { Id });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitProductValueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var p = await _contractRepository.QueryByIdAsync(model.ContractId);

            p.SubmitStatus = (int)SubmitStatus.Submitted;
            p.FinishDateTime = model.FinishDateTime;

            try
            {
                await _contractRepository.EditAsync(p);
            }
            catch (DbUpdateException ex)
            {

                throw;
            }

            return RedirectToAction(nameof(Details), new { Id = model.ContractId });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Warning()
        {
            var model = new ContractWarningIndexViewModel
            {
                StatusMessage = StatusMessage,
                ContractDelayViewModels = _contractService.GetDelayContract().Take(100),
                ContractDelayWarningViewModels = _contractService.GetDelayWarningContract().Take(100),
            };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager,ContractChecker")]
        [HttpGet]
        public IActionResult Check()
        {
            var model = new ContractCheckViewModel
            {
                StatusMessage = StatusMessage,

                ContractViewModels = _contractRepository.EntityItems
                .Where(x => x.CheckStatus == (int)CheckStatus.NotChecked && x.SubmitStatus == (int)SubmitStatus.Submitted)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))

            };
            return View(model);
        }

        [Authorize(Roles = "ContractChecker")]
        [HttpGet]
        public async Task<IActionResult> CheckConfirm(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var p = await _contractRepository.QueryByIdAsync(Id);

            p.CheckStatus = (int)CheckStatus.Checked;
            p.CheckDateTime = DateTime.Now;
            p.CheckUserName = user.StaffName;

            try
            {
                await _contractRepository.EditAsync(p);
                StatusMessage = $"已校核\"{p.Name}\"";
            }
            catch (DbUpdateException ex)
            {

                throw;
            }

            return RedirectToAction(nameof(Check));
        }

        [Authorize(Roles = "ContractReviewer")]
        [HttpGet]
        public IActionResult Review()
        {
            var model = new ContractReviewViewModel
            {
                StatusMessage = StatusMessage,
                DetailsContractViewModels = _contractRepository.EntityItems
                .Where(x => x.SubmitStatus == (int)SubmitStatus.Submitted
                && x.CheckStatus == (int)CheckStatus.Checked
                && x.ReviewStatus == (int)ReviewStatus.NotReviewed)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) =>
                new DetailsContractViewModel    //Ignore StatusMessage
                {
                    ContractViewModel = new ContractViewModel { Id = p.Id, Name = p.Name, No = p.No, UserName = p.UserName },
                    UserProductValueViewModels = _contractService.GetUserProductValue(p.Id)

                })

            };

            //var userProductViewModel = _contractService.GetUserProductValue(Id);

            //var model = new DetailsContractViewModel
            //{
            //    StatusMessage = StatusMessage,
            //    ContractViewModel = _contractRepository.EntityItems
            //        .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
            //        .Where(p => p.Id == Id).FirstOrDefault(),
            //    UserProductValueViewModels = userProductViewModel,
            //};
            return View(model);
        }

        [Authorize(Roles = "PowerManager,ContractReviewer")]
        [HttpGet]
        public async Task<IActionResult> ReviewConfirm(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var p = await _contractRepository.QueryByIdAsync(Id);

            p.ReviewStatus = (int)ReviewStatus.Reviewed;
            p.ReviewDateTime = DateTime.Now;
            p.ReviewUserName = user.StaffName;

            p.FinishStatus = (int)FinishStatus.Finished;

            try
            {
                await _contractRepository.EditAsync(p);
                StatusMessage = $"已审核\"{p.Name}\"";
            }
            catch (DbUpdateException ex)
            {
                throw;
            }


            return RedirectToAction(nameof(Check));
        }

        [HttpGet]
        public async Task<IActionResult> RestoreSubmit(Guid Id, string RetAction = nameof(Check))
        {
            var p = await _contractRepository.QueryByIdAsync(Id);

            p.SubmitStatus = (int)SubmitStatus.NotSubmitted;
            p.CheckStatus = (int)CheckStatus.NotChecked;
            p.ReviewStatus = (int)ReviewStatus.NotReviewed;

            try
            {
                await _contractRepository.EditAsync(p);
            }
            catch (DbUpdateException ex)
            {

                throw;
            }


            return RedirectToAction(RetAction);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateContractViewModel
            {
                SignedDate = DateTime.Now,
            };
            return View(model);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateContractViewModel model)
        {
            //var cnt = await _mainRepository.QueryByNoAsync(model.No);
            //if (cnt.Count > 0)
            //{
            //    ModelState.AddModelError("No", "已存在合同编号：" + model.No);

            //}

            if (!IsUnique(model.No))
            {
                ModelState.AddModelError("No", "已存在合同编号：" + model.No);

            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = _mapper.Map<Contract>(model);
            try
            {
                await _contractRepository.CreateAsync(contract);

                StatusMessage = $"合同\"{contract.Name}\"成功创建";
            }
            catch (DbUpdateException ex)
            {

                // Log the error(uncomment ex variable name and write a log.
                ModelState.AddModelError("", "无法保存更改。 " +
                 "请重试, 如果该问题仍然存在 " +
                 "请联系系统管理员。");

                StatusMessage = $"合同/项目创建失败";
                throw;
                //return View(new CreateCMProjectViewModel());
            }

            return RedirectToAction(nameof(ContractController.Index));
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {

            //查询项目负责人
            var item = _contractRepository.EntityItems.Where(x => x.Id == Id).FirstOrDefault();

            var itemToEdit = _mapper.Map<EditContractViewModel>(item);


            //TODO:AutoMapper重构
            //var userContractToEdit = _mapper.Map<EditUserContractViewModel>(userContract);

            return View(itemToEdit);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditContractViewModel model)
        {
            //AsNoTracking
            if (!IsUnique(model.Id, model.No))
            {
                ModelState.AddModelError("No", "已存在合同编号：" + model.No);

            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.Id).FirstOrDefault();
            
            contract.Name = model.Name;
            contract.No = model.No;
            contract.JobContent = model.JobContent;
            contract.Amount = model.Amount;
            contract.SignedDate = model.SignedDate;
            contract.Deadline = model.Deadline;
            contract.FinishDateTime = model.FinishDateTime;
            contract.AcceptUserId = model.AcceptUserId;
            contract.UserId = model.UserId;
            contract.UserName = model.UserName;

            try
            {

                await _contractRepository.EditAsync(contract);

                //StatusMessage = $"成功编辑\"参与人员\"{model.StaffName}";
                StatusMessage = $"成功编辑\"合同\"";

            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 合同详情，包括产值分配信息
        /// </summary>
        /// <param name="Id">合同Id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var userProductViewModel = _contractService.GetUserProductValue(Id);

            var model = new DetailsContractViewModel
            {
                StatusMessage = StatusMessage,

                PdtPercent=_userContractRepository.EntityItems.Where(x=>x.ContractId==Id).Sum(x=>x.Ratio)*100,
                ContractViewModel = _contractRepository.EntityItems
                    .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
                    .Where(p => p.Id == Id).FirstOrDefault(),
                UserProductValueViewModels = userProductViewModel,
            };
            return View(model);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var model = _contractRepository.EntityItems
                .Where(p => p.Id == Id)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<DeleteContractViewModel>(p))
                    .FirstOrDefault();

            return View(model);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            //var linqVar = await _CMProjectRepository.QueryByIdAsync(Id);

            try
            {
                var varDeleted = await _contractRepository.DeleteAsync(Id);
                if (varDeleted != null)
                {
                    StatusMessage = $"合同：\"{varDeleted.Name}\"已成功删除";
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}