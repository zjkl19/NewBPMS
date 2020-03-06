using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 关联职工表Id
        /// </summary>
        [PersonalData]
        public Guid StaffId { get; set; }

        /// <summary>
        /// 职工姓名
        /// </summary>
        [PersonalData]
        public string StaffName { get; set; }

        /// <summary>
        /// 职工工号
        /// </summary>
        [PersonalData]
        public int StaffNo { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [PersonalData]
        public int Department { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [PersonalData]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 负责合同
        /// </summary>
        public virtual ICollection<Contract> Contracts { get; }
        /// <summary>
        /// 承接合同
        /// </summary>
        public virtual ICollection<Contract> AcceptContracts { get; }

        public virtual ICollection<UserContract> UserContracts { get;}
        
    }
}
