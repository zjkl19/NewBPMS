﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels.ContractViewModels
{
    public class SubmitProductValueViewModel
    {
        public Guid ContractId { get; set; }

        [Range(typeof(bool),"true","true",ErrorMessage ="你必须先进行确认")]
        public bool TermsAccepted { get; set; }
    }
}
