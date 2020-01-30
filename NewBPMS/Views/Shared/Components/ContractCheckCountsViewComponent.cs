using Microsoft.AspNetCore.Mvc;
using NewBPMS.IRepository;
using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Views.Shared.Components
{
    public class ContractCheckCountsViewComponent : ViewComponent
    {
        private IContractRepository _contractRepository;
        public ContractCheckCountsViewComponent(
            IContractRepository contractRepository
            )
        {
            _contractRepository = contractRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            int count = (from p in _contractRepository.EntityItems
                         where
                         p.CheckStatus == (int)CheckStatus.NotChecked
                         && p.SubmitStatus == (int)SubmitStatus.Submitted
                         select p).Count();

            return View(count);
        }
    }
}
