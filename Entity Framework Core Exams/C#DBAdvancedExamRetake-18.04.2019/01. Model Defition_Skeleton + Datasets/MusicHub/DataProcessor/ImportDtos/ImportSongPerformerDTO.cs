﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class ImportSongPerformerDTO
    {
        [MinLength(3), MaxLength(20)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(3), MaxLength(20)]
        [Required]
        public string LastName { get; set; }

        [Range(18, 70)]
        [Required]
        public int Age { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Required]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public ImportSongIdDTO[] SongIds { get; set; }
    }
}
