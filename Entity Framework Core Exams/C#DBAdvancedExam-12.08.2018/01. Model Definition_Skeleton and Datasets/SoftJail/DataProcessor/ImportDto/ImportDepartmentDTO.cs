using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDTO
    {
        [Required]
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }

        public ImportCellDTO[] Cells { get; set; }
    }
}
