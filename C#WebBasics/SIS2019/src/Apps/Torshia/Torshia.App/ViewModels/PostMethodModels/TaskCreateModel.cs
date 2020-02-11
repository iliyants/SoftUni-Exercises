using SIS.MvcFramework.Attributes.Validation;
using System;

namespace Torshia.App.ViewModels.PostMethodModels
{
    public class TaskCreateModel
    {
        public string DueDate { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 20, "")]
        public string Title { get; set; }

        [RequiredSis]
        [StringLengthSis(10, 200, "")]
        public string Description { get; set; }

        [RequiredSis]
        public string Participants { get; set; }

        public string Customers { get; set; }

        public string Marketing { get; set; }

        public string Finances { get; set; }

        public string Internal { get; set; }

        public string Management { get; set; }
    }
}
