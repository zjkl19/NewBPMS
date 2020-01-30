using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class ContractIndexViewModel
    {
        public string StatusMessage { get; set; }

        [Display(Name = "合同编号")]
        public string ContractNo { get; set; }

        [Display(Name = "合同名称")]
        public string ContractName { get; set; }

        [Display(Name = "只看我负责的合同/项目")]
        public bool OnlyMe { get; set; }

        public IEnumerable<ContractViewModel> ContractViewModels { get; set; }
    }
}
