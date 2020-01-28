using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.UserRoleViewModels
{
    public class CreateUserRoleViewModel
    {
        public string StatusMessage { get; set; }
        [HiddenInput]
        public string UserId { get; set; }

        public List<RoleSelectViewModel> RoleSelectViewModels { get; set; }

        public string RoleSelect { get; set; }
    }
}
