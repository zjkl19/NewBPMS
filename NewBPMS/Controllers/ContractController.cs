using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using X.PagedList;

namespace NewBPMS.Controllers
{
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

        public IActionResult Index(int? page, string ContractName = "")
        {
            var linqVar = _contractService.GetContractIndexlinqVar(ContractName);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOflinqVar = linqVar.OrderByDescending(x => x.No).ToPagedList(pageNumber, 10); // will only contain 25 products max because of the pageSize

            var model = new ContractIndexViewModel
            {
                ContractName = ContractName,
                StatusMessage = StatusMessage,
                ContractViewModels = onePageOflinqVar,
            };
            return View(model);
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
    }
}