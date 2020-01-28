using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.UserRoleViewModels
{
    public class UserRoleListViewModel
    {
        [ScaffoldColumn(false)]
        public string UserId { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "关联职工姓名")]
        public string StaffName { get; set; }
        public IEnumerable<UserRoleViewModel> UserRoleViewModels { get; set; }
        public string StatusMessage { get; set; }
    }
}
