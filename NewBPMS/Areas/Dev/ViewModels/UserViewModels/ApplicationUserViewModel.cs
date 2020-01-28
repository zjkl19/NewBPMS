using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.UserViewModels
{
    public class ApplicationUserViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "工号")]
        public int StaffNo { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "关联职工Id")]
        public Guid StaffId { get; set; }

        [Display(Name = "关联职工姓名")]
        public string StaffName { get; set; }
    }
}
