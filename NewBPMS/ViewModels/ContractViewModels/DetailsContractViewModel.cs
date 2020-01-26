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

        public ContractViewModel ContractViewModel { get; set; }

        public IEnumerable<UserProductValueViewModel> UserProductValueViewModels { get; set; }

    }

}

