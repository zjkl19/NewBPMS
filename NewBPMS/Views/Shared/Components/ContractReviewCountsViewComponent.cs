using Microsoft.AspNetCore.Mvc;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace NewBPMS.Views.Shared.Components
{
    public class ContractReviewCountsViewComponent : ViewComponent
    {
        private IContractRepository _contractRepository;
        public ContractReviewCountsViewComponent(
            IContractRepository contractRepository
            )
        {
            _contractRepository = contractRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            var count = await _contractRepository.ContextSet.Where(p=>
                     p.CheckStatus == (int)CheckStatus.Checked
                     && p.ReviewStatus == (int)ReviewStatus.NotReviewed
                     && p.SubmitStatus == (int)SubmitStatus.Submitted).CountAsync();

            //int count = (from p in _contractRepository.EntityItems
            //             where
            //             p.CheckStatus == (int)CheckStatus.Checked
            //             && p.ReviewStatus == (int)ReviewStatus.NotReviewed
            //             && p.SubmitStatus == (int)SubmitStatus.Submitted
            //             select p).Count();

            return View(count);
        }
    }
}
