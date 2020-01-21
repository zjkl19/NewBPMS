using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.UserContractViewModels
{
    /// <summary>
    /// 产值汇总查询视图模型
    /// </summary>
    public class SummaryUserProductValueQueryViewModel
    {
        [Display(Name = "请输入工号")]
        [UIHint("MultilineText")]
        public string StaffNoString { get; set; }

        [Display(Name = "起始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //下一个月1号减1天，即上一个月月末
        [Display(Name = "终止日期")]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; } = (DateTime.Now.Month != 12) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1) : new DateTime(DateTime.Now.Year, 12, 31);

        [Display(Name = "排序方式")]
        public SummaryPdtValueOrderType SummaryPdtValueOrderType { get; set; } = SummaryPdtValueOrderType.Input;
    }
    public enum SummaryPdtValueOrderType
    {
        [Display(Name = "按工号输入顺序")]
        Input = 1,
        [Display(Name = "按产值，从高到低")]
        PdtValue = 2,
    }
}
