using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.UserViewModels
{
    public class UserIndexViewModel
    {
        public string StatusMessage { get; set; }

        [Display(Name = "职工姓名")]
        public string StaffName { get; set; }

        public IEnumerable<ApplicationUserViewModel> ApplicationUserViewModels { get; set; }
    }
}
