using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.UserRoleViewModels
{
    public class UserRoleViewModel
    {
        [Display(Name = "角色名")]
        public string RoleName { get; set; }
    }
}
