﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.RoleViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }
        
        [Display(Name = "描述")]
        public string Description { get; set; }

    }
}
