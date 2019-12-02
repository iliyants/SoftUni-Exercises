using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data.Models
{
    public class GameTag
    {
        [Key]
        [Required]
        public int GameId { get; set; }

        public Game Game { get; set; }

        [Key]
        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; }


    }
}
