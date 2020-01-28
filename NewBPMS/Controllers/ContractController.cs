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
using X.PagedList;

namespace NewBPMS.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IContractService _contractService;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ContractController(
            IMapper mapper,
             IContractService contractService
           , IContractRepository contractRepository
           , IUserRepository userRepository
            , IUserManagerRepository userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _contractService = contractService;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index(int? page, string ContractNo = "", string ContractName = "")
        {
            var linqVar = _contractService.GetContractIndexlinqVar(ContractNo, ContractName);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOflinqVar = linqVar.OrderByDescending(x => x.No).ToPagedList(pageNumber, 10); // will only contain 25 products max because of the pageSize

            var model = new ContractIndexViewModel
            {
                ContractNo = ContractNo,
                ContractName = ContractName,
                StatusMessage = StatusMessage,
                ContractViewModels = onePageOflinqVar,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitProductValueViewModel model)
        {
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check()
        {
            var model = new ContractCheckViewModel
            {
                StatusMessage = StatusMessage,
                ContractViewModels = _contractRepository.EntityItems.Where(x => x.CheckStatus == (int)CheckStatus.NotChecked)
                .Select(x => _mapper.Map<ContractViewModel>(x)),
            };
            return View(model);
        }

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
            }
            catch (DbUpdateException ex)
            {

                throw;
            }

            return RedirectToAction(nameof(Check));
        }
        [HttpGet]
        public IActionResult Review()
        {
            var model = new ContractReviewViewModel
            {
                StatusMessage = StatusMessage,
                ContractViewModels = _contractRepository.EntityItems
                .Where(x => x.CheckStatus == (int)CheckStatus.Checked && x.ReviewStatus == (int)ReviewStatus.NotReviewed)
                .Select(x => _mapper.Map<ContractViewModel>(x)),
            };
            return View(model);
        }

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContractViewModel model)
        {
            //var cnt = await _mainRepository.QueryByNoAsync(model.No);
            //if (cnt.Count > 0)
            //{
            //    ModelState.AddModelError("No", "已存在合同编号：" + model.No);

            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = _mapper.Map<Contract>(model);
            try
            {
                await _contractRepository.CreateAsync(contract);

                StatusMessage = $"项目\"{contract.Name}\"成功创建";
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditContractViewModel model)
        {

            var contract = _contractRepository.EntityItems.Where(x => x.Id == model.Id).FirstOrDefault();

            try
            {

                var itemToEdit = _mapper.Map<Contract>(model);

                await _contractRepository.EditAsync(itemToEdit);

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
                ContractViewModel = _contractRepository.EntityItems
                    .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))
                    .Where(p => p.Id == Id).FirstOrDefault(),
                UserProductValueViewModels = userProductViewModel,
            };
            return View(model);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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