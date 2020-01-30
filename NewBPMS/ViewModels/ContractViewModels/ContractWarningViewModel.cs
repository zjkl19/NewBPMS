using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    /// <summary>
    /// 延期及延期预警合同列入警告范围
    /// </summary>
    public class ContractWarningViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "合同编号")]
        public string No { get; set; }

        [Display(Name = "合同名称")]
        public string Name { get; set; }

        [Display(Name = "签订日期")]
        [DataType(DataType.Date)]
        public DateTime SignedDate { get; set; }

        [Display(Name = "期限")]
        public int Deadline { get; set; }

        [Display(Name = "延期天数")]
        public int DelayDays { get; set; }

        [Display(Name = "预警天数")]
        public int DelayWarningDays { get; set; }

        public string UserId { get; set; }
        [Display(Name = "负责人")]
        public string UserName { get; set; }
    }
}
