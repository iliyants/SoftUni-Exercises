﻿using System.Xml.Serialization;
namespace CarDealer.Dtos.Import
{
    [XmlType("Car")]
    public class ImportCarWIthPartIdsDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public ImportPartIdDTO[] PartIds { get; set; }
    }
}
