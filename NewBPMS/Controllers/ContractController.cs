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
        private readonly IContractService _contractService;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public ContractController(
            IMapper mapper,
             IContractService contractService
           , IContractRepository contractRepository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _contractRepository = contractRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index(int? page, string ContractNo = "",string ContractName = "")
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
            var linqVar = await _contractRepository.QueryByIdAsync(Id);
            var model = _mapper.Map<DeleteContractViewModel>(linqVar);
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