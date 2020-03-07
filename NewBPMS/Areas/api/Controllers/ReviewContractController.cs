using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBPMS.Areas.api.ViewModels.ContractViewModels;
using NewBPMS.IControllerServices;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels.ContractViewModels;

namespace NewBPMS.Areas.api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewContractController : ControllerBase
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IContractService _contractService;
        private readonly IContractRepository _contractRepository;
        private readonly IUserContractRepository _userContractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ReviewContractController(
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

        [Authorize(Roles = "ContractReviewer")]
        [HttpGet]
        public ActionResult<ContractReviewViewModel> Review()
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
            return model;
        }

        // PUT: api/Contracts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "ContractReviewer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(Guid id, Contract contract)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id == null || contract.Id==null)
            {
                return NotFound();
            }

            if (id != contract.Id)
            {
                return BadRequest();
            }

            var p = _contractRepository.ContextSet.Where(x => x.Id == contract.Id).AsNoTracking().FirstOrDefault();
            p.ReviewStatus = (int)ReviewStatus.Reviewed;
            p.ReviewDateTime = DateTime.Now;
            p.ReviewUserName = user.StaffName;

            try
            {
                await _contractRepository.EditAsync(contract);
                
                //判断是审核还是回退操作
                if(contract.ReviewStatus==(int)ReviewStatus.Reviewed)
                {
                    StatusMessage = $"已审核\"{contract.Name}\"";
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