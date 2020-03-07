using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels.ContractViewModels;

namespace NewBPMS.Areas.api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckContractController : ControllerBase
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IContractService _contractService;
        private readonly IContractRepository _contractRepository;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CheckContractController(
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

        [Authorize(Roles = "ContractChecker")]
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

        // PUT: api/Contracts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "ContractChecker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(Guid id, Contract contract)
        {
            if (id == null || contract.Id==null)
            {
                return NotFound();
            }

            if (id != contract.Id)
            {
                return BadRequest();
            }
            //var user = await _userManager.GetUserAsync(User);

            //Obsoleted code
            //var p = await _contractRepository.QueryByIdAsync(id);
            //p.CheckStatus = (int)CheckStatus.Checked;
            //p.CheckDateTime = DateTime.Now;
            //p.CheckUserName = user.StaffName;
            //p.CheckUserName = "api测试";

            try
            {
                await _contractRepository.EditAsync(contract);
                
                //判断是校核还是回退操作
                if(contract.CheckStatus==(int)CheckStatus.Checked)
                {
                    StatusMessage = $"已校核\"{contract.Name}\"";
                }
                else
                {
                    StatusMessage = $"已回退\"{contract.Name}\"";
                }
                
            }
            catch (DbUpdateException ex)
            {

                throw;
            }

            return Content(StatusMessage);
        }
    }
}