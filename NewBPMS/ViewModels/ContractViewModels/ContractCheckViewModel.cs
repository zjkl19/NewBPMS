using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class ContractCheckViewModel
    {
        public string StatusMessage { get; set; }

        public IEnumerable<ContractViewModel> ContractViewModels { get; set; }
    }
}
