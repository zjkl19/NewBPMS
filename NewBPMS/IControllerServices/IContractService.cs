using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.IControllerServices
{
    public interface IContractService
    {
        IEnumerable<ContractViewModel> GetContractIndexlinqVar(string ContractNo = "", string ContractName = "");
        /// <summary>
        /// 获取某个合同下的各人产值列表
        /// </summary>
        /// <param name="Id">合同Id</param>
        /// <returns></returns>
        IEnumerable<UserProductValueViewModel> GetUserProductValue(Guid Id);
    }
}
