using NewBPMS.ViewModels.UserContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.IControllerServices
{
    public interface IUserContractService
    {
        IEnumerable<SummaryUserProductValueViewModel> GetSummaryUserProductValue(SummaryUserProductValueQueryViewModel queryModel);
    }
}
