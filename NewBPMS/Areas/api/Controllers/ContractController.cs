using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;

namespace NewBPMS.Areas.api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
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

        [HttpGet]
        public ActionResult<ContractCheckViewModel> Check()
        {
            var model = new ContractCheckViewModel
            {
                StatusMessage = StatusMessage,

                ContractViewModels = _contractRepository.EntityItems
                .Where(x => x.CheckStatus == (int)CheckStatus.NotChecked && x.SubmitStatus == (int)SubmitStatus.Submitted)
                .Join(_userRepository.EntityItems, p => p.UserId, q => q.Id, (p, q) => _mapper.Map<ContractViewModel>(p))

            };
            return model;
        }
    }
}