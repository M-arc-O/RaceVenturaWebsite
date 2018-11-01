using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.Models
{
    [Table("StageLinks")]
    public class StageLinks
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public int RaceId { get; set; }

        public StageLinks()
        {

        }
    }
}
