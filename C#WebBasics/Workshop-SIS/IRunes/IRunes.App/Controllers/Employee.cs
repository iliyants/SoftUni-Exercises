using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IRunes.App.Controllers
{
    [XmlType]
    public class Employee
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("position")]
        public string Position { get; set; }

        [XmlElement("age")]
        public int Age { get; set; }

        [XmlElement("salary")]
        public decimal Salary { get; set; }
    }
}
