using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class EditContractViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "编号")]
        public string No { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }


        [Display(Name = "总金额")]    //（仅含本所检测项目金额）
        public decimal TotalAmount { get; set; }

        [Required]
        //合同金额（仅含本所检测项目金额，扣除如监控、维养费用以及其它所检测费用）
        [Display(Name = "合同金额")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "签订日期")]
        [DataType(DataType.Date)]
        public DateTime SignedDate { get; set; }

        [Required]
        [Display(Name = "期限")]    //合同有规定则填合同期限，没有规定由项目负责人填写估计期限
        public int Deadline { get; set; }

        [Required]
        [Display(Name = "约定工作内容")]
        public string JobContent { get; set; }

        [Display(Name = "项目地点")]
        public string ProjectLocation { get; set; }

        [Display(Name = "委托单位")]
        public string Client { get; set; }

        [Display(Name = "委托单位联系人")]
        public string ClientContactPerson { get; set; }

        [Display(Name = "委托单位联系人电话")]
        public string ClientContactPersonPhone { get; set; }

        //已所有报告出具作为界定合同完成的唯一标准
        [Display(Name = "合同完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishDateTime { get; set; }

        /// <summary>
        /// 承接方式
        /// </summary>
        public int AcceptWay { get; set; }

        /// <summary>
        /// 合同签订状态
        /// </summary>
        public int SignStatus { get; set; }


        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitDateTime { get; set; } = new DateTime(9999, 1, 1);


        /// <summary>
        /// 变更记录
        /// </summary>
        public string ChangeLog { get; set; }

        /// <summary>
        /// 1份合同只能由1位职工负责
        /// </summary>
        public string UserId { get; set; }
        [Display(Name = "负责人")]
        public string UserName { get; set; }
    }
}
