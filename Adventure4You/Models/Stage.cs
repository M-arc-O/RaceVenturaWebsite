using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.Models
{
    [Table("Stages")]
    public class Stage
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? MimimumPointsToCompleteStage { get; set; }

        public Stage()
        {

        }
    }
}
