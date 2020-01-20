using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.IControllerServices
{
    public interface IContractService
    {
        IEnumerable<ContractViewModel> GetContractIndexlinqVar(string ContractName = "");
    }
}
