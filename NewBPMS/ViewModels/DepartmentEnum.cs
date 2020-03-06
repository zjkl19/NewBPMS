using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels
{
    /// <summary>
    /// 500以上为分公司
    /// </summary>
    public class DepartmentEnum
    {
        public enum Department
        {

            [Display(Name = "综合行政部")]
            Composite = 1,
            [Display(Name = "市场经营部")]
            Market = 2,
            [Display(Name = "技术质量部")]
            Tech = 3,
            [Display(Name = "地基基础研究所")]
            Foundation = 4,
            [Display(Name = "建筑环境设备与节能检测研究所")]
            Equip = 5,
            [Display(Name = "环境检测与污染防治研究所")]
            Environment = 6,
            [Display(Name = "建筑物可靠性鉴定与抗震研究所")]
            Relia = 7,
            [Display(Name = "结构检测研究所")]
            Struct = 8,
            [Display(Name = "材料测试分析研究所")]
            Material = 9,
            [Display(Name = "路桥检测研究所")]
            RoadAndBridge = 10,
            [Display(Name = "绿色建筑与节能研究所")]
            EnergySaving = 11,
            [Display(Name = "厦门分公司")]
            XiamenBranch = 501,
            [Display(Name = "泉州分公司")]
            QuanzhouBranch = 502,
            [Display(Name = "漳州分公司")]
            ZhangzhouBranch = 503,
            [Display(Name = "龙岩分公司")]
            LongyanBranch = 504,
            [Display(Name = "莆田分公司")]
            PutianBranch = 505,
            [Display(Name = "三明分公司")]
            SanmingBranch = 506,
            [Display(Name = "长乐分公司")]
            ChangleBranch = 507,
            [Display(Name = "南平分公司")]
            NanpingBranch = 508,
            [Display(Name = "平潭分公司")]
            PingtanBranch = 509,

            [Display(Name = "其它")]
            Others = 999,
        }
    }
}
