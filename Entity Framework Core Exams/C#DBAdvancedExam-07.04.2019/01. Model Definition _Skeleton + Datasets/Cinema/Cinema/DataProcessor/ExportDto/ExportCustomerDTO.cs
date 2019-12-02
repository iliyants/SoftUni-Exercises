using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    public class ExportCustomerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Balance { get; set; }
    }
}
