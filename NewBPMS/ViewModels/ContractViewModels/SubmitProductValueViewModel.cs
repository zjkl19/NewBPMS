using NewBPMS.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class SubmitProductValueViewModel
    {
        public Guid ContractId { get; set; }

        [Required]
        [Display(Name ="完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishDateTime { get; set; }
        //[Range(typeof(bool),"true","true",ErrorMessage ="你必须先进行确认")]
        [MustBeTrue(ErrorMessage ="你需要先进行确认")]
        public string TermsAccepted { get; set; }
    }
}
