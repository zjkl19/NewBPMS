using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.Areas.Dev.ViewModels.RoleViewModels
{
    public class RoleIndexViewModel
    {
        public string StatusMessage { get; set; }

        public IEnumerable<RoleViewModel> RoleViewModels { get; set; }
    }
}
