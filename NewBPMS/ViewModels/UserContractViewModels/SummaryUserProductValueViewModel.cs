using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewBPMS.ViewModels.UserContractViewModels
{
    /// <summary>
    /// 汇总用产值视图模型（列表）
    /// </summary>
    public class SummaryUserProductValueViewModel
    {

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "姓名")]
        public string StaffName { get; set; }

        [Display(Name = "工号")]
        public int StaffNo { get; set; }

        [Display(Name = "产值")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

    }
}
