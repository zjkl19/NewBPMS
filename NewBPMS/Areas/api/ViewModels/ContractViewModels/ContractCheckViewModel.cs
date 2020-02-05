using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.api.ViewModels.ContractViewModels
{
    public class ContractAPIReviewViewModel
    {
        public string StatusMessage { get; set; }

        public IEnumerable<ContractViewModel> ContractViewModels { get; set; }
    }
}
