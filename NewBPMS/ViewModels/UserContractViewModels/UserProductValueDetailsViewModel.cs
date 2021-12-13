using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.UserContractViewModels
{
    public class UserProductValueDetailsViewModel
    {
        [Display(Name = "工号")]
        public int StaffNo { get; set; }

        [Display(Name = "姓名")]
        public string StaffName { get; set; }

        [Display(Name = "合同编号")]
        public string ContractNo { get; set; }

        [Display(Name = "合同名称")]
        public string ContractName { get; set; }

        [Display(Name = "合同完成时间")]
        [DataType(DataType.Date)]
        public DateTime ContractFinishDateTime { get; set; }

        [Display(Name = "分工")]
        public Labor Labor { get; set; }

        [Display(Name = "占合同比例")]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public decimal Ratio { get; set; }

        [Display(Name = "金额")]
        public decimal Amount { get; set; }
    }
}
