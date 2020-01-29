using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class ContractWarningIndexViewModel
    {
        public string StatusMessage { get; set; }

        public IEnumerable<ContractWarningViewModel> ContractDelayViewModels { get; set; }

        public IEnumerable<ContractWarningViewModel> ContractDelayWarningViewModels { get; set; }
    }
}
