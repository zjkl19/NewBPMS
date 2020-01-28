using Microsoft.AspNetCore.Mvc;
using NewBPMS.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.UserContractViewModels
{
    public class EditUserContractViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "姓名")]
        public string StaffName { get; set; }

        [Required]
        [Display(Name = "分工")]
        public Labor Labor { get; set; }

        [Required]
        [Remote(action: "IsTotalRatioNotGreaterThenOneForEdit", controller: "UserContract", AdditionalFields = "Id,ContractId")]

        [Display(Name = "产值比例")]
        public decimal Ratio { get; set; }

        [Display(Name = "产值")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [HiddenInput]
        public Guid ContractId { get; set; }
    }
}
