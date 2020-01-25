using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.UserContractViewModels
{
    public class ListViewModel
    {
        [Display(Name = "起始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        [Display(Name = "终止日期")]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; } = (DateTime.Now.Month != 12) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1) : new DateTime(DateTime.Now.Year, 12, 31);

        public List<UserSelectViewModel> UserSelectListViewModel { get; set; }

        public List<string> ItemChosen { get; set; }
    }
}
