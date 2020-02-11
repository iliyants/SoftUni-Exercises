using SIS.MvcFramework.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.App.ViewModels
{
    public class SubmissionCreateViewModelPOST
    {
        [RequiredSis]
        [StringLengthSis(30,800, "Code length should be bewteen 30 and 800 characters")]
        public string Code { get; set; }
    }
}
