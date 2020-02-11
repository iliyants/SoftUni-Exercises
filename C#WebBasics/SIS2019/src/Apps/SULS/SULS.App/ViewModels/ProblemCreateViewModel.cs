using SIS.MvcFramework.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.App.ViewModels
{
    public class ProblemCreateViewModel
    {
        [RequiredSis]
        [StringLengthSis(5,20, "Problem name should consist of minimum 5 and maxmum 20 characters")]
        public string Name { get; set; }

        [RequiredSis]
        [RangeSis(50,300, "Points must be in range 50 - 300")]
        public int Points { get; set; }
    }
}
