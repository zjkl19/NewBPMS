using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class DetailsContractViewModel
    {
        public string StatusMessage { get; set; }

        /// <summary>
        /// 已分配产值比例
        /// </summary>
        [Display(Name = "产值已分配")]
        public decimal PdtPercent { get; set; }

        public ContractViewModel ContractViewModel { get; set; }

        public IEnumerable<UserProductValueViewModel> UserProductValueViewModels { get; set; }

    }

}

