using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Models
{
    /// <summary>
    /// 用户-项目关联表
    /// </summary>
    public class UserContract
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 分工
        /// </summary>
        public int Labor { get; set; }

        /// <summary>
        /// 产值比例
        /// </summary>
        public decimal Ratio { get; set; }

        /// <summary>
        /// 标准产值
        /// </summary>
        //[Column(TypeName = "money")]
        //public decimal? StandardValue { get; set; }

        /// <summary>
        /// 换算标准产值
        /// </summary>
        //[Column(TypeName = "money")]
        //public decimal? CalcValue { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public string UserName { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Contract")]
        public Guid ContractId { get; set; }

        public string ContractName { get; set; }
        public virtual Contract Contract { get; set; }


    }
}
