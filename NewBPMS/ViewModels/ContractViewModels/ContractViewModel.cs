using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class ContractViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "编号")]
        public string No { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 合同总金额（仅含本所检测项目金额）
        /// </summary>
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 合同金额（仅含本所检测项目金额，扣除监控费用以及其它所检测费用）
        /// </summary>
        [Display(Name = "合同额")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "签订日期")]
        [DataType(DataType.Date)]
        public DateTime SignedDate { get; set; }

        [Display(Name = "期限")]
        public int Deadline { get; set; }

        /// <summary>
        /// 合同负责人约定期限
        /// </summary>
        public int PromisedDeadline { get; set; }

        [Display(Name = "工作内容")]
        public string JobContent { get; set; }

        /// <summary>
        /// 项目地点
        /// </summary>
        public string ProjectLocation { get; set; }

        /// <summary>
        /// 委托单位
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// 委托单位联系人
        /// </summary>
        public string ClientContactPerson { get; set; }

        /// <summary>
        /// 委托单位联系人电话
        /// </summary>
        public string ClientContactPersonPhone { get; set; }

        /// <summary>
        /// 承接方式
        /// </summary>
        public int AcceptWay { get; set; }

        /// <summary>
        /// 合同签订状态
        /// </summary>
        public int SignStatus { get; set; }

        /// <summary>
        /// 变更记录
        /// </summary>
        public string ChangeLog { get; set; }

        /// <summary>
        /// 提交状态
        /// </summary>
        public SubmitStatus SubmitStatus { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitDateTime { get; set; }

        /// <summary>
        /// 校核状态
        /// </summary>
        public CheckStatus CheckStatus { get; set; }
        /// <summary>
        /// 校核时间
        /// </summary>
        public DateTime CheckDateTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ReviewDateTime { get; set; } = new DateTime(9999, 1, 1);
        /// <summary>
        /// 完成状态
        /// </summary>
        public FinishStatus FinishStatus { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [Display(Name = "合同完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishDateTime { get; set; }

        /// <summary>
        /// 1份合同只能由1位职工负责
        /// </summary>
        public string UserId { get; set; }
        [Display(Name = "负责人")]
        public string UserName { get; set; }
        [Display(Name = "备注")]
        public string Comment { get; set; }
    }

    public enum SubmitStatus
    {
        [Display(Name = "未提交")]
        NotSubmitted = 0,
        [Display(Name = "已提交")]
        Submitted = 1,
    }

    public enum CheckStatus
    {
        [Display(Name = "未校对")]
        NotChecked = 0,
        [Display(Name = "已校对")]
        Checked = 1,
    }

    public enum ReviewStatus
    {
        [Display(Name = "未审核")]
        NotReviewed = 0,
        [Display(Name = "已审核")]
        Reviewed = 1,
    }

    public enum FinishStatus
    {
        [Display(Name = "未完成")]
        NotFinished = 0,
        [Display(Name = "已完成")]
        Finished = 1
    }
}
