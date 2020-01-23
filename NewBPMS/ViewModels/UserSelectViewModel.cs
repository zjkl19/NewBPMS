using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels
{
    public class UserSelectViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Display(Name = "姓名")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "工号")]
        public int No { get; set; }

    }
}
